using System;
using Microsoft.Extensions.Configuration;

namespace Server
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }


        static void Main(string[] args)
        {
            var configuration = new AppConfig(args);
            var serverSettings = new ServerSettings(configuration.GetSection("Server"));

            Console.WriteLine($"Listening on {serverSettings.Port}");

            Console.WriteLine("Hello World!");
        }


    }
}
