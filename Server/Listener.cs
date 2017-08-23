using System;
using System.IO;
using System.Text;
namespace Server
{
    internal class Listener : IDisposable
    {
        private ConsoleWriter ConsoleWriter { get; }
        private ServerSettings Settings { get; }
        private SocketListener SocketListener { get; }
        private FileStream LogFile { get; }
        private QueingLogWriter LogWriter { get; }          

        public Listener() {
            
            Settings = AppConfig.ServerSettings;
            ConsoleWriter = new ConsoleWriter(Settings.OutputIntervalSeconds);
            SocketListener = new SocketListener();
            LogFile = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), Settings.LogName), FileMode.Create);
            LogWriter = new QueingLogWriter(new StreamWriter(LogFile, Encoding.UTF8));
        }

        public void Dispose()
        {
            LogWriter.Dispose();
            LogFile.Dispose();

            SocketListener.Stop();
            ConsoleWriter.Stop();
        }

        internal void Run(Action terminate)
        {
            ConsoleWriter.Start();
            SocketListener.Start(socket => {
                var reader = new SocketStreamReader(socket);
                reader.Read(ProcessValue, terminate);
            });
        }

        protected void ProcessValue(int value)
        {
            if(LogWriter.WriteUnique(value)) {
                ConsoleWriter.RecordUnique();
            }else {
                ConsoleWriter.RecordDuplicate();
            }
        }
    }
}