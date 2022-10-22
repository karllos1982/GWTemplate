using Newtonsoft.Json;
using Microsoft.JSInterop;
using GW.Membership.Models;
using GW.Core.Common;

namespace Template.ServerCode
{

    public class Cookies
    {
        private IJSRuntime _jsruntime;
        ServerFunctions utils = new ServerFunctions();

        public Cookies(IJSRuntime jsruntime)
        {
            _jsruntime = jsruntime;

        }

        public async Task SetUserInfo(UserAuthenticated user, DateTime expires)
        {
            string val = JsonConvert.SerializeObject(user);

            await utils.SetCookie(_jsruntime, "USERINFO", val, expires.ToString(@"yyyy-MM-dd HH:mm:ss"));

        }

        public async Task<UserAuthenticated> GetUserInfo()
        {
            UserAuthenticated ret = null;

            string aux = await utils.GetCookie(_jsruntime, "USERINFO");

            if (aux != null)
            {
                ret = JsonConvert.DeserializeObject<UserAuthenticated>(aux);
            }

            return ret; 
        }

        public async Task ClearUserInfo()
        {            
            await utils.SetCookie(_jsruntime, "USERINFO", null, DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss"));
        }

        //


        public async Task SetUserPermissions(List<UserPermissions> permissions, DateTime expires)
        {
            string val = JsonConvert.SerializeObject(permissions);

            await utils.SetCookie(_jsruntime, "USERPERMISSIONS", val, expires.ToString(@"yyyy-MM-dd HH:mm:ss"));

        }

        public async Task<List<UserPermissions>> GetUserPermissions()
        {
            List<UserPermissions> ret = null;

            string aux = await utils.GetCookie(_jsruntime, "USERPERMISSIONS");

            if (aux != null)
            {
                ret = JsonConvert.DeserializeObject<List<UserPermissions>>(aux);
            }

            return ret;
        }

        public async Task ClearUserPermissions()
        {
            await utils.SetCookie(_jsruntime, "USERPERMISSIONS", null, DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss"));
        }



        //

        public async Task SetUserContext(UserConext value, DateTime expires)
        {
            string val = JsonConvert.SerializeObject(value);                        

            await utils.SetCookie(_jsruntime, "USERCONTEXT", val, expires.ToString(@"yyyy-MM-dd HH:mm:ss"));

        }

        public async Task<UserConext> GetUserContext()
        {
            UserConext ret = null;

            string aux = await utils.GetCookie(_jsruntime, "USERCONTEXT");

            if (aux != null)
            {
                ret = JsonConvert.DeserializeObject<UserConext>(aux);
            }

            return ret;
        }

        public async Task ClearUserContext()
        {
            await utils.SetCookie(_jsruntime, "USERCONTEXT", null, DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss"));
        }
      

    }

}