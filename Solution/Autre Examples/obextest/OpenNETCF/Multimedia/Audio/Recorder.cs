//==========================================================================================
//
//		OpenNETCF.Multimedia.Audio.Recorder
//		Copyright (c) 2003, OpenNETCF.org
//
//		This library is free software; you can redistribute it and/or modify it under 
//		the terms of the OpenNETCF.org Shared Source License.
//
//		This library is distributed in the hope that it will be useful, but 
//		WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
//		FITNESS FOR A PARTICULAR PURPOSE. See the OpenNETCF.org Shared Source License 
//		for more details.
//
//		You should have received a copy of the OpenNETCF.org Shared Source License 
//		along with this library; if not, email licensing@opennetcf.org to request a copy.
//
//		If you wish to contact the OpenNETCF Advisory Board to discuss licensing, please 
//		email licensing@opennetcf.org.
//
//		For general enquiries, email enquiries@opennetcf.org or visit our website at:
//		http://www.opennetcf.org
//
//==========================================================================================
using System;
using System.IO;
using OpenNETCF.Win32;
using System.Threading;
using System.Collections;

namespace OpenNETCF.Multimedia.Audio
{
	/// <summary>
	/// Recorder class. Wraps low-level WAVE API for recording purposes
	/// </summary>
	public class Recorder : Audio
	{
		/// <summary>
		/// Handles the event that is fired when wave device is successfully opened
		/// </summary>
		public event WaveOpenHandler		WaveOpen;
		/// <summary>
		/// Handles the event that is fired when wave device is successfully closed
		/// </summary>
		public event WaveCloseHandler		WaveClose;
		/// <summary>
		/// Handles the event that is fired when recording is stopped (on timer or by calling <see cref="Recorder.Stop">Stop</see> method
		/// </summary>
		public event WaveFinishedHandler	DoneRecording;

		private IntPtr				m_hWaveIn	= IntPtr.Zero;
#if !NDOC
		private SoundMessageWindow	m_recmw;
#endif
		private bool				recording		= false;
		private bool				recordingFinished = false;
		private int					m_recBufferSize;
		private WaveFormatEx		m_recformat;
		private RiffChunk			m_ck;
		private Stream				m_streamRecord;
		// Recoding timer
		private Timer				m_recTimer;

		/// <summary>
		/// Whether the Recorder is presently recording
		/// </summary>
		public bool Recording { get { return recording; } }

		/// <summary>
		/// Creates Recorder object and attaches it to the default wave device
		/// </summary>
		public Recorder()
		{
#if NDOC
	throw new Exception("This is an NDOC Build, not a build for use!");
#endif
			m_qBuffers = new Queue(MaxBuffers);
			m_HandleMap = new Hashtable(MaxBuffers);
		}

		/// <summary>
		/// Creates Recorder object and attaches it to the given wave device
		/// </summary>
		/// <param name="AudioDeviceID">Wave device ID</param>
		public Recorder(int AudioDeviceID) : this()
		{			
			m_deviceID = AudioDeviceID;
		}

		/// <summary>
		/// A list of PCM wave formats supported by the default device
		/// </summary>
		/// <returns>SoundFormats collection</returns>
		public SoundFormats SupportedRecordingFormats()
		{
			return SupportedRecordingFormats(0);
		}
			
		/// <summary>
		/// A list of PCM wave formats supported by the given device
		/// </summary>
		/// <param name="DeviceID">Wave device</param>
		/// <returns>SoundFormats collection</returns>
		public SoundFormats SupportedRecordingFormats(int DeviceID)
		{
			Core.WaveInCaps wic = new OpenNETCF.Win32.Core.WaveInCaps();

			int ret = Core.waveInGetDevCaps(DeviceID, wic.ToByteArray(), wic.ToByteArray().Length);
			CheckWaveError(ret);

			return (SoundFormats)wic.Formats;
		}

		/// <summary>
		/// Number of wave input devices in the system
		/// </summary>
		public static int NumDevices { get { return Core.waveInGetNumDevs(); } }

		/// <summary>
		/// Stop recording operation currently in progress. 
		/// Throws an error if no recording operation is in progress
		/// </summary>
		public void Stop()
		{
            if (!recordingFinished)
            {
                m_recTimer.Dispose();
                CheckWaveError( Core.waveInReset(m_hWaveIn) );
                recordingFinished = true;
            }
        }

		/// <summary>
		/// Record sound data for specified number of seconds at 11025 sps and 1 channel
		/// The stream will be a properly formatted RIFF file
		/// </summary>
		/// <param name="st">Stream into which recorded samples are written</param>
		/// <param name="Seconds">Seconds of data to record</param>
		public void RecordFor(Stream st, short Seconds)
		{
			RecordFor(st, Seconds, SoundFormats.Mono8bit11kHz);
		}

		/// <summary>
		/// Record sound data for specified number of seconds using given wave format
		/// The stream will be a properly formatted RIFF file
		/// </summary>
		/// <param name="st">Stream into which recorded samples are written</param>
		/// <param name="Seconds">Seconds of data to record</param>
		/// <param name="SoundFormat">Sound format to record in.</param>
		public void RecordFor(Stream st, short Seconds, SoundFormats SoundFormat)
		{
			m_hWaveIn	= IntPtr.Zero;

			// only allow 1 recording session at a time
			if(recording)
			{
				throw new InvalidOperationException("Already recording");
			}

			// set our global flag
			recording = true;

			if ( m_qBuffers == null )
				m_qBuffers = new Queue(MaxBuffers);
			if ( m_HandleMap  == null )
				m_HandleMap = new Hashtable(MaxBuffers);

			m_recformat	= new WaveFormatEx();
#if !NDOC
			// create the callback message window
			m_recmw = new SoundMessageWindow();
			m_recmw.WaveDoneMessage +=new WaveDoneHandler(m_recmw_WaveDoneMessage);
			m_recmw.WaveCloseMessage +=new WaveCloseHandler(mw_WaveCloseMessage);
			m_recmw.WaveOpenMessage += new WaveOpenHandler(mw_WaveOpenMessage);
#endif
			// set up format
			#region long format switch()
			switch(SoundFormat)
			{
				case SoundFormats.Mono16bit11kHz:
					m_recformat.Channels = 1;
					m_recformat.SamplesPerSec = 11025;
					m_recformat.BitsPerSample = 16;
					break;
				case SoundFormats.Mono16bit22kHz:
					m_recformat.Channels = 1;
					m_recformat.SamplesPerSec = 22050;
					m_recformat.BitsPerSample = 16;
					break;
				case SoundFormats.Mono16bit44kHz:
					m_recformat.Channels = 1;
					m_recformat.SamplesPerSec = 44100;
					m_recformat.BitsPerSample = 16;
					break;
				case SoundFormats.Mono8bit11kHz:
					m_recformat.Channels = 1;
					m_recformat.SamplesPerSec = 11025;
					m_recformat.BitsPerSample = 8;
					break;
				case SoundFormats.Mono8bit22kHz:
					m_recformat.Channels = 1;
					m_recformat.SamplesPerSec = 22050;
					m_recformat.BitsPerSample = 8;
					break;
				case SoundFormats.Mono8bit44kHz:
					m_recformat.Channels = 1;
					m_recformat.SamplesPerSec = 44100;
					m_recformat.BitsPerSample = 8;
					break;
				case SoundFormats.Stereo16bit11kHz:
					m_recformat.Channels = 2;
					m_recformat.SamplesPerSec = 11025;
					m_recformat.BitsPerSample = 16;
					break;
				case SoundFormats.Stereo16bit22kHz:
					m_recformat.Channels = 2;
					m_recformat.SamplesPerSec = 22050;
					m_recformat.BitsPerSample = 16;
					break;
				case SoundFormats.Stereo16bit44kHz:
					m_recformat.Channels = 2;
					m_recformat.SamplesPerSec = 44100;
					m_recformat.BitsPerSample = 16;
					break;
				case SoundFormats.Stereo8bit11kHz:
					m_recformat.Channels = 2;
					m_recformat.SamplesPerSec = 11025;
					m_recformat.BitsPerSample = 8;
					break;
				case SoundFormats.Stereo8bit22kHz:
					m_recformat.Channels = 2;
					m_recformat.SamplesPerSec = 22050;
					m_recformat.BitsPerSample = 8;
					break;
				case SoundFormats.Stereo8bit44kHz:
					m_recformat.Channels = 2;
					m_recformat.SamplesPerSec = 44100;
					m_recformat.BitsPerSample = 8;
					break;
			}
			#endregion long format switch()

			m_recformat.FormatTag = WAVE_FORMAT_PCM;
			m_recformat.AvgBytesPerSec = m_recformat.SamplesPerSec * m_recformat.Channels;
			m_recformat.BlockAlign = (short)((m_recformat.Channels * m_recformat.BitsPerSample) / 8);
			m_recformat.Size = 0;

			m_ck = new RiffChunk(st);
			m_ck.BeginWrite();
			WaveChunk wck = m_ck.CreateSubChunk(FourCC.Wave, st) as WaveChunk;
			wck.BeginWrite();
			FmtChunk fck = wck.CreateSubChunk(FourCC.Fmt, st) as FmtChunk;
			fck.BeginWrite();
			fck.WaveFormat = m_recformat;
			fck.Write(fck.WaveFormat.GetBytes());
			fck.EndWrite();
			DataChunk dck = wck.CreateSubChunk(FourCC.Data, st) as DataChunk;
			m_streamRecord = dck.BeginWrite();
#if !NDOC
			// check for support of selected format
			CheckWaveError(Core.waveInOpen(out m_hWaveIn, WAVE_MAPPER, m_recformat, IntPtr.Zero, 0, WAVE_FORMAT_QUERY));

			// open wave device
			CheckWaveError(Core.waveInOpen(out m_hWaveIn, (uint)m_deviceID, m_recformat, m_recmw.Hwnd, 0, CALLBACK_WINDOW));

			m_recBufferSize = (int)(Math.Min( (int)Seconds, BufferLen ) * m_recformat.SamplesPerSec * m_recformat.Channels);
			
			for ( int i = 0; i < 2; i ++ )
			{
				WaveHeader hdr = GetNewRecordBuffer( m_recBufferSize );
		
				// send the buffer to the device
				CheckWaveError(Core.waveInAddBuffer(m_hWaveIn, hdr.Header, hdr.HeaderLength));
			}

			// begin recording
			CheckWaveError(Core.waveInStart(m_hWaveIn));
			recordingFinished = false;
			m_recTimer = new Timer(new TimerCallback(RecTimerCallback), this, Seconds * 1000, Timeout.Infinite);
#endif
		}

		private void RecTimerCallback(object state)
		{
			if(recording)
			{
				Stop();
			}
		}

		/// <summary>
		/// Creates a recording buffer
		/// </summary>
		/// <param name="dwBufferSize"></param>
		/// <returns>new buffer as WaveHeader</returns>
		private WaveHeader GetNewRecordBuffer(int dwBufferSize)
		{
			WaveHeader hdr = new WaveHeader(dwBufferSize);
			Monitor.Enter(m_HandleMap.SyncRoot);
			m_HandleMap.Add(hdr.Header.ToInt32(), hdr);
			Monitor.Exit(m_HandleMap.SyncRoot);
			// prepare the header
			CheckWaveError(Core.waveInPrepareHeader(m_hWaveIn, hdr.Header, hdr.HeaderLength));
			return hdr;
		}

		private void DumpRecordBuffers()
		{
			while( m_qBuffers.Count > 0 )
			{
				Monitor.Enter(m_qBuffers.SyncRoot);
				WaveHeader hdr = (WaveHeader)m_qBuffers.Dequeue();
				Monitor.Exit(m_qBuffers.SyncRoot);

				
				m_streamRecord.Write(hdr.GetData(), 0, (int)hdr.waveHdr.BytesRecorded);
				hdr.Dispose();
			}
		}

		private void mw_WaveOpenMessage(object sender)
		{
			if(WaveOpen != null)
			{
				WaveOpen(this);
			}
		}

		private void mw_WaveCloseMessage(object sender)
		{
			if(WaveClose != null)
			{
				WaveClose(this);
			}
		}

		private void m_recmw_WaveDoneMessage(object sender, IntPtr wParam, IntPtr lParam)
		{
#if !NDOC
			// Retrieve Waveheader object by the lpHeader pointer
			Monitor.Enter(m_HandleMap.SyncRoot);
			WaveHeader hdr = m_HandleMap[lParam.ToInt32()] as WaveHeader;
			m_HandleMap.Remove(hdr.Header.ToInt32());
			Monitor.Exit(m_HandleMap.SyncRoot);

			// unprepare the header
			CheckWaveError(Core.waveInUnprepareHeader(m_hWaveIn, hdr.Header, hdr.HeaderLength));
			hdr.RetrieveHeader();
			m_qBuffers.Enqueue(hdr);
				
			if ( recordingFinished ) // last chunk
			{
				DumpRecordBuffers();
				CheckWaveError(Core.waveInClose(m_hWaveIn));
				WaveChunk wck = m_ck[0] as WaveChunk;
				if ( wck != null )
				{
					DataChunk dck = wck.FindChunk(FourCC.Data) as DataChunk;
					if ( dck != null )
					{
						dck.EndWrite();
					}
					wck.EndWrite();
				}
				m_ck.EndWrite();
				m_streamRecord.Close();
				// clean up the messageWindow
				m_recmw.Dispose();

				// reset the global flag
				recording = false;
				// set our event
				if(DoneRecording != null)
				{
					DoneRecording();
				}
				
				foreach( WaveHeader whdr in m_HandleMap.Values )
					whdr.Dispose();
				m_HandleMap.Clear();
			}
			else
			{
				hdr = GetNewRecordBuffer(m_recBufferSize);
				CheckWaveError(Core.waveInAddBuffer(m_hWaveIn, hdr.Header, hdr.HeaderLength));
				DumpRecordBuffers();
			}
#endif
		}
	}
}
