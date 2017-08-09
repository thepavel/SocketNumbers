using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
namespace Server
{
    public class ServerSettings
    {
        public ServerSettings() {
            
        }

        public ServerSettings(IConfigurationSection configSection) {

            if (configSection != null)
            {
                Port = GetIntValue(configSection, "Port");
                MaxClients = GetIntValue(configSection, "MaxClients");
                OutputIntervalSeconds = int.Parse(configSection["OutputInterval"]);
                LogName = configSection["LogName"];
                TerminateCommand = configSection["TerminateCommand"];
            }
        }
        public int Port { get; set; }
        public int MaxClients { get; set; }
        public int OutputIntervalSeconds { get; set; }
        public string LogName { get; set; }
        public string TerminateCommand { get; set; }

        public static Dictionary<string, dynamic> DefaultSettings => new Dictionary<string, dynamic>() {
                    { "Port", 4000},


                };

        public static int GetIntValue(IConfigurationSection section, string key) {
            return section[key] != null ? int.Parse(section[key]) : int.Parse(DefaultSettings[key]);
        }
    }
}
