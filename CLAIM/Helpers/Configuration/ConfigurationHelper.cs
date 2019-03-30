namespace CLAIM.Helpers.Configuration
{
    public class ConfigurationHelper : ConfigurationBaseHelper
    {
        private const string REQUEST_FOLLOWUP_RETURN_URL_AUTH = "RequestFollowUpReturnUrlAuth";
        private const string REQUEST_FOLLOWUP_RETURN_URL_NOAUTH = "RequestFollowUpReturnUrlNoAuth";
        private const string REQUEST_FOLLOWUP_RETURN_URL_DENIED = "RequestFollowUpReturnUrlDenied";
        private const string IACA_RETURN_URL = "IACAReturnUrl";
        private const string IS_MOCK_USER_IDENTITY = "IsMockUserIdentity";

        private const string HELP_TELEPHONE = "HelpTelephone";
        private const string HELP_MAIL = "HelpMail";

        private const string CANADA_POST_API_KEY = "CanadaPostAPIKey";

        //public string RequestFollowUpReturnUrl => GetAppSettingOrEmpty<string>(WE.Security.Identity.UserIdentity.IsGuestUser() ? REQUEST_FOLLOWUP_RETURN_URL_NOAUTH : REQUEST_FOLLOWUP_RETURN_URL_AUTH);

        public string RequestFollowUpReturnUrlDenied => GetAppSettingOrEmpty<string>(REQUEST_FOLLOWUP_RETURN_URL_DENIED);

        public string IACAReturnUrl => GetAppSettingOrEmpty<string>(IACA_RETURN_URL);

        public bool IsMockUserIdentity => GetAppSettingOrEmpty<string>(IS_MOCK_USER_IDENTITY)?.ToUpper() == "TRUE";

        public string HelpTelephone => GetAppSettingOrEmpty<string>(HELP_TELEPHONE);
        public string HelpMail => GetAppSettingOrEmpty<string>(HELP_MAIL);
        public string CanadaPostAPIKey => GetAppSettingOrEmpty<string>(CANADA_POST_API_KEY);
    }
}
