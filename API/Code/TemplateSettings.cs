using GW.Core;
using GW.Common;
using GW.ApplicationHelpers;
using Newtonsoft.Json;

namespace Template.API
{
    public class TemplateSettings : ISettings
    {

        private IConfiguration _env;

        public TemplateSettings()
        {
            
        }

        public TemplateSettings(IConfiguration webhost)
        {
            _env = webhost;
            LoadSettings();
        }

        public SourceConfig[] Sources { get; set; }

        public string SiteURL { get; set; }

        public string ProfileImageDir { get; set; }

        public string FileStorageConnection { get; set; }

        public string ApplicationName { get; set; }

        public MailSettings MailSettings { get; set; }

        public string LocalizationLanguage { get; set; }

        public int ContextLength { get; set; }

        public void LoadSettings()
        {
            this.Sources= new SourceConfig[1];

            this.Sources[0] = new SourceConfig(); 
            this.Sources[0].SourceValue = _env["Sources01"];
            this.Sources[0].SourceName = "Default";

            this.SiteURL = _env["SiteURL"];
            this.ProfileImageDir = _env["ProfileImageDir"];
            this.FileStorageConnection = _env["FileStorageConnection"];
            this.ApplicationName = _env["ApplicationName"];            
            this.LocalizationLanguage = _env["LocalizationLanguage"];
            this.ContextLength = (int)_env.GetValue(typeof(int), "ContextLength");
                       
            this.MailSettings = new MailSettings();
            this.MailSettings.SMTPServer = _env["SMTPServer"];
            this.MailSettings.SMTPUser = _env["SMTPUser"];
            this.MailSettings.SMTPPassword = _env["SMTPPassword"];
            this.MailSettings.Port = _env["Port"];
            this.MailSettings.EmailSender = _env["EmailSender"];
            this.MailSettings.NameSender = _env["NameSender"];

        }


    }
}
