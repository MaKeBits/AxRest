using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections;

namespace Chelab.AX2009.Configuration
{
    public class ConfigurationProvider
    {
        #region Public Methods

        /// <summary>
        /// Load String Configuration From AppSettings
        /// </summary>
        /// <param name="_KeyName">Key Name in AppSettings</param>
        /// <param name="DefaultValue">Default String Value</param>
        /// <returns></returns>
        public static String LoadStringConfiguration(String _KeyName, String DefaultValue)
        {
            if (ConfigurationManager.AppSettings[_KeyName] != null)
                return ConfigurationManager.AppSettings[_KeyName];
            else
                return DefaultValue;
        }

        /// <summary>
        /// Load Int Configuration From AppSettings
        /// </summary>
        /// <param name="_KeyName">Key Name in AppSettings</param>
        /// <param name="DefaultValue">Default Int Value</param>
        /// <returns></returns>
        public static int LoadIntConfiguration(String _KeyName, int DefaultValue)
        {
            var value = LoadStringConfiguration(_KeyName, String.Empty);
            if (String.IsNullOrEmpty(value))
                return DefaultValue;

            int result;
            if (!Int32.TryParse(value, out result))
                return DefaultValue;

            return result;
        }

        /// <summary>
        /// Load Bool Configuration From AppSettings
        /// </summary>
        /// <param name="_KeyName">Key Name in AppSettings</param>
        /// <param name="DefaultValue">Default Bool Value</param>
        /// <returns></returns>
        public static bool LoadBoolConfiguration(String _KeyName, bool DefaultValue)
        {
            var value = LoadStringConfiguration(_KeyName, String.Empty);
            if (String.IsNullOrEmpty(value))
                return DefaultValue;

            bool result;
            if (!bool.TryParse(value, out result))
                return DefaultValue;

            return result;
        }

        /// <summary>
        /// Load Guid Configuration From AppSettings
        /// </summary>
        /// <param name="_KeyName">Key Name in AppSettings</param>
        /// <param name="DefaultValue">Default Guid Value</param>
        /// <returns></returns>
        public static Guid LoadGuidConfiguration(String _KeyName, Guid DefaultValue)
        {
            var value = LoadStringConfiguration(_KeyName, String.Empty);
            if (String.IsNullOrEmpty(value))
                return DefaultValue;

            try
            {
                return new Guid(value);
            }
            catch
            {
                return DefaultValue;
            }
        }

        public static ConfigurationBase getConfiguration()
        {
            var configuration = new ConfigurationBase();

            // Get the appSettings.
            NameValueCollection appSettings =
                 ConfigurationManager.AppSettings;

            // Get the collection enumerator.
            IEnumerator appSettingsEnum =
                appSettings.Keys.GetEnumerator();

            // Loop through the collection and
            // display the appSettings key, value pairs.
            while (appSettingsEnum.MoveNext())
            {
                if (appSettingsEnum.Current is String)
                {
                    var keyName = (String)appSettingsEnum.Current;
                    configuration.loadConfiguration(keyName, appSettings[keyName]);
                }
            }
            return configuration;
        }

        #endregion
    }
}
