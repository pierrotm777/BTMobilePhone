using System;
using System.Net;
using System.Net.Sockets;
using InTheHand.Net.Sockets;
using System.Diagnostics.CodeAnalysis;

namespace CmdlineRunner
{
    /// <summary>
    /// Stores a reference to a connected <see cref="T:System.Net.Sockets.Socket"/>
    /// or a <see cref="T:InTheHand.Net.Sockets.BluetoothClient"/> etc 
    /// to provide access to its <see cref="T:System.Net.Sockets.NetworkStream"/> etc
    /// </summary>
    /// -
    /// <remarks>
    /// <para>There's no common superclass/interface to <see cref="T:System.Net.Sockets.Socket"/>
    /// and the various "Client" wrappers <see cref="T:System.Net.Sockets.TcpClient"/>,
    /// <see cref="T:InTheHand.Net.Sockets.BluetoothClient"/> etc.  Thus it's difficult
    /// when writing a multi-protocol server (or client) to know how to pass/store
    /// a socket.  Originally we used a <see cref="T:System.Net.Sockets.Socket"/>
    /// both in the listening and connected cases, but with the addition of Widcomm
    /// support to 32feet.NET this is no longer possible.  That's because Widcomm
    /// does not use sockets so there's no way to get a <see cref="T:System.Net.Sockets.Socket"/>
    /// there (<see cref="M:InTheHand.Net.Sockets.BluetoothClient.GetStream">BluetoothClient.GetStream</see>
    /// returns error <see cref="T:System.NotSupportedException"/>).
    /// </para>
    /// <para>So we've created this pair of wrappers, <see cref="T:CmdlineRunner.SocketClient"/>
    /// and <see cref="T:CmdlineRunner.SocketListener"/>.  They are meant to simply
    /// provide access to the server's Accept method, and the client's GetStream
    /// method; and some basic properties e.g. <see cref="P:CmdlineRunner.SocketListener.LocalEndPoint"/>.
    /// </para>
    /// <para>We do not want to add lots of functionality to them, adding all of
    /// the IO methods for instance.  If you really need access to Socket's advanced
    /// IO methods then we should provide a method to return the Socket if its available
    /// in that case (e.g. method named <c>GetSocketIfSupported</c>.
    /// </para>
    /// </remarks>
    /// -
    /// <seealso cref="T:CmdlineRunner.SocketListener"/>
    public class SocketClient
    {
        #region Fields
        // We have to store the BluetoothClient and not a Socket and on Widcomm(/etc?)
        // there's no Socket to store, BluetoothListener.Client returns NotSupportedException.
        readonly BluetoothClient _cliBt;
        readonly Socket _cliSock;
        NetworkStream _strm;
        #endregion

        #region Constructors
        internal SocketClient(BluetoothClient cli)
        {
            if (cli == null)
                throw new ArgumentNullException("cli");
            _cliBt = cli;
        }

        internal SocketClient(IrDAClient cli)
        {
            if (cli == null)
                throw new ArgumentNullException("cli");
            // No reason to not store just its internal socket for now.
            _cliSock = cli.Client;
        }

        internal SocketClient(TcpClient cli)
        {
            if (cli == null)
                throw new ArgumentNullException("cli");
            // No reason to not store just its internal socket for now.
            _cliSock = cli.Client;
        }

        internal SocketClient(Socket sock)
        {
            if (sock == null)
                throw new ArgumentNullException("sock");
            _cliSock = sock;
        }
        #endregion

        //----
        #region Methods
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public NetworkStream GetStream()
        {
            if (_strm == null) {
                if (_cliBt != null)
                    _strm = _cliBt.GetStream();
                else
                    _strm = new NetworkStream(_cliSock, true);
            }
            return _strm;
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public Socket GetSocketIfAvailable()
        {
            Socket sock;
            if (_cliBt != null)
                sock = _cliBt.Client;
            else
                sock = _cliSock;
            return sock;
        }
        #endregion

        //----
        #region Properties
        public EndPoint RemoteEndPoint
        {
            get
            {
                EndPoint ep;
                if (_cliBt != null)
                    ep = _cliBt.RemoteEndPoint;
                else
                    ep = _cliSock.RemoteEndPoint;
                return ep;
            }
        }
        #endregion

    }
}
