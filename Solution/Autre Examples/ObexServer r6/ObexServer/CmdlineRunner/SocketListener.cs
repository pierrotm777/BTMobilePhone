using System;
using System.Net;
using System.Net.Sockets;
using InTheHand.Net.Sockets;

namespace CmdlineRunner
{
    /// <summary>
    /// Stores a reference to a listening <see cref="T:System.Net.Sockets.Socket"/>
    /// or a <see cref="T:InTheHand.Net.Sockets.BluetoothListener"/> etc
    /// to provide access to its <c>Accept</c> method etc
    /// </summary>
    /// -
    /// <remarks>See <see cref="T:CmdlineRunner.SocketClient"/> for all information.
    /// </remarks>
    /// -
    /// <seealso cref="T:CmdlineRunner.SocketClient"/>
    internal class SocketListener
    {
        #region Fields
        // We have to store the BluetoothListener and not a Socket and on Widcomm(/etc?)
        // there's no Socket to store, BluetoothListener.Server returns NotSupportedException.
        readonly BluetoothListener _lsnrBt;
        readonly Socket _lsnrSock;
        #endregion

        #region Constructors
        public SocketListener(BluetoothListener lsnr)
        {
            if (lsnr == null)
                throw new ArgumentNullException("lsnr");
            _lsnrBt = lsnr;
        }

        public SocketListener(IrDAListener lsnr)
        {
            if (lsnr == null)
                throw new ArgumentNullException("lsnr");
            // No reason to not store just its internal socket for now.
            _lsnrSock = lsnr.Server;
        }

        public SocketListener(TcpListener lsnr)
        {
            if (lsnr == null)
                throw new ArgumentNullException("lsnr");
            // No reason to not store just its internal socket for now.
            _lsnrSock = lsnr.Server;
        }

        public SocketListener(Socket sock)
        {
            if (sock == null)
                throw new ArgumentNullException("sock");
            _lsnrSock = sock;
        }
        #endregion

        //----
        #region Conversion To operators
        public static implicit operator SocketListener(BluetoothListener lsnr)
        {
            return new SocketListener(lsnr);
        }

        public static implicit operator SocketListener(IrDAListener lsnr)
        {
            return new SocketListener(lsnr);
        }

        public static implicit operator SocketListener(TcpListener lsnr)
        {
            return new SocketListener(lsnr);
        }

        public static implicit operator SocketListener(Socket sock)
        {
            return new SocketListener(sock);
        }
        #endregion

        //----
        #region Methods
        public SocketClient Accept()
        {
            SocketClient cli;
            if (_lsnrBt != null)
                cli = new SocketClient(_lsnrBt.AcceptBluetoothClient());
            else 
                cli = new SocketClient(_lsnrSock.Accept());
            return cli;
        }

        internal void Close()
        {
            if (_lsnrBt != null)
                _lsnrBt.Stop();
            else
                _lsnrSock.Close();
        }
        #endregion

        //----
        #region Properties
        public EndPoint LocalEndPoint
        {
            get
            {
                EndPoint ep;
                if (_lsnrBt != null)
                    ep = _lsnrBt.LocalEndPoint;
                else
                    ep = _lsnrSock.LocalEndPoint;
                return ep;
            }
        }
        #endregion

    }
}
