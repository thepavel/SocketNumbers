using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Server
{
    public class ServerSettings
    {
        IConfigurationSection ConfigurationSection { get; }

        public ServerSettings(IConfigurationSection configSection)
        {
            ConfigurationSection = configSection;

            Port = GetIntValue("Port");
            MaxClients = GetIntValue("MaxClients");
            OutputIntervalSeconds = GetIntValue("OutputInterval");
            LogName = GetStringValue("LogName");
            TerminateCommand = GetStringValue("TerminateCommand");
            InputLength = GetIntValue("InputLength");
        }
        public int Port { get; }
        public int MaxClients { get; }
        public int OutputIntervalSeconds { get; }
        public string LogName { get; }
        public string TerminateCommand { get; }
        public int InputLength { get; }

        public static Dictionary<string, string> DefaultSettings => new Dictionary<string, string> {
                    { "Port", "4000"},
                    { "MaxClients", "5"},
                    { "OutputInterval", "10"},
                    { "LogName", "numbers.log"},
                    { "TerminateCommand", "terminate"},
                    { "InputLength", "9"}
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
