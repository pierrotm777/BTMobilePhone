using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Brecham.TestsInfrastructure;
using NUnit.Framework;

namespace ObexServer.Tests
{

    public class GetTester : ServerTester
    {
        Stream m_content;

        //----------------------------------------------------------
        public GetTester()
            : this(new DefaultObexServerFactory())
        { }

        public GetTester(ObexServerFactory factory)
            :base(factory)
        { }

        //----------------------------------------------------------------------
        public void SetGetContent(Stream content)
        {
            if (content == null)
                throw new ArgumentNullException("content");
            m_content = content;
        }

        public void SetGetContent(byte[] content)
        {
            if (content == null)
                throw new ArgumentNullException("content");
            SetGetContent(new MemoryStream(content, false));
        }

        //----------------------------------------------------------------------
        protected override IObexServer GetServer(Stream peer)
        {
            IObexServer svr = m_factory.GetNewObexServer(peer);
            return svr;
        }

        protected override ObexServerHost GetServerHost(Stream peer, int bufferSize)
        {
            ObexServerHost host;
            //HACK host = new AsyncObexServerHost(peer, bufferSize, pduHandler);//HACK choosing sync/async
            host = new SyncObexServerHost(peer, bufferSize);//HACK choosing sync/async
            return host;
        }

        //----------------------------------------------------------
        protected override void InitServer()
        {
            RememberGetMetadataProvideContentStreamObexServer gpcSvr
                = m_svr as RememberGetMetadataProvideContentStreamObexServer;
            if (gpcSvr != null) {
                gpcSvr.SetContent(m_content);
            }
        }

        protected override void BeforeAssert()
        { }

        protected override void AfterAssert()
        {
            return;
        }

        //----------------------------------------------------------
        internal class DefaultObexServerFactory : ObexServerFactory
        {
            public override IObexServer GetNewObexServer(Stream peer)
            {
                RememberGetMetadataProvideContentStreamObexServer svr
                    = new RememberGetMetadataProvideContentStreamObexServer(peer, 2048);
                return svr;
            }
        }
        //----------------------------------------------------------
        public class RememberGetMetadataProvideContentStreamObexServer : ObexGetServer
        {
            public struct GetContent
            {
                private Brecham.Obex.ObexHeaderCollection m_metadata;

                //------------------------------------------------------
                public Brecham.Obex.ObexHeaderCollection Metadata
                {
                    get { return m_metadata; }
                    internal set { m_metadata = value; }
                }
            }

            private List<GetContent> m_objects = new List<GetContent>();
            private Stream m_contentSource;

            //----------------------------------------------------------
            public RememberGetMetadataProvideContentStreamObexServer(Stream peer, int bufferSize)
            {
                SetServerOnHost();
            }

            internal void SetContent(Stream content)
            {
                m_contentSource = content;
            }

            protected virtual void SetServerOnHost()
            {
                this.CreateGetStream += this.RgmCreateStream;
            }

            //----------------------------------------------------------
            public int ContentCount { get { return m_objects.Count; } }
            public GetContent GetContentEntry(int index)
            {
                return m_objects[index];
            }

            //----------------------------------------------------------
            protected void RgmCreateStream(object sender, CreateGetStreamEventArgs e)
            {
                IObexServer svr = (IObexServer)sender;
                ObexServerHost host = svr.Host;
                //
                GetContent item = new GetContent();
                item.Metadata = e.GetMetadata;
                m_objects.Add(item);
                //
                if (m_contentSource == null) {
                    throw new InvalidOperationException("m_contentSource is unset.");
                }
                e.GetStream = m_contentSource;
            }

        }//class2

        //----------------------------------------------------------
    }//class

}
