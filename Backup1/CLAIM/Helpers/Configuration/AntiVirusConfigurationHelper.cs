using System;
using System.Configuration;

namespace CLAIM.Helpers.Configuration
{
    public class AntiVirusConfigurationHelper : ConfigurationBaseHelper
    {

        private const string ACTIVE = "AntivirusActive";
        private const string MALWARE_CODES = "AntivirusMalwareCodes";

        private const string ICAP_SERVER_IP = "IcapServerIp";
        private const string ICAP_SERVER_PORT = "IcapServerPort";
        private const string ICAP_OVER_SSL = "IcapOverSSL";
        private const string ICAP_SERVICE = "IcapService";
        private const string ICAP_PROFILE = "IcapProfile";

        public bool Active => bool.Parse(GetAppSettingOrEmpty<string>(ACTIVE));
        public string[] MalwareCodes => GetAppSettingOrEmpty<string>(MALWARE_CODES).Split(',');
        public string IcapServerIp => GetAppSettingOrEmpty<string>(ICAP_SERVER_IP);
        public int IcapServerPort => int.Parse(GetAppSettingOrEmpty<string>(ICAP_SERVER_PORT));
        public bool IcapOverSSL => bool.Parse(GetAppSettingOrEmpty<string>(ICAP_OVER_SSL));
        public string IcapService => GetAppSettingOrEmpty<string>(ICAP_SERVICE);
        public string IcapProfile => GetAppSettingOrEmpty<string>(ICAP_PROFILE);
    }
}
