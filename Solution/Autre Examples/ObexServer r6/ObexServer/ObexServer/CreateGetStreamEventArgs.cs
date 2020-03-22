using System;
//
using System.IO;
//
using Brecham.Obex.Pdus;
using Brecham.Obex;


namespace ObexServer
{


    /// <summary>
    /// Provides data for the GET stream create event.
    /// </summary>
    /// -
    /// <remarks>
    /// <para>A <see cref="P:ObexServer.ObexInboxServer.CreateGetStream"/> event
    /// is raised when a request to GET an object is received.</para>
    /// <para>The <see cref="P:ObexServer.CreateGetStreamEventArgs.GetMetadata"/>
    /// property contains the OBEX Headers received from the client.  If the request
    /// is acceptable then a <see cref="T:System.IO.Stream"/> should be opened and
    /// the instance passed to the
    /// <see cref="P:ObexServer.CreateGetStreamEventArgs.GetStream"/> property.
    /// Otherwise if the request is not valid then the 
    /// <see cref="P:ObexServer.CreateGetStreamEventArgs.ErrorResponsePdu"/>
    /// property should be set.
    /// </para>
    /// </remarks>
    public class CreateGetStreamEventArgs : EventArgs
    {
        // Fields
        ObexHeaderCollection m_getMetadata;
        Stream m_getStream;
        ObexCreatedPdu m_rspPdu;

        // Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CancelEventArgs"/>
        /// class with the <see cref="P:GetMetadata"/> property set to the given value.
        /// </summary>
        /// <param name="metadata"></param>
        public CreateGetStreamEventArgs(ObexHeaderCollection metadata, DirectoryInfo currentFolder)
        {
            m_getMetadata = metadata;
            m_currentFolder = currentFolder;
        }

        // Properties

        /// <summary>
        /// Gets the OBEX metadata received from the client in this GET operation.
        /// </summary>
        /// <remarks>
        /// Will not contain any <see cref="F:Brecham.Obex.ObexHeaderId.Body"/> or
        /// <see cref="F:Brecham.Obex.ObexHeaderId.EndOfBody"/> headers received
        /// from the client.
        /// </remarks>
        public ObexHeaderCollection GetMetadata {
            [System.Diagnostics.DebuggerStepThrough]
            get { return m_getMetadata; }
        }

        /// <summary>
        /// Gets or sets a value indicating the <see cref="T:System.IO.Stream"/> from
        /// which the GET content is to be read.
        /// </summary>
        /// <remarks>
        /// If it is not known from where the content should be read, for instance if the 
        /// received metadata does not include the required content e.g. the Name header,
        /// then <c>null</c> should be set here.
        /// </remarks>
        public Stream GetStream
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return m_getStream; }
            set { m_getStream = value; }
        }

        /// <summary>
        /// Sets the error response to return to the client, when the request was
        /// not acceptable and a stream was not opened.
        /// </summary>
        /// <remarks>
        /// In the case where the content was not acceptable and <see langword="null"/>
        /// was set for the (<see cref="P:ObexServer.CreateGetStreamEventArgs.GetStream"/>
        /// property then a PDU should be returned here to be sent in response.
        /// </remarks>
        public ObexCreatedPdu ErrorResponsePdu
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return m_rspPdu; }
            set { m_rspPdu = value; }
        }

        private DirectoryInfo m_currentFolder;
        public DirectoryInfo CurrentFolder
        {
            get { return m_currentFolder; }
        }

    }//class

}
