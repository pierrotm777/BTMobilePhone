using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using Brecham.Obex.Pdus;
using Brecham.Obex;
using System.Net.Sockets;
using System.Net;



namespace ObexServer
{

    /// <summary>
    /// Handles the OBEX protocol, it handles the PDUs read by <see cref="T:ObexServer.IObexServerHost"/>
    /// and for each returns a PDU to it for it to send, or an error.
    /// </summary>
    public interface IObexServer
    {

        /// <summary>
        /// When overriden in a descendant class, accepts a PDU just received, handles
        /// it as it sees fit, and returns a PDU to be sent in response.
        /// </summary>
        /// -
        /// <remarks>
        /// Note, if the bytes received as the PDU fail to parse, then the connection
        /// is just closed, and the <see cref="P:ObexServer.ObexServer.ExitReason"/>
        /// property will return <see cref="F:ObexServer.ObexServer.ExitReason.InvalidClientPdu"/>.
        /// </remarks>
        /// -
        /// <param name="requestPdu"></param>
        /// The PDU received from the peer client, as an <see 
        /// cref="T:Brecham.Obex.Pdus.ObexParsedRequestPdu"/>
        /// -
        /// <returns>
        /// A <see cref="T:Brecham.Obex.Pdus.ObexCreatedPdu"/> instance containing
        /// the response PDU to be sent back to the peer.
        /// </returns>
        ObexCreatedPdu HandlePdu(ObexParsedRequestPdu requestPdu);

        /// <summary>
        /// Get or set the container <see cref="T:ObexServer.ObexServerHost"/>.
        /// It is set by 
        /// <see cref="M:ObexServer.ObexServerHost.AddHandler(ObexServer.IObexServer)"/>.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>When a new <see cref="T:ObexServer.IObexServer"/> instance is set 
        /// on a <see cref="T:ObexServer.ObexServerHost"/> using its 
        /// <see cref="M:ObexServer.ObexServerHost.AddHandler(ObexServer.IObexServer)"/>"/>
        /// method it sets itself as the containing host using this property.  This 
        /// is similar to the behaviour of the FCL&#x2019;s 
        /// <see cref="T:System.ComponentModel.IComponent"/> and 
        /// <see cref="T:System.ComponentModel.Container"/> pair of classes.
        /// </para>
        /// <para>The server may want to call into its host to use the 
        /// <see cref="M:ObexServer.ObexServerHost.CheckRequestPduContains(Brecham.Obex.ObexHeaderId,Brecham.Obex.Pdus.ObexParsedRequestPdu)"/>
        /// method or similar.
        /// </para>
        /// </remarks>
        ObexServerHost Host { get; set; }

    }


    //--------------------------------------------------------------------------

    //TODO Delete: public interface IObexServerHostFactory
    //{
    //    IObexServerHost CreateObexServerHost(Stream peer, int bufferSize);
    //}

}
