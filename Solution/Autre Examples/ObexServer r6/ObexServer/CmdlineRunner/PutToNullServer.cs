using System;
using System.IO;
using Brecham.Obex.Pdus;
using Brecham.Obex;
using ObexServer;


namespace CmdlineRunner
{
    class PutToNullServer : ObexServer.ObexInboxServer
    {
        //--------------------------------------------------------------
        public PutToNullServer()
        { }

        //--------------------------------------------------------------
        protected override void OnCreatePutStream(CreatePutStreamEventArgs ea)
        {
            System.Diagnostics.Debug.Fail("Untested...");

            if (ea.PutMetadata.Contains(ObexHeaderId.Name)) {
                String name = ea.PutMetadata.GetString(ObexHeaderId.Name);
                Stream dest = File.OpenWrite(name);
                Console.WriteLine("Opened content stream for: " + name);
                ea.PutStream = dest;
            } else {
                // Not opened stream, as didn't known Name, so complain.
                ea.ErrorResponsePdu = Host.CreateResponseWithDescription("No PUT Name given.");
            }
        }
    }//class

}
