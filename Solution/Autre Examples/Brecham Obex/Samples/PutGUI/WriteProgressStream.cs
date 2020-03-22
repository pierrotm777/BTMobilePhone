using System;
using System.IO;
using System.Runtime.InteropServices;   //e.g. ComVisible


namespace PutGuiCs
{

    public class ReadProgressStream : Stream
    {
        protected Stream m_strm;
        long m_readLength = 0;
        long m_totalReadLength = -1;


        public long ReadLength { get { return m_readLength; } }
        public void SetTotalReadLength(long totalLength)
        {
            m_totalReadLength = totalLength;
        }
        public float ReadPercentage
        {
            get
            {
                if (m_totalReadLength < 0)
                    throw new InvalidOperationException("To read the percentage complete the total length must have been set beforehand.");
                // For zero length objects...
                if (m_totalReadLength == 0) {
                    return 100.0f;
                }
                return 100.0f * m_readLength / m_totalReadLength;
            }
        }


        public ReadProgressStream(Stream strm)
        {
            m_strm = strm;
        }

        public override bool CanRead { get { return m_strm.CanRead; } }
        public override bool CanSeek { get { return m_strm.CanSeek; } }
#if ! FX1_1
        [ComVisible(false)]
        public override bool CanTimeout { get { return m_strm.CanTimeout; } }
#endif
        public override bool CanWrite { get { return m_strm.CanWrite; } }
        public override long Length { get { return m_strm.Length; } }
        public override long Position
        {
            get { return m_strm.Position; }
            set { m_strm.Position = value; }
        }
#if ! FX1_1
        [ComVisible(false)]
        public override int ReadTimeout
        {
            get { return m_strm.ReadTimeout; }
            set { m_strm.ReadTimeout = value; }
        }
#endif
#if ! FX1_1
        [ComVisible(false)]
        public override int WriteTimeout
        {
            get { return m_strm.WriteTimeout; }
            set { m_strm.WriteTimeout = value; }
        }
#endif

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, 
            AsyncCallback callback, object state)
        {
            IAsyncResult ar = m_strm.BeginRead(buffer, offset, count, callback, state);
            m_readLength += count;
            return ar;
        }
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, 
            AsyncCallback callback, object state)
        {
            return m_strm.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void Close() { m_strm.Close(); }
        public override int EndRead(IAsyncResult asyncResult)
        {
            return m_strm.EndRead(asyncResult);
        }
        public override void EndWrite(IAsyncResult asyncResult)
        {
            m_strm.EndRead(asyncResult);
        }
        public override void Flush() { m_strm.Flush(); }
        public override int Read(byte[] buffer, int offset, int count)
        {
            int length = m_strm.Read(buffer, offset, count);
            m_readLength += length;
            return length;
        }
        public override int ReadByte()
        {
            int value = m_strm.ReadByte();
            m_readLength += 1;
            return value;
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return m_strm.Seek(offset, origin);
        }
        public override void SetLength(long value)
        {
            m_strm.SetLength(value);
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            m_strm.Write(buffer, offset, count);
        }
        public override void WriteByte(byte value)
        {
            m_strm.WriteByte(value);
        }
    }//class--WriteProgressStream

}
