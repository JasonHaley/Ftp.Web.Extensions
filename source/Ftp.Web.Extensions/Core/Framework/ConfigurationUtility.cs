
using System;
using System.Configuration;

namespace Ftp.WebJobs.Extensions.Framework
{
    // from Microsoft.Azure.WebJobs.Host

    internal static class ConfigurationUtility
    {
        public static string GetSettingFromConfigOrEnvironment(string settingName)
        {
            string configValue = ConfigurationManager.AppSettings[settingName];
            if (!string.IsNullOrEmpty(configValue))
            {
                // config values take precedence over environment values
                return configValue;
            }

            return Environment.GetEnvironmentVariable(settingName) ?? configValue;
        }
    }
}
