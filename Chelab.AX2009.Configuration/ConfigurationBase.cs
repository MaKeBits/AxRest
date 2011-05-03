using System;
using System.Collections.Generic;

namespace Chelab.AX2009.Configuration
{
    /// <summary>
    /// Classe che immagazzina le configurazioni
    /// La classe è generica e le configurazioni vengono immagazzinate in un dizionario generico
    /// </summary>
    public class ConfigurationBase
    {
        #region Private Properties

        private Dictionary<String, object> baseDictionary;

        private const String NOT_FOUND_CONFIGURATION = @"La chiave ""{0}"" non è presente nella configurazione";

        #endregion

        #region CTOR

        public ConfigurationBase()
        {
            baseDictionary = new Dictionary<string, object>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Return the object Value of the selected key
        /// </summary>
        /// <param name="_keyName">Name of the key</param>
        /// <returns>object Value</returns>
        public object getValue(String _keyName)
        {
            if (String.IsNullOrEmpty(_keyName))
                return null;

            if (baseDictionary.ContainsKey(_keyName))
                return baseDictionary[_keyName];
            else
                return null;
        }

        public String getStringValue(String _keyName)
        {
            var value = getValue(_keyName);
            if (value == null)
                return String.Empty;

            if (value is string)
                return (String)value;
            else
                return String.Empty;
        }

        public Guid getGuidValue(String _keyName)
        {
            var value = getStringValue(_keyName);
            if (String.IsNullOrEmpty(value))
                return Guid.Empty;

            try
            {
                return new Guid(value);
            }
            catch
            {
                return Guid.Empty;
            }

        }

        /// <summary>
        /// Load one value in dictionary
        /// </summary>
        /// <param name="_keyName"></param>
        /// <param name="_value"></param>
        public void loadConfiguration(String _keyName, object _value)
        {
            if (String.IsNullOrEmpty(_keyName))
                return;
            if (_value == null)
                return;

            if (baseDictionary.ContainsKey(_keyName))
                baseDictionary[_keyName] = _value;
            else
                baseDictionary.Add(_keyName, _value);
        }

        /// <summary>
        /// Controlla la presenza delle chiavi selezionate nelle configurazioni
        /// </summary>
        /// <param name="keys">Elenco di chiavi</param>
        /// <param name="Message">Messaggio di risposta</param>
        /// <returns></returns>
        public bool checkConfigurationKeys(String[] keys, out String Message)
        {
            Message = String.Empty;
            foreach (var key in keys)
            {
                if (!baseDictionary.ContainsKey(key))
                {
                    Message = String.Format(NOT_FOUND_CONFIGURATION, key);
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
