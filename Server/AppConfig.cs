using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Server
{
    public class AppConfig
    {

        public IConfigurationRoot Settings { get; }
        public AppConfig()
        {
            Settings = GetConfiguration();
        }

        public AppConfig(string[] args)
        {
            Settings = GetConfiguration(args);

        }

        private static IConfigurationRoot GetConfiguration(string[] args = null)
        {
            var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("config.json");
            if (args != null)
            {
                builder = builder.AddCommandLine(args);
            }
            return builder.Build();
        }

        public IConfigurationSection GetSection(string name)
        {
            return Settings.GetSection(name);
        }
    }
}
