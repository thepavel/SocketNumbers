using System;
namespace Server
{
    internal class Listener : IDisposable
    {
        private ConsoleWriter ConsoleWriter { get; }

        public Listener(ServerSettings settings) {
            Settings = settings;
            ConsoleWriter = new ConsoleWriter();
        }

        public ServerSettings Settings { get; private set; }

        public void Dispose()
        {
            ConsoleWriter.Stop();
        }
    }
}