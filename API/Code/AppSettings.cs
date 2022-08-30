using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using GW.Core.Common;
using Newtonsoft.Json;
using GW.Core.Helpers;

namespace Template.API
{
    public class TemplateSettings : AppSettings
    {
        public List<ConnectionString> Connections { get; set; }

        public string SiteURL { get; set; }

        public string ProfileImageDir { get; set; }

        public string NomeSistema { get; set; }

       // public string ServerRootPath { get; set; }
                
        public MailSettings MailSettings { get; set; }


    }

    public class TemplateManagerSettings : IAppSettingsManager<TemplateSettings>
    {
        public TemplateSettings Settings { get; set; }
        
        public object EnvironmentSettings  
        {
            get
            {
                return _env;
            } 

            set
            {
                _env = (IWebHostEnvironment)value;
            }
        }

        private IWebHostEnvironment _env;

        public TemplateManagerSettings(IWebHostEnvironment webhost)
        {
            _env = (IWebHostEnvironment)webhost;
            
            LoadSettings(null);
        }


        public void LoadSettings(HttpClient http = null)
        {
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
                    this.Settings = JsonConvert.DeserializeObject<TemplateSettings>(jsontxt);
                    if (this.Settings != null)
                    {
                        if (this.Settings.ProfileImageDir == "")
                        {
                            this.Settings.ProfileImageDir = _env.ContentRootPath + @"FileServer\UserProfileImage";
                        }
                    }

                }
            }
        }

       
    }

   

}