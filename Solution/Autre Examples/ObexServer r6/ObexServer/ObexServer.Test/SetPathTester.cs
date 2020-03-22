using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ObexServer.Tests
{
    class SetPathTester : ServerTester
    {
        //----------------------------------------------------------
        public SetPathTester()
            // Could instead just of had this class inherit from GetTester...
            : this(new GetTester.DefaultObexServerFactory())
        { }

        public SetPathTester(ObexServerFactory factory)
            :base(factory)
        { }

        //----------------------------------------------------------------------
        protected override IObexServer GetServer(System.IO.Stream peer)
        {
            IObexServer svr = base.GetServer(peer);
            // Adding the FolderChange event handler.
            ObexGetServer gsvr = (ObexGetServer)svr;
            gsvr.FolderChange += new EventHandler<FolderChangeEventArgs>(HandleFolderChange);
            return svr;
        }

        public List<FolderChangeEventArgs> m_setPathEventArgsList = new List<FolderChangeEventArgs>();
        public Brecham.Obex.Pdus.ObexCreatedPdu m_setPathResponsePdu;

        void HandleFolderChange(object sender, FolderChangeEventArgs e)
        {
            m_setPathEventArgsList.Add(e);
            if (m_setPathResponsePdu != null)
                e.ErrorResponsePdu = m_setPathResponsePdu;
        }

        //----------------------------------------------------------------------
        protected override void InitServer()
        {
        }

        protected override void BeforeAssert()
        {
        }

        protected override void AfterAssert()
        {
        }

        //----------------------------------------------------------
    }
}
