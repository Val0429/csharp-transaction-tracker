using System.Configuration;

namespace Constant.Configuration
{
    public static class ConfigurationUtility
    {
        public static void RefreshSection(string sectionName)
        {
            ConfigurationManager.RefreshSection(sectionName);
        }

        public static string GetAppSetting(this System.Configuration.Configuration configuration, string key)
        {
            var setting = configuration.AppSettings.Settings[key];

            return setting != null ? setting.Value : null;
        }

        public static void SetAppSetting(this System.Configuration.Configuration configuration, string key, string value)
        {
            var setting = configuration.AppSettings.Settings[key];
            if (setting == null)
            {
                setting = new KeyValueConfigurationElement(key, value);
                configuration.AppSettings.Settings.Add(setting);
            }
            else
            {
                setting.Value = value;
            }
        }

        public static string GetEncryptedAppSetting(this System.Configuration.Configuration configuration, string key)
        {
            var encrypted = configuration.GetAppSetting(key);
            
            if (string.IsNullOrEmpty(encrypted)) return null;

            var original = Encryptions.DecryptDES(encrypted);

            return original;
        }

        public static void SetEncryptedAppSetting(this System.Configuration.Configuration configuration, string key, string original)
        {
            var encrypted = Encryptions.EncryptDES(original);

            configuration.SetAppSetting(key, encrypted);
        }

        public static string GetAppSetting(string key, bool keepLatest = true)
        {
            if (keepLatest)
            {
                ConfigurationManager.RefreshSection("appSettings");
            }
            return ConfigurationManager.AppSettings.Get(key);
        }

        public static void SetAppSetting(string key, string value)
        {
            ConfigurationManager.AppSettings.Set(key, value);
        }

        public static void SaveAppSetting(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.SetAppSetting(key, value);
            config.Save(ConfigurationSaveMode.Modified);
        }

        public static string GetEncryptedAppSetting(string key)
        {
            var encrypted = ConfigurationManager.AppSettings.Get(key);

            if (string.IsNullOrEmpty(encrypted)) return null;

            var original = Encryptions.DecryptDES(encrypted);

            return original;
        }

        public static void SetEncyptedAppSetting(string key, string original)
        {
            var encrypted = Encryptions.EncryptDES(original);

            SetAppSetting(key, encrypted);
        }

        public static bool HasSection(string section)
        {
            try
            {
                object obj = ConfigurationManager.GetSection(section);
                
                return obj != null;
            }
            catch (ConfigurationErrorsException)
            {
                return false;
            }
        }
    }
}
