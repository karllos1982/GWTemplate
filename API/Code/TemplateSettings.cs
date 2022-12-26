using GW.Core;
using GW.Common;
using GW.ApplicationHelpers;
using Newtonsoft.Json;

namespace Template.API
{
    public class TemplateSettings : ISettings
    {

        private IWebHostEnvironment _env;

        public TemplateSettings()
        {
            
        }

        public TemplateSettings(IWebHostEnvironment webhost)
        {
            _env = (IWebHostEnvironment)webhost;
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
            TemplateSettings settings = new TemplateSettings();
            string filename = "appsettings.json";
            string jsontxt = "";
            string dir = _env.ContentRootPath;

            if (dir == "/app")
            {
                dir = $"{Directory.GetCurrentDirectory()}";
                filename = $"{Directory.GetCurrentDirectory()}{@"/appsettings.json"}";
            }
            else
            {
                filename = dir + "/" + filename;
            }

            if (File.Exists(filename))
            {

                jsontxt = File.ReadAllText(filename);

                if (jsontxt.Length > 0)
                {
                    settings = JsonConvert.DeserializeObject<TemplateSettings>(jsontxt);
                    if (settings != null)
                    {
                        if (settings.ProfileImageDir == "")
                        {
                            settings.ProfileImageDir = _env.ContentRootPath + @"FileServer\UserProfileImage";
                        }

                        this.Sources = settings.Sources;
                        this.SiteURL = settings.SiteURL;
                        this.ProfileImageDir = settings.ProfileImageDir;
                        this.FileStorageConnection = settings.FileStorageConnection;
                        this.ApplicationName = settings.ApplicationName;
                        this.MailSettings = settings.MailSettings;
                        this.LocalizationLanguage = settings.LocalizationLanguage;
                        this.ContextLength = settings.ContextLength;
                    }

                }
            }
            
        }


    }
}
