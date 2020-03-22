using System;
using System.Collections.Generic;
using System.Text;
using Brecham.Obex; //eg ObexHeaderCollection
using Brecham.Obex.Pdus; //eg ObexCreatedPdu
using System.IO;

namespace ObexServer
{

    public class FolderChangeEventArgs : EventArgs
    {
        private FolderChangeType m_FolderChangeType;
        private String m_childFolderName;
        private bool m_mayCreateIfNotExist;
        //
        DirectoryInfo m_current, m_newFolder;
        //
        private ObexCreatedPdu m_errorRspPdu;

        //--------
        /// <summary>
        /// Initialize a new instance of <see cref="T:ObexServer.FolderChangeEventArgs"/>.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>Although this constructor initialises only one of the properties
        /// the other properties must also be set.  <see cref="P:ObexServer.FolderChangeEventArgs.FolderChangeType"/>
        /// must always be set, and in cases <see cref="F:ObexServer.FolderChangeType.Up"/>
        /// and <see cref="F:ObexServer.FolderChangeType.UpAndDown"/> properties
        /// <see cref="P:"/>
        /// .
        /// </para>
        /// </remarks>
        /// -
        /// <param name="currentFolder">The current folder in which the server is 
        /// active, that is where PUT/GET files will loaded/written to.
        /// </param>
        public FolderChangeEventArgs(DirectoryInfo currentFolder)
        {
            if (currentFolder == null)
                throw new ArgumentNullException("currentFolder");
            m_current = currentFolder;
        }

        //--------

        public FolderChangeType FolderChangeType
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return m_FolderChangeType; }
            set { m_FolderChangeType = value; }
        }

        public bool MayCreateIfNotExist
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return m_mayCreateIfNotExist; }
            set { m_mayCreateIfNotExist = value; }
        }

        public String ChildFolderName
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return m_childFolderName; }
            set { m_childFolderName = value; }
        }

        //----

        public DirectoryInfo CurrentFolder { get { return m_current; } }

        public DirectoryInfo NewFolder
        {
            get { return m_newFolder; }
            set { m_newFolder = value; }
        }

        //----

        /// <summary>
        /// Sets the error response to return to the client, when the request was
        /// not acceptable.
        /// </summary>
        /// <remarks>
        /// In the case where the SetPath operation was no acceptable 
        /// then a PDU should be returned here to be sent in response.
        /// </remarks>
        public ObexCreatedPdu ErrorResponsePdu
        {
            get { return m_errorRspPdu; }
            set { m_errorRspPdu = value; }
        }

    }//class

    public enum FolderChangeType
    {
        /// <summary>
        /// Reset to the original folder.
        /// </summary>
        Reset,

        /// <summary>
        /// Move to the parent folder.
        /// </summary>
        Up,

        /// <summary>
        /// Move to a child folder.
        /// </summary>
        Down,

        /// <summary>
        /// Move to a sibling folder, i.e. move to the parent folder followed by 
        /// move to a child folder.
        /// </summary>
        UpAndDown
    }//enum

}