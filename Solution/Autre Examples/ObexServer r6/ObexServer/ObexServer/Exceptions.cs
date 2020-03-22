
using System;
#if ! NETCF
using System.Runtime.Serialization;
#endif
using Brecham.Obex.Pdus;
using System.Security.Permissions;
using System.Diagnostics.CodeAnalysis;
namespace ObexServer
{

    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    [Serializable]
    public class ObexClientFatalException : Exception
    {
        public ObexClientFatalException()
            : base()
        { }

        public ObexClientFatalException(String message)
            : base(message)
        { }

#if ! NETCF
        /// <summary>
        ///     Initializes a new instance of the ObexClientFatalException class with serialized
        ///     data.
        /// </summary>
        /// <param name="info">
        ///     The System.Runtime.Serialization.SerializationInfo that holds the serialized
        ///     object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The System.Runtime.Serialization.StreamingContext that contains contextual
        ///     information about the source or destination.
        /// </param>
        /// -
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        ///     The class name is null or System.Exception.HResult is zero (0).
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     The info parameter is null.
        /// </exception>
        protected ObexClientFatalException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }//class


    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    [Serializable]
    public class ObexClientOperationException : Exception
    {
        ObexCreatedPdu m_pdu;

        //--------------------------------------------------------------
        public ObexClientOperationException(ObexCreatedPdu responsePdu)
        {
            m_pdu = responsePdu;
        }

        //--------------------------------------------------------------

        ///// <summary>
        /////     Initializes a new instance of the System.Exception class with a specified
        /////     error message and a reference to the inner exception that is the cause of
        /////     this exception.
        ///// </summary>
        ///// <param name="message">
        /////     The error message that explains the reason for the exception.
        ///// </param>
        ///// <param name="innerException">
        /////     The exception that is the cause of the current exception, or a null reference
        /////     (Nothing in Visual Basic) if no inner exception is specified.
        ///// </param>
        //public ObexClientOperationException(string message, Exception innerException)
        //    : base(message, innerException)
        //{
		//}

#if ! NETCF
        /// <summary>
        ///     Initializes a new instance of the ObexClientOperationException class with serialized
        ///     data.
        /// </summary>
        /// <param name="info">
        ///     The System.Runtime.Serialization.SerializationInfo that holds the serialized
        ///     object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The System.Runtime.Serialization.StreamingContext that contains contextual
        ///     information about the source or destination.
        /// </param>
        /// -
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        ///     The class name is null or System.Exception.HResult is zero (0).
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     The info parameter is null.
        /// </exception>
        protected ObexClientOperationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // TODO ((this.m_pdu = (ObexCreatedPdu)info.GetValue("m_pdu", typeof(ObexCreatedPdu));))
        }
#endif

#if ! NETCF
		/// <summary>
        /// Populates a <c>SerializationInfo</c> with the data needed to serialize 
        /// the target object. 
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to 
        /// populate with data.
        /// </param>
        /// <param name="context">
        /// A <see cref="T:T:System.Runtime.Serialization.StreamingContext"/> that 
        /// specifies the destination for this serialization.
        /// </param>
        // info will be validated by base.GetObjectData(info, context);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            //TODO ((info.AddValue("m_pdu", (ObexCreatedPdu)this.m_pdu, typeof(ObexCreatedPdu));))
        }
#endif

        //--------------------------------------------------------------
        public ObexCreatedPdu ResponsePdu { get { return m_pdu; } }

        //--------------------------------------------------------------
    }//class

}
