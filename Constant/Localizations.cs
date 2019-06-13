using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using Constant.Utility;

namespace Constant
{
    public static class Localizations
    {
        static Localizations()
        {
            LoadResX(Key);
        }


        public static String Key = "en-us";
        public static List<String> UnTranslateKey = new List<String>();

        public static void Update(Dictionary<String, String> localization)
        {
            UpdateDictionary(localization);
        }

        private static void UpdateDictionary(IDictionary<String, String> localization)
        {
            if (localization == null || Dictionary.Count == 0) return;

            var keys = localization.Keys.ToList();
            foreach (String key in keys)
            {
                if (Dictionary.ContainsKey(key))
                {
                    localization[key] = Dictionary[key];
                }
                else
                {
                    //if you see this, you should add this key to localization file.
                    Debug.WriteLine(string.Format("Please add {0} to localization file.", key));
                }
            }

            OnUpdated();
        }

        public static void Update(IDictionary<String, String> localization)
        {
            UpdateDictionary(localization);
        }

        public static Dictionary<String, String> Dictionary = new Dictionary<String, String>();

        private static void LoadResX(string cultureName)
        {
            var lang = string.Format("lang\\{0}.resx", cultureName);
            String langPath = Path.Combine(GenericUtility.GetWorkingDirectory(), lang);

            Load(langPath);
        }

        public static void Load(CultureInfo cultureInfo)
        {
            LoadResX(cultureInfo.Name);
        }

        public static void Load(string langPath)
        {
            if (File.Exists(langPath))
            {
                var resXResourceReader = new ResXResourceReader(langPath);

                foreach (DictionaryEntry entry in resXResourceReader)
                {
                    Dictionary[entry.Key.ToString()] = entry.Value.ToString().Replace("\\n", Environment.NewLine);
                }

                resXResourceReader.Close();

                OnUpdated();
            }
        }

        public static string GetValueOrDefault(string key, string defaultValue)
        {
            if (Dictionary.ContainsKey(key))
            {
                return Dictionary[key];
            }
            Debug.WriteLine(string.Format("Key ({0}) is not found, use the default value ({1}).", key, defaultValue));

            return defaultValue;
        }


        public static event EventHandler Updated;

        private static void OnUpdated()
        {
            var handler = Updated;
            if (handler != null)
            {
                handler(null, EventArgs.Empty);
            }
        }
    }
}
