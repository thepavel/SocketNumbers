using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.CommandLineUtils;
using log4net.Core;
using log4net;
using System.Reflection;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Server
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        private static Listener Listener { get; set; }

        static void Main(string[] args)
        {
            var configuration = new AppConfig(args);
            var serverSettings = new ServerSettings(configuration.GetSection("Server"));

            ConfigureLogging(Level.Info);

            LaunchServer(serverSettings);

            Console.WriteLine($"Listening on {serverSettings.Port}");

            Console.WriteLine("Hello World!");
        }

        private static void LaunchServer(ServerSettings settings)
        {
            var cmd = new CommandLineApplication()
            {
                FullName = "A socket server which will log 9-digit numbers that are sent to it.",
                Name = "dotnet run --"
            };
            cmd.OnExecute(() =>
            {
                Run(settings);
                return 0;
            });
        }

        private static void Run(ServerSettings settings)
        {
            
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
