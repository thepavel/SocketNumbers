using System;
using System.Reflection;
using System.Threading;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.CommandLineUtils;

namespace Server
{
    class Program
    {

        private static Listener Listener => new Listener();
        private static ManualResetEventSlim ExitSignal = new ManualResetEventSlim();

        static void Main(string[] args)
        {
            ConfigureLogging(Level.Info);

            var cmd = new CommandLineApplication
            {
                FullName = "A socket server which will log 9-digit numbers that are sent to it.",
                Name = "dotnet run --"
            };
            cmd.OnExecute(() =>
            {
                
                Run();
                return 0;
            });
            cmd.Execute(args);
        }

        private static void Run()
        {
            
            Listener.Run(Terminate);

            Console.CancelKeyPress += (sender, e) => KillCmd();
            ExitSignal.Wait();
        }

        private static void KillCmd() {
            Console.WriteLine("");
            Console.WriteLine("Cancel detected from console");
            StopListener();

        }

        private static void Terminate()
        {
            Console.WriteLine("Terminate command received");
            StopListener();
        }

        private static void StopListener()
        {
            StopServer();
            ExitSignal.Set();
        }

        private static void StopServer()
        {
            Console.WriteLine("Stopping server");
            try
            {
                Listener.Dispose();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Exception while stopping server. Exception: {e.Message}");
            }
        }

        private static void ConfigureLogging(Level level)
        {
            var logsRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            var appender = new ConsoleAppender
            {
                Layout = new PatternLayout("%message%newline")
            };
            ((Hierarchy)logsRepository).Root.Level = level;
            log4net.Config.BasicConfigurator.Configure(logsRepository, appender);
        }



    }
}
