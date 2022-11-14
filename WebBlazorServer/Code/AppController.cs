using Microsoft.JSInterop;
using Newtonsoft.Json;
using GW.Core.Common;
using GW.Core.Helpers;
using GW.Membership.Models;
using Template.Gateway;
using WebBlazorServer.Pages.SuperAdmin;


namespace Template.ServerCode
{
    public interface IAppControllerAsync<T> where T : UserAuthenticated
    {
        Task<bool> IsAuthenticated();

        PermissionsState CheckPermissions(UserAuthenticated user,
            string objectcode, bool allownone);

        T UserInfo { get; set; }

        Task <OperationStatus> Login(IAuthGatewayManager apigateway,  UserLogin user);

        Task Logout();

        Task<OperationStatus> CreateSession(UserAuthenticated user);

        Task GetSession();

        Task ClearSession();

        Task<bool> CheckSession();

        Task ReplaceUserInfo(UserAuthenticated user);

        Task<List<UserPermissions>> GetUserPermissions(); 
    }

    public class TemplateSettings: AppSettings
    {

        public string SiteURL { get; set; }

        public string ServiceURL { get; set; }

        public string NomeSistema { get; set; }

        public string SessionTimeOut { get; set; }

        public string FileContentMenu { get; set; }

        public List<MenuObject> ContentMenu { get; set; }
         
    }

    public class MenuObject
    {
        public string ID { get; set; }

        public string Role { get; set; }

        public string Title { get; set; }

        public string NavigationURL { get; set; }

        public string ClassIcon { get; set; }

        public string ClassStatus { get; set; }

        public string Description { get; set; }

        public List<MenuObject> Childs { get; set; }

    }



    public class TemplateManagerSettings : IAppSettingsManager<TemplateSettings>
    {
        public TemplateSettings Settings { get; set; }
        
        public object EnvironmentSettings  { get; set; }

        private IWebHostEnvironment _env;

       
        public TemplateManagerSettings(IWebHostEnvironment webhost)
        {
            _env = (IWebHostEnvironment)webhost;
           // LoadSettings(null);
        }


        public void LoadSettings(HttpClient client)
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
                filename = Path.Combine(dir , filename);
            }


            if (File.Exists(filename))
            {

                jsontxt = File.ReadAllText(filename);

                if (jsontxt.Length > 0)
                {
                    this.Settings = JsonConvert.DeserializeObject<TemplateSettings>(jsontxt);

                    //filename = dir + "/" + this.Settings.FileContentMenu;

                    //jsontxt = File.ReadAllText(filename, System.Text.Encoding.UTF8);

                    //this.Settings.ContentMenu = JsonConvert.DeserializeObject<List<MenuObject>>(jsontxt);

                }
            }
        }
    }

    public class UserInfo: UserAuthenticated
    {

    }
   
    public class UserConext
    {
        public string IdClinicaSelecionada { get; set; }

        public string NomeClinicaSelecionada { get; set; }


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

        private TemplateSettings _settings;
        public TemplateSettings Settings 
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
                await _cookies.SetUserPermissions(usr.Permissions, expires);

                await this.CreateSession(usr);
                usr.Permissions = null;

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

        public async Task<List<UserPermissions>> GetUserPermissions()
        {
            List<UserPermissions> ret = new List<UserPermissions>();

            ret = await _cookies.GetUserPermissions(); 

            return ret;

        }
    }

    public interface IMenuItemActive
    {
        void ActiveItemMenu(string itemname);

        string GetActiveItemMenu();
    }

    public class MenuItemActive : IMenuItemActive
    {
        private string _itemname = "Admin/Home";

        public void ActiveItemMenu(string itemname)
        {
            _itemname = itemname;
        }

        public string GetActiveItemMenu()
        {
            return _itemname;
        }
    }

}