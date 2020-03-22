using System;
using System.Text;
using System.IO;

namespace Brecham.Obex.Net
{

#if FX1_1
    /*
#endif
#pragma warning disable 1591
#if FX1_1
    */
#endif


    /// <exclude/>
    public class TraceStream : ForV1CloseDisposeLikeV2Stream
    {
        TextWriter m_log;
        Stream m_child;
        object m_lock = new object();
        //
        StringBuilder m_lines;
        StringBuilder m_hex;
        StringBuilder m_text;


        //--------------------------------------------------------------
        public TraceStream(Stream stream, String logFilename)
            :this(stream, new StreamWriter(logFilename))
        { }

        public TraceStream(Stream stream, TextWriter log)
        {
            m_log = log;
            m_child = stream;
            InitHexWriter();
        }

        //--------------------------------------------------------------
        protected override void Dispose(bool disposing)
        {
            try {
                if (disposing) {
                    try {
                        m_child.Close();
                    } finally {
                        m_log.Close();
                    }
                }//if
            } finally {
                base.Dispose(disposing);
            }
        }

        //--------------------------------------------------------------
        public override bool CanRead
        {
            get { return m_child.CanRead; }
        }

        public override bool CanSeek
        {
            get { throw new NotSupportedException(); }
        }

        public override bool CanWrite
        {
            get { return m_child.CanWrite; }
        }

        public override void Flush()
        {
            m_child.Flush();
        }

        public override long Length
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override long Position
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (m_lock) {
                m_log.WriteLine("Read()");
            }
            int readLen = m_child.Read(buffer, offset, count);
            lock (m_lock) {
                m_log.WriteLine("Data from Read");
                WriteData(buffer, offset, readLen);
                m_log.WriteLine("Exiting Read()");
            }
            return readLen;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetLength(long value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            lock (m_lock) {
                m_log.WriteLine("Write()");
                m_log.WriteLine("Data from Write");
                WriteData(buffer, offset, count);
            }
            m_child.Write(buffer, offset, count);
            lock (m_lock) {
                m_log.WriteLine("Exiting Write()");
            }
        }

        private void WriteData(byte[] buffer, int offset, int count)
        {
            WriteHexData(buffer, offset, count);
            WriteHexTextBuffers();
        }

        //--------------------------------------------------------------
        void InitHexWriter()
        {
            const int LenHex = 3 + 16 + 1;
            const int LenText = 16;
            const int LineLength = LenHex + 2 + LenText;
            m_lines = new StringBuilder(10 * LineLength);
            m_hex = new StringBuilder(LenHex);
            m_text = new StringBuilder(LenText);
        }

        public void WriteHexData(byte[] buffer, int offset, int count)
        {
            if (buffer == null) {
                throw new ArgumentNullException("buffer");
            }
            //
            int numLines = buffer.Length / 16 + 1;
            for (int i = 0; i < count; ++i) {
                byte curByte = buffer[i + offset];
                m_hex.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                    "{0:X2} ", curByte);
                //
                char curChar = (Char)curByte;
                if (Char.IsControl(curChar) || curChar >= 0x80) {
                    m_text.Append(".");
                } else {
                    m_text.Append(curChar);
                }
                //
                const int LengthOfExtrasSpaceInText = 1;
                if (m_text.Length == 0 || m_text.Length == 1) {
                } else if ((m_text.Length - LengthOfExtrasSpaceInText) % 16 == 0) {
                    WriteHexTextBuffers();
                } else if (m_text.Length % 8 == 0 && m_text.Length % 16 != 0) {
                    m_hex.Append(" ");
                    m_text.Append(" ");
                }
            }//for
        }

        private static void appendHexTextLine(StringBuilder lines, StringBuilder hex, StringBuilder text)
        {
            if (hex.Length == 0) {
                System.Diagnostics.Debug.Assert(text.Length == 0, "text.Length==0 <-- hex.Length==0");
            } else {
                lines.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                    "{0,-49} {1}", hex, text);
                lines.Append("\r\n");
                hex.Length = 0;
                text.Length = 0;
            }
        }

        private void WriteHexTextBuffers()
        {
            appendHexTextLine(m_lines, m_hex, m_text);
            m_log.Write(m_lines.ToString());
            m_lines.Length = 0;
        }

    }//class

}
