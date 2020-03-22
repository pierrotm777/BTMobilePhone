//==========================================================================================
//
//		OpenNETCF.Multimedia.Audio.Player
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
using OpenNETCF.Win32;
using System.IO;
using System.Collections;
using System.Threading;

namespace OpenNETCF.Multimedia.Audio
{
	/// <summary>
	/// Wave Audio player class. Supports PCM waveform playback from the stream
	/// </summary>
	public class Player : Audio
	{
		/// <summary>
		/// Raised when the wave device is opened
		/// </summary>
		public event WaveOpenHandler		WaveOpen;
		/// <summary>
		/// Raised when the wave device is closed
		/// </summary>
		public event WaveCloseHandler		WaveClose;
		/// <summary>
		/// Raised when the wave device has finished playback
		/// </summary>
		public event WaveDoneHandler		DonePlaying;

		private WaveFormatEx		m_format;
		private IntPtr				m_hWaveOut		= IntPtr.Zero;
		private bool				m_playing		= false;
		//private bool				m_paused		= false;

		private ArrayList			m_mwArray;
		private ArrayList			m_streamArray;

		/// <summary>
		/// Constructs Player object on the default wave device
		/// </summary>
		public Player()
		{
			m_mwArray = new ArrayList();
			m_streamArray = new ArrayList();
			m_qBuffers = new Queue(MaxBuffers);
			m_HandleMap = new Hashtable(MaxBuffers);
		}

		/// <summary>
		/// Constructs Player object on the given wave device
		/// </summary>
		/// <param name="AudioDeviceID">Wave device ID</param>
		public Player(int AudioDeviceID) : this()
		{			
			m_deviceID =AudioDeviceID;
		}

		/// <summary>
		/// Number of the output wave devices in the system
		/// </summary>
		public static int NumDevices { get { return Core.waveOutGetNumDevs(); } }

		/// <summary>
		/// Restart a paused wave file
		/// </summary>
		public void Restart() { Core.waveOutRestart(m_hWaveOut); }

		/// <summary>
		/// Pause Play
		/// </summary>
		public void Pause() { Core.waveOutPause(m_hWaveOut); }

		/// <summary>
		/// Stop Play
		/// </summary>
		public void Stop() 
		{ 
			Core.waveOutReset(m_hWaveOut); 
			m_playing = false;
		}

		/// <summary>
		/// Gets or sets playback volume on the current wave device
		/// </summary>
		public int Volume
		{
			get
			{
				int vol = 0;
				CheckWaveError(Core.waveOutGetVolume(m_hWaveOut, ref vol));
				return vol;
			}
			set
			{
				CheckWaveError(Core.waveOutSetVolume(m_hWaveOut, Convert.ToInt32(value)));
			}
		}

		/// <summary>
		/// True, if the player is currently playing. False otherwise
		/// </summary>
		public bool Playing { get{ return m_playing; } }


		/// <summary>
		/// Plays waveform contained in the given stream. Stream is exepcted to contain full riff header
		/// </summary>
		/// <param name="playStream">Stream with the waveform</param>
		public void Play( Stream playStream )
		{
			if(m_playing) return;

			
			MMChunk ck = MMChunk.FromStream(playStream);
			if ( ck != null && ck.FourCC == FourCC.Riff )
			{
				if ( ck[0] is WaveChunk )
				{
					DataChunk dck = ck[0].FindChunk(FourCC.Data) as DataChunk;
					if ( dck != null )
					{
						playStream = dck.BeginRead();
					}
					FmtChunk fck = ck[0].FindChunk(FourCC.Fmt) as FmtChunk;
					if ( fck != null )
					{
						m_format = fck.WaveFormat;
					}
				}
			}
			if ( playStream == null )
			{
				throw new Exception("No valid WAV file has been opened");
			}

#if!NDOC

			if ( m_qBuffers == null )
				m_qBuffers = new Queue(MaxBuffers);
			if ( m_HandleMap == null )
				m_HandleMap = new Hashtable(MaxBuffers);

			// create a window to catch waveOutxxx messages
			SoundMessageWindow	mw = new SoundMessageWindow();

			// wire in events
			mw.WaveOpenMessage += new WaveOpenHandler(mw_WaveOpenMessage);
			mw.WaveCloseMessage += new WaveCloseHandler(mw_WaveCloseMessage);
			mw.WaveDoneMessage += new WaveDoneHandler(mw_WaveDoneMessage);

			// add it to the global array
			int i = m_mwArray.Add(mw);
			m_streamArray.Add(playStream);

			// open the waveOut device and register the callback
			CheckWaveError(Core.waveOutOpen(out m_hWaveOut, m_deviceID, m_format, ((SoundMessageWindow)m_mwArray[i]).Hwnd, 0, CALLBACK_WINDOW));

			RefillPlayBuffers();

			Monitor.Enter(m_qBuffers.SyncRoot);
			WaveHeader hdr = m_qBuffers.Dequeue() as WaveHeader;
			Monitor.Exit(m_qBuffers.SyncRoot);

			// play the file
			int ret = Core.waveOutWrite(m_hWaveOut, hdr.Header, hdr.HeaderLength);
			CheckWaveError(ret);

			m_playing = true;
			
#endif
		}

		// Add more buffers to the playback queue - keep it full
		private void RefillPlayBuffers()
		{
			while( m_qBuffers.Count < MaxBuffers )
			{
				int cb = (int)(BufferLen * m_format.SamplesPerSec * m_format.Channels);
				byte[] data = new byte[cb];

				for(int i = 0 ; i < m_streamArray.Count ; i++)
				{
					if( m_streamArray[i] != null )
					{
						cb = ((Stream)m_streamArray[i]).Read(data, 0, cb);
						break;
					}
				}

				if ( cb == 0 )
					break;

				WaveHeader hdr = new WaveHeader(data);
				Monitor.Enter(m_HandleMap.SyncRoot);
				m_HandleMap.Add(hdr.Header.ToInt32(), hdr);
				Monitor.Exit(m_HandleMap.SyncRoot);

				// prepare the header
				CheckWaveError(Core.waveOutPrepareHeader(m_hWaveOut, hdr.Header, hdr.HeaderLength));
				Monitor.Enter(m_qBuffers.SyncRoot);
				m_qBuffers.Enqueue(hdr);
				Monitor.Exit(m_qBuffers.SyncRoot);
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
#if !NDOC
			// remove the message window from the gloabal array
			for(int smw = 0 ; smw < m_mwArray.Count ; smw++)
			{
				if(((SoundMessageWindow)m_mwArray[smw]).Hwnd == ((SoundMessageWindow)sender).Hwnd)
				{
					// remove the messages
					((SoundMessageWindow)m_mwArray[smw]).WaveOpenMessage -= new WaveOpenHandler(mw_WaveOpenMessage);
					((SoundMessageWindow)m_mwArray[smw]).WaveCloseMessage -= new WaveCloseHandler(mw_WaveCloseMessage);
					((SoundMessageWindow)m_mwArray[smw]).WaveDoneMessage -= new WaveDoneHandler(mw_WaveDoneMessage);

					((SoundMessageWindow)m_mwArray[smw]).Dispose();
					m_mwArray.RemoveAt(smw);
					((Stream)m_streamArray[smw]).Close();
					m_streamArray.RemoveAt(smw);

					break;
				}
			}
			if(WaveClose != null)
			{
				WaveClose(this);
			}
#endif
			m_playing = false;
		}
		private void mw_WaveDoneMessage(object sender, IntPtr wParam, IntPtr lParam)
		{
			// free the header
			Monitor.Enter(m_HandleMap.SyncRoot);
			WaveHeader hdr = m_HandleMap[lParam.ToInt32()] as WaveHeader;
			m_HandleMap.Remove(lParam.ToInt32());
			Monitor.Exit(m_HandleMap.SyncRoot);
			CheckWaveError(Core.waveOutUnprepareHeader(m_hWaveOut, lParam, hdr.HeaderLength));
			hdr.Dispose();

			// Check if we got here because of waveOutReset
			if ( !m_playing )
			{
				// Cleanup - free buffers and headers
				Monitor.Enter(m_qBuffers.SyncRoot);
				while( m_qBuffers.Count > 0 )
				{
					hdr = m_qBuffers.Dequeue() as WaveHeader;
					CheckWaveError(Core.waveOutUnprepareHeader(m_hWaveOut, hdr.Header, hdr.HeaderLength));
					m_HandleMap.Remove(hdr.Header.ToInt32());
					hdr.Dispose();
				}
				Monitor.Exit(m_qBuffers.SyncRoot);
			}

			// Are there any pending buffers?
			if ( m_qBuffers.Count == 0 )
			{
				// No more buffers
				// Close the device
				CheckWaveError(Core.waveOutClose(m_hWaveOut));
				m_playing = false;
				m_qBuffers = null;

				// Notify clients
				if(DonePlaying != null)
				{
					DonePlaying(this, wParam, lParam);
				}
			}
			else
			{
				//Get next buffer (already prepared)
				Monitor.Enter(m_qBuffers.SyncRoot);
				hdr = (WaveHeader)m_qBuffers.Dequeue();
				Monitor.Exit(m_qBuffers.SyncRoot);

				// play the file
				CheckWaveError(Core.waveOutWrite(m_hWaveOut, hdr.Header, hdr.HeaderLength));
				
				// Add more buffers
				RefillPlayBuffers();
			}
			
		}

	}
}
