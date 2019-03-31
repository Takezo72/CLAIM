namespace CLAIM.Helpers.Configuration
{
    public class FileUploadConfigurationHelper : ConfigurationBaseHelper
    {
        private const string ENABLE_FILE_UPLOAD = "EnableFileUpload";
        private const string URL_FILE_UPLOAD = "UrlFileUpload";
        private const string FILE_MAX_LENGTH = "FileMaxLength";
        private const string FILE_MIN_LENGTH = "FileMinLength";
        private const string FILENAME_MAX_LENGTH = "FileNameMaxLength";
        private const string FILE_TYPES = "FileTypes";
        private const string MAX_NUMBER_FILE = "MaxNumberFile";
        private const string FILE_DEPOT_URL = "FileDepotUrl";

        public bool EnableFileUpload => GetAppSettingOrEmpty<string>(ENABLE_FILE_UPLOAD).ToUpper() == "TRUE";

        public string UrlFileUpload => GetAppSettingOrEmpty<string>(URL_FILE_UPLOAD);

        public int FileMaxLength => int.Parse(GetAppSettingOrEmpty<string>(FILE_MAX_LENGTH) != null ? GetAppSettingOrEmpty<string>(FILE_MAX_LENGTH) : "0");

        public int FileMinLength => int.Parse(GetAppSettingOrEmpty<string>(FILE_MIN_LENGTH) != null ? GetAppSettingOrEmpty<string>(FILE_MIN_LENGTH) : "0");

        public int FileNameMaxLength => int.Parse(GetAppSettingOrEmpty<string>(FILENAME_MAX_LENGTH) != null ? GetAppSettingOrEmpty<string>(FILENAME_MAX_LENGTH) : "0");
        public int MaxNumberFile => int.Parse(GetAppSettingOrEmpty<string>(MAX_NUMBER_FILE) != null ? GetAppSettingOrEmpty<string>(MAX_NUMBER_FILE) : "0");

        public string FileTypes => GetAppSettingOrEmpty<string>(FILE_TYPES);
        public string FileDepotUrl => GetAppSettingOrEmpty<string>(FILE_DEPOT_URL);
    }
}

