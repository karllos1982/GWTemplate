using GW.Core;
using GW.Common;
using GW.ApplicationHelpers;

namespace Template.Core.Manager
{
    public class TemplateSettings : ISettings
    {
        public SourceConfig[] Sources { get; set; }

        public string SiteURL { get; set; }

        public string ProfileImageDir { get; set; }

        public string ApplicationName { get; set; }

        public MailSettings MailSettings { get; set; }

        public string LocalizationLanguage { get; set; }
    }
}
