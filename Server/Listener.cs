using System;
namespace Server
{
    internal class Listener : IDisposable
    {
        private ConsoleWriter ConsoleWriter { get; }
        private ServerSettings Settings { get; }

        public Listener(ServerSettings settings) {
            Settings = settings;
            ConsoleWriter = new ConsoleWriter();
        }



        public void Dispose()
        {
            ConsoleWriter.Stop();
        }
    }
}