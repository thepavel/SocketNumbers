using System.Net.Sockets;

namespace Server
{
    class SocketConnectionProxy
    {

        public SocketConnectionProxy(Socket socket)
        {
            Socket = socket;
        }

        private Socket Socket { get; }

        public int Receive(byte[] buffer, int offset, int size)
        {
            return Socket.Receive(buffer, offset, size, SocketFlags.None);
        }
    }
}