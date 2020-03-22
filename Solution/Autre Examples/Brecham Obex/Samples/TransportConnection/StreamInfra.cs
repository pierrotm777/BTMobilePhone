using System;
using System.Text;
using System.IO;

namespace Brecham.Obex.Net
{

    /// <exclude/>
    /// <summary>
    /// An internal abstract <see cref="T:System.IO.Stream"/> used to implement some of the 
    /// Close/Dispose functionality from FXv2 when compiling the library for FXv1.1.
    /// </summary>
    public abstract class ForV1CloseDisposeLikeV2Stream : Stream
    {
#if FX1_1
        /* The sequence of related calls supplied by Stream in V2 is a follows.
         * There is no such sequence in V1 so we supply it in the 
         * ForV1CloseDisposeLikeV2Stream class.
         * 
         * public override void Close()
         *   In base (Stream) calls Dispose(true);
         * public void Dispose()
         *   In base (Stream) calls Close();
         * protected override void Dispose(bool disposing)
         *   In base (AbortableStream) calls CloseCore(true);
         *   and then base.Dispose(disposing);
         */

        public /*virtual*/override void Close()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            this.Close();
        }

        protected virtual void Dispose(bool disposing)
        { }
#endif
    }//class

}