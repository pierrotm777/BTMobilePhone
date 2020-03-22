using System;
using System.IO;

namespace CmdlineRunner
{

    class NullStream : Stream
    {
        public NullStream()
        { }

        public override bool CanRead { get { throw new NotImplementedException(); } }

        public override bool CanSeek { get { throw new NotImplementedException(); } }

        public override bool CanWrite { get { return true; } }

        public override void Flush() { throw new NotImplementedException(); }

        public override long Length { get { throw new NotImplementedException(); } }

        public override long Position
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override int Read(byte[] buffer, int offset, int count) { throw new NotImplementedException(); }

        public override long Seek(long offset, SeekOrigin origin) { throw new NotImplementedException(); }

        public override void SetLength(long value) { throw new NotImplementedException(); }

        public override void Write(byte[] buffer, int offset, int count)
        {
            /*NOOP*/
            //Console.Write('n');
        }
    }//class

}