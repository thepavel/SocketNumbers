using System;
using System.IO;
using System.Threading;
using log4net;
using Server.Models;

namespace Server
{
    internal class ConsoleWriter
    {
        private const int DefaultInterval = 10;
        private ManualResetEventSlim StopSignal { get; }
        private int IntervalSeconds { get; }
        private ILog Log { get; }
        private ConsoleReportModel Model { get; }
        private readonly object _lock;

        public ConsoleWriter()
        {
            _lock = new object();
            StopSignal = new ManualResetEventSlim();
            Log = LogManager.GetLogger(typeof(ConsoleWriter));
            Model = new ConsoleReportModel();
            IntervalSeconds = DefaultInterval;
        }

        public ConsoleWriter(int intervalSeconds)
        {
            IntervalSeconds = intervalSeconds;

        }

        public void Start()
        {
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

        public void RecordDuplicate()
        {
            lock (_lock)
            {
                Model.IncrementalDuplicateCount++;
                Model.TotalDuplicateCount++;
            }
        }

        public void RecordUnique()
        {
            lock (_lock)
            {
                Model.IncrementalUniqueCount++;
                Model.TotalUniqueCount++;
            }
        }

        public void WriteOutput()
        {
            using (var writer = new StringWriter())
            {
                ConsoleOut(writer);
                Log.Info(writer.ToString());
            }
        }

        public void ConsoleOut(StringWriter writer)
        {
            var model = new ConsoleReportModel();
            lock (_lock)
            {
                model = new ConsoleReportModel(Model);
                Model.IncrementalDuplicateCount = Model.IncrementalUniqueCount = 0;

            }
            writer.Write($"Received {model.IncrementalUniqueCount} unique numbers, {model.IncrementalDuplicateCount} duplicates. Unique total: {model.TotalUniqueCount}");

        }

        public void Stop()
        {
            StopSignal.Set();
        }
    }
}