namespace ObexServer
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags", Justification = "Is not a flags enum.")]
    public enum ServerExitReason
    {
        None = 0,
        //--
        ConnectionGracefulClose = 1,
        ConnectionGracefulCloseWithinAPdu,
        /// <summary>
        /// Normal close appears as reset from some peer devices over some protocols,
        /// so allow for that behaviour.  One such device is PalmOS over IrDA.
        /// </summary>
        ConnectionHardCloseOutwithAPdu,
        ConnectionErrorOnRead,
        ConnectionErrorOnWrite,
        InvalidClientPdu,

        /// <summary>
        /// For example Sending Connect twice
        /// </summary>
        InvalidClientBehaviour,

        /// <summary>
        /// Client sent a Disconnect PDU, we will respond, and then close the connection.
        /// </summary>
        DisconnectPdu,
        //--
        /// <summary>
        /// An unhandled error in the local code.
        /// </summary>
        InternalServerError = 0x40,

        /// <summary>
        /// Indicates an unhandled error occurred in the code provided by a descendent 
        /// concrete class.  For instance in the code it provides to 
        /// </summary>
        InternalServerErrorInHandler,
        //--
        Disposed = 0x80,
        Finalized,
    }

}
