using System;
using System.Threading;

namespace Server
{
    internal class ConsoleWriter
    {
        private readonly ManualResetEventSlim StopSignal;

        public ConsoleWriter()
        {
            StopSignal = new ManualResetEventSlim();
        }

        public void Stop()
        {
            StopSignal.Set();
        }
    }
}