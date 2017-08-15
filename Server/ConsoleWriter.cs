using System;
using System.IO;
using System.Threading;
using log4net;

namespace Server
{
    internal class ConsoleWriter
    {
        private ManualResetEventSlim StopSignal { get; }
        private int IntervalSeconds { get; }
        private ILog Log { get; }
        private readonly object _lock;

        public ConsoleWriter(int intervalSeconds)
        {
            _lock = new object();
            StopSignal = new ManualResetEventSlim();
            IntervalSeconds = intervalSeconds;
            Log = LogManager.GetLogger(typeof(ConsoleWriter));
        }

        public void Start() {
            StopSignal.Reset();
            var thread = new Thread(new ThreadStart(() =>
            {
                while (!StopSignal.IsSet)
                {
                    StopSignal.Wait(IntervalSeconds * 1000);
                    if (StopSignal.IsSet)
                    {
                        break;
                    }
                    WriteOutput();
                }
            }));
            thread.Start();
        }

        public void WriteOutput() {
            using (var writer = new StringWriter()) {
                
            }
        }

        public void Stop()
        {
            StopSignal.Set();
        }
    }
}