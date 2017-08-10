using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Server
{
    public class ServerSettings
    {
        IConfigurationSection ConfigurationSection { get; }

        public ServerSettings()
        {
            Initialize();
        }

        private void Initialize()
        {
            Port = GetIntValue("Port");
            MaxClients = GetIntValue("MaxClients");
            OutputIntervalSeconds = GetIntValue("OutputInterval");
            LogName = GetStringValue("LogName");
            TerminateCommand = GetStringValue("TerminateCommand");
        }

        public ServerSettings(IConfigurationSection configSection)
        {
            ConfigurationSection = configSection;

            Initialize();
        }
        public int Port { get; set; }
        public int MaxClients { get; set; }
        public int OutputIntervalSeconds { get; set; }
        public string LogName { get; set; }
        public string TerminateCommand { get; set; }

        public static Dictionary<string, string> DefaultSettings => new Dictionary<string, string> {
                    { "Port", "4000"},
                    { "MaxClients", "5"},
                    { "OutputInterval", "10"},
                    { "LogName", "numbers.log"},
                    { "TerminateCommand", "terminate"}
                };

        public string GetStringValue(string key)
        {
            return ConfigurationSection != null && ConfigurationSection[key] != null ? ConfigurationSection[key] : DefaultSettings[key];
        }

        public int GetIntValue(string key)
        {
            return int.Parse(GetStringValue(key));
        }
    }
}
