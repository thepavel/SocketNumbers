using System;
using System.Net.Sockets;

namespace Server
{
    internal class SocketConnectionProxy : ISocketConnectionProxy
    {
        private Socket socket;

        public SocketConnectionProxy(Socket socket)
        {
            this.socket = socket;
        }

        public int Receive(byte[] buffer, int offset, int size)
        {
            return socket.Receive(buffer, offset, size, SocketFlags.None);
        }
    }
}