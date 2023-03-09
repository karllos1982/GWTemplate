using Microsoft.JSInterop;
using Newtonsoft.Json;
using GW.Common;
using GW.Helpers;
using GW.Membership.Models;
using Template.Gateway;
using WebBlazorServer.Pages.SuperAdmin;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Template.ServerCode
{
 
    public class TemplateAppSettings : IAppSettings
    {

        private IConfiguration _env;

        public TemplateAppSettings()
        {

        }

        public TemplateAppSettings(IConfiguration webhost)
        {
            _env = webhost;
            LoadSettings();
        }

        public string SiteURL { get; set; }

        public string ServiceURL { get; set; }

        public string NomeSistema { get; set; }

        public string SessionTimeOut { get; set; }

        public string DefaultLanguage { get; set; }


        public string FileContentMenu { get; set; }

        public List<MenuObject> ContentMenu { get; set; }

        public void LoadSettings(HttpClient http = null)
        {

            this.SiteURL = _env["SiteURL"];
            this.ServiceURL = _env["ServiceURL"];
            this.NomeSistema = _env["NomeSistema"];
            this.DefaultLanguage = _env["DefaultLanguage"];
            this.SessionTimeOut = _env["SessionTimeOut"];
            this.FileContentMenu = _env["FileContentMenu"];

        }
    }
           
    public class TemplateAppController : IAppControllerAsync<UserAuthenticated>
    {
        public UserAuthenticated UserInfo { get; set; }
        
        private bool _IsAuthenticated = false;

        private Cookies _cookies;
        public Cookies AppCookies 
        { 
            get
            {
                return _cookies;
            }
        }

        private LocalStorage _localStorage;

        public LocalStorage AppLocalStorage
        {
            get
            {
                return _localStorage;
            }
        }


        private IJSRuntime _jscontext;
        public IJSRuntime JSContext
        {
            get
            {
                return _jscontext;
            }
        }

        private IConfiguration _webhost;
        public IConfiguration WebHost
        {
            get
            {
                return _webhost;
            }
        }

        private IAppSettings _settings;
        public IAppSettings Settings 
        { 
            get
            {
                return _settings;
            }
            set
            {
                _settings = value;
            }
        }

        public TemplateAppController(IConfiguration webhost, IJSRuntime jscontext)
        {
            _webhost = webhost;
            _jscontext = jscontext;
            _cookies = new Cookies(jscontext);
            _localStorage = new LocalStorage(jscontext); 
           // GetSession();                       
           
        }

        public async Task<bool> CheckSession()
        {
            bool ret = false;

            await GetSession();

            if (UserInfo != null)
            {               
               ret = true;               
            }

            return ret;
        }

        public async Task ClearSession()
        {            
            await _cookies.ClearUserInfo();
        }

        public async Task<OperationStatus> CreateSession(UserAuthenticated user)
        {
            OperationStatus ret = new OperationStatus(true);
            
            DateTime expires = DateTime.Now.AddMinutes(int.Parse(_settings.SessionTimeOut)); 
            await _cookies.SetUserInfo(user,expires);

            return ret;
        }

        public async Task ReplaceUserInfo(UserAuthenticated user)
        {
            await _cookies.ClearUserInfo();
            DateTime expires = DateTime.Now.AddMinutes(int.Parse(_settings.SessionTimeOut));
           await _cookies.SetUserInfo(UserInfo, expires);
            
        }

        public async Task GetSession()
        {
            UserAuthenticated ticket = null;

            ticket = await _cookies.GetUserInfo();

            UserInfo = ticket; 
        }

        public async Task<bool> IsAuthenticated()
        {
            bool ret = _IsAuthenticated;

            if (!_IsAuthenticated)
            {
                ret = await CheckSession();
            }

            return ret;
        }

        public async Task<OperationStatus> Login(IAuthGatewayManager apigateway, UserLogin user)
        {
            OperationStatus ret = new OperationStatus(true);
            UserAuthenticated usr = null;

            user.SessionTimeOut = _settings.SessionTimeOut;
            usr = await ((AuthGateway)apigateway).Login(user);

            if (usr != null)
            {
                DateTime expires = DateTime.Now.AddMinutes(int.Parse(_settings.SessionTimeOut));
                await _localStorage.SetUserPermissions(usr.Permissions,usr.Token);

                usr.Permissions = null;
                await this.CreateSession(usr);
                
                ret.Returns = usr; 
            }
            else
            {                               
                ret.InnerExceptions = apigateway.GetDefaultError(ref ret.Error);                               
                ret.Status = false; 
            }

            return ret;
        }
     
        public async Task Logout()
        {            
            await _cookies.ClearUserInfo();
            await _cookies.ClearUserPermissions(); 
            await _cookies.ClearAllCookies();
            await _localStorage.ClearUserPermissions();
        }

        public  PermissionsState CheckPermissions(UserAuthenticated user,
            string objectcode, bool allownone)
        {
            PermissionsState ret = new PermissionsState(false,false,false);
            
            List<UserPermissions> permissions = user.Permissions;           

            ret =
                Utilities.GetPermissionsState(permissions, objectcode, allownone);         

            return ret;
        }

        public async Task<List<UserPermissions>> GetUserPermissions(string token)
        {
            List<UserPermissions> ret = new List<UserPermissions>();

            ret = await _localStorage.GetUserPermissions(token); 

            return ret;

        }
    }


}