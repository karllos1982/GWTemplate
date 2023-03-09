using Microsoft.JSInterop;
using Newtonsoft.Json;
using GW.Common;
using GW.Helpers;
using GW.Membership.Models;
using Template.Gateway;
using Microsoft.AspNetCore.Mvc.Formatters;

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

        Task<List<UserPermissions>> GetUserPermissions(string token); 
    }

    public interface IAppSettings
    {
        string SiteURL { get; set; }

        string ServiceURL { get; set; }

        string NomeSistema { get; set; }

        string SessionTimeOut { get; set; }

        string DefaultLanguage { get; set; }

        string FileContentMenu { get; set; }

        List<MenuObject> ContentMenu { get; set; }
        

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
   

    public class UserInfo: UserAuthenticated
    {

    }
   
    public class UserConext
    {
        public string Id { get; set; }

        public string Name { get; set; }


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