using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.CommandLineUtils;
using log4net.Core;
using log4net;
using System.Reflection;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System.IO;
using System.Threading;

namespace Server
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        private static Listener Listener { get; set; }
        private static ManualResetEventSlim ExitSignal = new ManualResetEventSlim();

        static void Main(string[] args)
        {
            var serverSettings = AppConfig.ServerSettings;

            ConfigureLogging(Level.Info);

            LaunchServer(args);

            Console.WriteLine($"Listening on {serverSettings.Port}");

            Console.WriteLine("Hello World!");
        }

        private static void LaunchServer(string[] args)
        {
            var cmd = new CommandLineApplication()
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
            Listener = new Listener();
            Listener.Run(Terminate);

            Console.CancelKeyPress += (sender, e) => StopServer();
            ExitSignal.Wait();
        }

        private static void Terminate()
        {
            Console.WriteLine("Terminate command received");
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
            catch
            {

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
