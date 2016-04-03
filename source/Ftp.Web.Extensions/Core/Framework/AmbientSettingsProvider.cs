
namespace Ftp.WebJobs.Extensions.Framework
{
    public class AmbientSettingsProvider
    {
        private static readonly AmbientSettingsProvider Singleton = new AmbientSettingsProvider();

        internal static readonly string Prefix = "AzureWebJobs";

        private AmbientSettingsProvider()
        {
        }

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        public static AmbientSettingsProvider Instance
        {
            get { return Singleton; }
        }

        /// <summary>
        /// Attempts to first read a setting from the appSettings configuration section.
        /// If not found there, it will attempt to read from environment variables.
        /// </summary>
        /// <param name="settingName">The name of the setting to look up.</param>
        /// <returns>The setting, or <see langword="null"/> if no setting was found.</returns>
        public string GetSetting(string settingName)
        {
            // first try prefixing
            string prefixedSettingName = GetPrefixedSettingName(settingName);
            string setting = ConfigurationUtility.GetSettingFromConfigOrEnvironment(prefixedSettingName);

            if (string.IsNullOrEmpty(setting))
            {
                // next try a direct unprefixed lookup
                setting = ConfigurationUtility.GetSettingFromConfigOrEnvironment(settingName);
            }

            return setting;
        }

        internal static string GetPrefixedSettingName(string settingName)
        {
            return Prefix + settingName;
        }
    }
}
