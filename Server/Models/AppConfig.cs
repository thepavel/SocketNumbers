﻿using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Server
{
    public class AppConfig
    {

        public static IConfigurationRoot Settings => GetConfiguration();

        private static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("config.json");
            return builder.Build();
        }

        public static ServerSettings ServerSettings => new ServerSettings(Settings.GetSection("Server"));

        public IConfigurationSection GetSection(string name)
        {
            return Settings.GetSection(name);
        }


    }
}
