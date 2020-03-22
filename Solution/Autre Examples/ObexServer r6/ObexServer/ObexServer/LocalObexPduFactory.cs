using System;
using System.Collections.Generic;
using System.Text;
using Brecham.Obex.Pdus;

namespace ObexServer
{

    //TODO Create a clean PduFactory base type infrastructure.
    /// <summary>
    /// Just allow the server class access to any protected methods of ObexPduFactory
    /// that it needs.
    /// </summary>
    internal class LocalObexPduFactory : ObexPduFactory
    {
        public LocalObexPduFactory(int bufferSize)
            : base(bufferSize)
        { }

        //internal new byte[] MakeConnectBytes()
        //{
        //    return base.MakeConnectBytes();
        //}

        internal void ServerUseConnectBytes(byte[] bytes)
        {
            base.UseConnectBytes(bytes);
        }
    }//class


}
