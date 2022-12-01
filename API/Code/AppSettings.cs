using System;
using GW.Common;
using GW.ApplicationHelpers; 
using Newtonsoft.Json;
using Template.Core.Manager;

namespace Template.API
{
 
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

    public interface IUserPermissionsManager<T>
    {
        
        List<UserPermissions> GetPermissions();

        void SetPermissions(List<UserPermissions> permissions); 
    }

    public class UserPermissionsManager : IUserPermissionsManager<UserPermissions>
    {
        public UserPermissionsManager()
        {

        }

        private List<UserPermissions> _permissions; 

        public UserPermissionsManager(List<UserPermissions> permissions)
        {
            _permissions = permissions;
        }   

        public List<UserPermissions> GetPermissions()
        {
            return _permissions;
        }

        public void SetPermissions(List<UserPermissions> permissions)
        {
            _permissions = permissions;
        }
    }

}