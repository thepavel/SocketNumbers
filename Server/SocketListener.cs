using log4net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    public class SocketListener
    {
        private readonly int Port;
        private readonly int MaxConnections;
        private Socket Socket;
        private List<Socket> Connections;
        private object _lock;

        static ILog Log = LogManager.GetLogger(typeof(SocketListener));

        public SocketListener()
        {
            var settings = AppConfig.ServerSettings;
            Port = settings.Port;
            MaxConnections = settings.MaxClients;
            Connections = new List<Socket>();
            _lock = new object();
        }

        public void Start(Action<Socket> newSocketConnectionCallback)
        {
            // Start thread for server socket that will listen for
            // new connections.
            var thread = new Thread(new ThreadStart(() =>
            {
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket.Bind(new IPEndPoint(IPAddress.Loopback, Port));
                Socket.Listen(MaxConnections);

                Log.Info($"Listening for socket connections on port {Port}...");

                BlockAndAcceptConnections(newSocketConnectionCallback);
            }));
            thread.Start();
        }

        public void Stop()
        {
            lock (_lock)
            {
                Connections.ForEach(ShutdownSocket);
            }

            if (Socket != null)
            {
                Socket.Dispose();
                Socket = null;
            }
        }

        private void BlockAndAcceptConnections(Action<Socket> newSocketConnectionCallback)
        {
            while (Socket != null)
            {
                Socket connection;
                try
                {
                    // Blocking method
                    connection = Socket.Accept();
                }
                catch (SocketException ex)
                {
                    Log.Debug($"Socket accept failed: {ex.Message}");
                    continue;
                }

                if (ShouldRefuseConnection)
                {
                    // We already have the max number of connections.
                    ShutdownSocket(connection);
                    Log.Info("Socket connection refused.");
                    continue;
                }

                Log.Info("Socket connection accepted.");

                DispatchThreadForNewConnection(connection, newSocketConnectionCallback);
            }
        }

        bool ShouldRefuseConnection
        {
            get
            {
                lock (_lock)
                {
                    return Connections.Count >= MaxConnections;
                }
            }
        }

        private void DispatchThreadForNewConnection(Socket connection, Action<Socket> newSocketConnectionCallback)
        {
            // Create thread to manage new socket connection.
            // Will stay alive as long as callback is executing.
            var thread = new Thread(new ThreadStart(() =>
            {
                ExecuteCallback(connection, newSocketConnectionCallback);

                lock (_lock)
                {
                    Connections.Remove(connection);
                }
            }));
            thread.Start();

            lock (_lock)
            {
                Connections.Add(connection);
            }
        }

        private static void ExecuteCallback(Socket connection, Action<Socket> newSocketConnectionCallback)
        {
            try
            {
                newSocketConnectionCallback(connection);
            }
            catch (SocketException ex)
            {
                Log.Debug($"Socket connection closed forcibly: {ex.Message}");
            }
            finally
            {
                ShutdownSocket(connection);
                Log.Info("Socket connection closed.");
            }
        }

        private static void ShutdownSocket(Socket socket)
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch (SocketException ex)
            {
                Log.Debug($"Socket could not be shutdown: {ex.Message}");
            }
        }
    }
}