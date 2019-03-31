using System;
using System.Configuration;

namespace CLAIM.Helpers.Configuration
{
    public abstract class ConfigurationBaseHelper
    {
        protected T GetAppSettingOrEmpty<T>(string key)
        {
            if (ConfigurationManager.AppSettings[key] == null || string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
            {
                return typeof(T).IsValueType ? Activator.CreateInstance<T>() : default(T);
            }
            return (T)(object)ConfigurationManager.AppSettings[key];
        }
    }
}