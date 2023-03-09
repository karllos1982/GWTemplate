using Newtonsoft.Json;
using Microsoft.JSInterop;
using GW.Membership.Models;
using GW.Common;

namespace Template.ServerCode
{
    public class LocalStorage
    {

        private IJSRuntime _jsruntime;
        ServerFunctions utils = new ServerFunctions();

        public LocalStorage(IJSRuntime jsruntime)
        {
            _jsruntime = jsruntime;

        }

        public async Task SetUserPermissions(List<UserPermissions> permissions, string token)
        {

            // adicionando uma camada de segurança; criar um registro de permissão com o token

            permissions.Add(new UserPermissions() { PermissionID = 0, ObjectCode = token });

            string val = JsonConvert.SerializeObject(permissions);            

            await utils.SaveLocalData(_jsruntime, "USERPERMISSIONS", val);

        }

        public async Task<List<UserPermissions>> GetUserPermissions(string token)
        {
            List<UserPermissions> ret = null;

            string aux = await utils.ReadLocalData(_jsruntime, "USERPERMISSIONS");

            if (aux != null)
            {
                ret = JsonConvert.DeserializeObject<List<UserPermissions>>(aux);

                //verificando o token...
                var obj = ret.Where(x=>x.PermissionID == 0).FirstOrDefault();
                if (obj.ObjectCode==token)
                {
                    ret.Remove(obj); 
                }
                else
                {
                    ret = new List<UserPermissions>(); // caso o token não coincida, retornar a lista de permissões vazia
                }
            }

            return ret;
        }

        public async Task ClearUserPermissions()
        {
            await utils.ClearLocalData(_jsruntime, "USERPERMISSIONS");
        }

    }

}
