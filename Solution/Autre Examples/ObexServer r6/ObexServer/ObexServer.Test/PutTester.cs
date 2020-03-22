using System;
using System.Collections.Generic;
using System.Text;
//
using System.IO;
//
using NUnit.Framework;
using Brecham.TestsInfrastructure;

namespace ObexServer.Tests
{

    public class PutTester : ServerTester
    {
        //----------------------------------------------------------
        public PutTester()
            : this(new DefaultObexServerFactory())
        { }

        public PutTester(ObexServerFactory factory)
            :base(factory)
        { }

        //----------------------------------------------------------
        protected override void InitServer()
        { }

        protected override void BeforeAssert()
        { }

        protected override void AfterAssert()
        {
            // Received PUT content
            RememberPutMetadataAndContentPutStreamObexServer svr
                = (RememberPutMetadataAndContentPutStreamObexServer)m_svr;
            Assert.AreEqual(m_expectedObjects.Count, svr.ContentCount, "Number of PUT operations");
            for (int i = 0; i < m_expectedObjects.Count; ++i) {
                // Metadata
                String name;
                if (svr.ContentCount >= 1 && svr.GetContentEntry(i).Metadata != null
                    && svr.GetContentEntry(i).Metadata.Contains(Brecham.Obex.ObexHeaderId.Name)) {
                    name = svr.GetContentEntry(i).Metadata.GetString(Brecham.Obex.ObexHeaderId.Name);
                } else {
                    name = null;
                }
                Assert.AreEqual(m_expectedObjects[i].ExpectedName, name, "PUT Name, of #" + i.ToString());

                // Content
                byte[] actualContent = svr.GetContentEntry(i).Content;
                Assert.AreEqual(m_expectedObjects[i].ExpectedContent, actualContent, "PUT Content");
            }//for
        }//fn

        //----------------------------------------------------------
        private class DefaultObexServerFactory : ObexServerFactory
        {
            public override IObexServer GetNewObexServer(Stream peer)
            {
                return new RememberPutMetadataAndContentPutStreamObexServer(peer, 2048);
            }
        }
        //----------------------------------------------------------
        public class RememberPutMetadataAndContentPutStreamObexServer : ObexInboxServer
        {
            public struct PutContent
            {
                private Brecham.Obex.ObexHeaderCollection m_metadata;
                private MemoryStream m_content;

                //------------------------------------------------------
                public byte[] Content
                {
                    get
                    {
                        if (ContentStream == null) { return null; }
                        return ContentStream.ToArray();
                    }
                }

                public MemoryStream ContentStream
                {
                    get { return m_content; }
                    internal set { m_content = value; }
                }

                public Brecham.Obex.ObexHeaderCollection Metadata
                {
                    get { return m_metadata; }
                    internal set { m_metadata = value; }
                }
            }

            private List<PutContent> m_objects = new List<PutContent>();

            //----------------------------------------------------------
            public RememberPutMetadataAndContentPutStreamObexServer(Stream peer, int bufferSize)
            {
                SetFolder(new DirectoryInfo("."));
                SetServerOnHost();
            }

            protected virtual void SetServerOnHost()
            {
                this.CreatePutStream += this.RpmacCreateStream;
            }

            //----------------------------------------------------------
            public int ContentCount { get { return m_objects.Count; } }
            public PutContent GetContentEntry(int index)
            {
                return m_objects[index];
            }

            //----------------------------------------------------------
            protected void RpmacCreateStream(object sender, CreatePutStreamEventArgs e)
            {
                IObexServer svr = (IObexServer)sender;
                ObexServerHost host = svr.Host;
                //
                PutContent item = new PutContent();
                item.Metadata = e.PutMetadata;
                if (!item.Metadata.Contains(Brecham.Obex.ObexHeaderId.Name)) {
                    e.ErrorResponsePdu = host.CreateResponseWithDescription(Brecham.Obex.Pdus.ObexResponseCode.BadRequest,
                        "No Name give in PUT operation.");
                    item.ContentStream = null;
                } else {
                    item.ContentStream = new MemoryStream();
                }
                m_objects.Add(item);
                e.PutStream = item.ContentStream;
            }

        }

        //----------------------------------------------------------
    }//class

}
