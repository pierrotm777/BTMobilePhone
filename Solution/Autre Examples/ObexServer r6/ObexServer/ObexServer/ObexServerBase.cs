using System;
//
using System.IO;
//
using Brecham.Obex.Pdus;
using Brecham.Obex;


namespace ObexServer
{

    /// <summary>
    /// The base class of an OBEX server.  Handles the PDUs received by the server 
    /// host, and for each returns a response PDU or an error.
    /// </summary>
    public abstract class ObexServerBase : IObexServer
    {
        //----------------------------------------------------------
        ObexServerHost m_host;

        //--------------------------------------------------------------

        public ObexServerHost Host
        {
            set
            {
                if (m_host != null) {
                    throw new InvalidOperationException("IObexServerHost already set.");
                }
                m_host = value;
            }
            [System.Diagnostics.DebuggerStepThrough]
            get { return m_host; }
        }

        protected ObexPduFactory PduFactory {
            [System.Diagnostics.DebuggerStepThrough]
            get { return Host.PduFactory; }
        }

        //--------------------------------------------------------------
        public abstract ObexCreatedPdu HandlePdu(ObexParsedRequestPdu requestPdu);

    }
}