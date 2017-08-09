using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Server
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }


        static void Main(string[] args)
        {
            var configuration = GetConfiguration(args);

            Console.WriteLine("Hello World!");
        }

        private static ServerSettings GetConfiguration(string[] args)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("config.json")
                    .AddCommandLine(args);
            
            Configuration = builder.Build();

            return new ServerSettings(Configuration.GetSection("Server"));
        }
    }
}
