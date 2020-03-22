using System;
//
using System.IO;
//
using Brecham.Obex.Pdus;
using Brecham.Obex;


namespace ObexServer
{


    /// <summary>
    /// Provides data for the PUT stream create event.
    /// </summary>
    /// -
    /// <remarks>
    /// <para>A <see cref="P:ObexServer.ObexInboxServer.CreatePutStream"/> event
    /// is raised when a request to PUT an object is received.</para>
    /// <para>The <see cref="P:ObexServer.CreatePutStreamEventArgs.PutMetadata"/>
    /// property contains the OBEX Headers received from the client.  If the request
    /// is acceptable then a <see cref="T:System.IO.Stream"/> should be opened and
    /// the instance passed to the
    /// <see cref="P:ObexServer.CreatePutStreamEventArgs.PutStream"/> property.
    /// Otherwise if the request is not valid then the 
    /// <see cref="P:ObexServer.CreatePutStreamEventArgs.ErrorResponsePdu"/>
    /// property should be set.
    /// </para>
    /// </remarks>
    public class CreatePutStreamEventArgs : EventArgs
    {
        // Fields
        ObexHeaderCollection m_putMetadata;
        Stream m_putStream;
        ObexCreatedPdu m_rspPdu;
        DirectoryInfo m_currentFolder;

        // Constructors ----

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CancelEventArgs"/>
        /// class with the <see cref="P:PutMetadata"/> property set to the given value.
        /// </summary>
        /// <param name="metadata"></param>
        public CreatePutStreamEventArgs(ObexHeaderCollection metadata, DirectoryInfo currentFolder)
        {
            if (currentFolder == null)
                throw new ArgumentNullException("currentFolder");
            m_putMetadata = metadata;
            m_currentFolder = currentFolder;
        }

        // Properties ----

        /// <summary>
        /// Gets the OBEX metadata received from the client in this PUT operation.
        /// </summary>
        /// <remarks>
        /// Will not contain any <see cref="F:Brecham.Obex.ObexHeaderId.Body"/> or
        /// <see cref="F:Brecham.Obex.ObexHeaderId.EndOfBody"/> headers received
        /// from the client.
        /// </remarks>
        public ObexHeaderCollection PutMetadata { get { return m_putMetadata; } }

        /// <summary>
        /// Gets or sets a value indicating the <see cref="T:System.IO.Stream"/> to
        /// which the received PUT is to be written.
        /// </summary>
        /// <remarks>
        /// If it is not known where the content is to be stored, for instance if the 
        /// received metadata does not include the required content e.g. the Name header,
        /// then <c>null</c> should be set here.
        /// </remarks>
        public Stream PutStream
        {
            get { return m_putStream; }
            set { m_putStream = value; }
        }

        /// <summary>
        /// Sets the error response to return to the client, when the request was
        /// not acceptable and a stream was not opened.
        /// </summary>
        /// <remarks>
        /// In the case where the content was not acceptable and <see langword="null"/>
        /// was set for the (<see cref="P:ObexServer.CreatePutStreamEventArgs.PutStream"/>
        /// property then a PDU should be returned here to be sent in response.
        /// </remarks>
        public ObexCreatedPdu ErrorResponsePdu
        {
            get { return m_rspPdu; }
            set { m_rspPdu = value; }
        }

        public DirectoryInfo CurrentFolder
        {
            get { return m_currentFolder; }
        }

    }//class

}
