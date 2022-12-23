using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GW.Common;
using GW.Helpers;
using GW.Membership.Models;
using Newtonsoft.Json;
using GW.ApplicationHelpers;
using Newtonsoft.Json.Linq;

namespace Template.Gateway
{
    public interface IMembershipGatewayManager
    {
        void Init(HttpClient http, string baseurl, string token);

        //List<InnerException> GetDefaultError(ref Exception defaulterror);
    }

    public class MembershipGateway: APIGatewayManagerAsync, IMembershipGatewayManager
    {
        public UserGateway User = null;

        public RoleGateway Role = null;

        public SessionGateway Session = null;

        public DataLogGateway DataLog = null;

        public ObjectPermissionGateway ObjectPermission = null;

        public InstanceGateway Instance = null; 

        public PermissionGateway Permission = null;

        public MembershipGateway()
        {

        }

        public void Init(HttpClient http, string baseurl, string token)
        {
            User = new UserGateway();
            Role = new RoleGateway();
            Session = new SessionGateway();
            DataLog = new DataLogGateway();
            ObjectPermission = new ObjectPermissionGateway();
            Permission = new PermissionGateway();
            Instance = new InstanceGateway();

            User.InitializeAPI(http, baseurl + "/membership/user/", token);
            User.IsAuthenticated = true;

            Instance.InitializeAPI(http, baseurl + "/membership/instance/", token);
            Instance.IsAuthenticated = true;

            Role.InitializeAPI(http, baseurl + "/membership/role/", token);
            Role.IsAuthenticated = true;

            Session.InitializeAPI(http, baseurl + "/membership/session/", token);
            Session.IsAuthenticated = true;

            DataLog.InitializeAPI(http, baseurl + "/membership/datalog/", token);
            DataLog.IsAuthenticated = true;

            ObjectPermission.InitializeAPI(http, baseurl + "/membership/objectpermission/", token);
            ObjectPermission.IsAuthenticated = true;

            Permission.InitializeAPI(http, baseurl + "/membership/permission/", token);
            Permission.IsAuthenticated = true;

        }

        //public List<InnerException> GetDefaultError(ref Exception defaulterror)
        //{
        //    return base.GetInnerExceptions(ref defaulterror);
        //}

    }

    //

    public class UserGateway : APIGatewayManagerAsync
    {
        
        public UserGateway()
        {

        }

        public async Task<List<UserResult>> Search(UserParam data)
        {
            List<UserResult> ret = null;
            
            ret = await PostAsJSON<List<UserResult>>("search", 
                JsonConvert.SerializeObject(data),null);
            

            return ret;
        }

        public async Task<List<UserList>> List(UserParam data)
        {
            List<UserList> ret = null;
            
            ret = await PostAsJSON<List<UserList>>("list",
                JsonConvert.SerializeObject(data),null);
           
            return ret;
        }

        public async Task<UserResult> Get(string id)
        {
            UserResult ret = null;

            object[] param = new object[1];       
            param[0] = new DefaultGetParam(id) ;
            
            ret = await GetAsJSON<UserResult>("get", param);
           
            return ret;
        }

        public async Task<UserEntry> Set(UserEntry data)
        {
            UserEntry ret = null;

            ret = await PostAsJSON<UserEntry>("set", JsonConvert.SerializeObject(data),null);            

            return ret;
        }

        public async Task<UserEntry> CreateNewUser(NewUser data)
        {
            UserEntry ret = null;
            
            ret = await this.PostAsJSON<UserEntry>("createnewuser", 
                JsonConvert.SerializeObject(data), null);
                       
            return ret;
        }

        public async Task<UserRolesEntry> AddToRole(string userid, string roleid)
        {
            UserRolesEntry ret = null;
            UserRolesEntry data = new UserRolesEntry()
            {
                UserID = Int64.Parse(userid),
                RoleID = Int64.Parse(roleid)
            }
            ;
            ret = await this.PostAsJSON<UserRolesEntry>("addtorole",
                JsonConvert.SerializeObject(data), null);

            return ret;
        }

        public async Task<UserRolesEntry> RemoveFromRole(string userid, string roleid)
        {
            UserRolesEntry ret = null;
            UserRolesEntry data = new UserRolesEntry()
            {
                UserID = Int64.Parse(userid),
                RoleID = Int64.Parse(roleid)
            }
            ;
            ret = await this.PostAsJSON<UserRolesEntry>("removefromrole",
                JsonConvert.SerializeObject(data), null);

            return ret;
        }

        public async Task<bool> AlterInstance(UserInstancesResult data)
        {
            bool ret = false;
            
            ret = await this.PostAsJSON<bool>("alterinstance",
                JsonConvert.SerializeObject(data), null);

            return ret;
        }

        public async Task<bool> AlterRole(UserRolesResult data)
        {
            bool ret = false;

            ret = await this.PostAsJSON<bool>("alterrole",
                JsonConvert.SerializeObject(data), null);

            return ret;
        }

        public async Task ChangeState(UserChangeState data)
        {
            object ret = null;

            ret = await this.PostAsJSON<object>("changestate", 
                JsonConvert.SerializeObject(data), null);          
        }
       
    }

    public class InstanceGateway : APIGatewayManagerAsync
    {

        public InstanceGateway()
        {

        }

        public async Task<List<InstanceResult>> Search(InstanceParam param)
        {
            List<InstanceResult> ret = null;

            ret = await PostAsJSON<List<InstanceResult>>("search",
                JsonConvert.SerializeObject(param), null);


            return ret;
        }

        public async Task<List<InstanceList>> List(InstanceParam param)
        {
            List<InstanceList> ret = null;

            ret = await PostAsJSON<List<InstanceList>>("list",
                JsonConvert.SerializeObject(param), null);

            return ret;
        }

        public async Task<InstanceResult> Get(string id)
        {
            InstanceResult ret = null;

            object[] param = new object[1];
            param[0] = new DefaultGetParam(id);

            ret = await GetAsJSON<InstanceResult>("get", param);

            return ret;
        }

        public async Task<InstanceEntry> Set(InstanceEntry data)
        {
            InstanceEntry ret = null;

            ret = await PostAsJSON<InstanceEntry>("set", JsonConvert.SerializeObject(data), null);

            return ret;
        }


    }


    public class RoleGateway : APIGatewayManagerAsync
    {
     
        public RoleGateway()
        {

        }

        public async Task<List<RoleResult>> Search(RoleParam param)
        {
            List<RoleResult> ret = null;

            ret = await PostAsJSON<List<RoleResult>>("search",
                JsonConvert.SerializeObject(param), null);


            return ret;
        }

        public async Task<List<RoleList>> List(RoleParam param)
        {
            List<RoleList> ret = null;

            ret = await PostAsJSON<List<RoleList>>("list",
                JsonConvert.SerializeObject(param), null);

            return ret;
        }

        public async Task<RoleResult> Get(string id)
        {
            RoleResult ret = null;

            object[] param = new object[1];
            param[0] = new DefaultGetParam(id);

            ret = await GetAsJSON<RoleResult>("get", param);

            return ret;
        }

        public async Task<RoleEntry> Set(RoleEntry data)
        {
            RoleEntry ret = null;

            ret = await PostAsJSON<RoleEntry>("set", JsonConvert.SerializeObject(data), null);

            return ret;
        }


    }

    public class SessionGateway : APIGatewayManagerAsync
    {

        public SessionGateway()
        {

        }

        public async Task<List<SessionLogResult>> Search(SessionLogParam data)
        {
            List<SessionLogResult> ret = null;

            ret = await PostAsJSON<List<SessionLogResult>>("search",
                JsonConvert.SerializeObject(data), null);


            return ret;
        }


        public async Task<List<SessionLogList>> List(SessionLogParam data)
        {
            List<SessionLogList> ret = null;

            ret = await PostAsJSON<List<SessionLogList>>("list",
                JsonConvert.SerializeObject(data), null);

            return ret;
        }


        public async Task<SessionLogResult> Get(string id)
        {
            SessionLogResult ret = null;

            object[] param = new object[1];
            param[0] = new DefaultGetParam(id);

            ret = await GetAsJSON<SessionLogResult>("get", param);

            return ret;
        }
            

    }

    public class DataLogGateway : APIGatewayManagerAsync
    {

        public DataLogGateway()
        {

        }

        public async Task<List<DataLogResult>> Search(DataLogParam data)
        {
            List<DataLogResult> ret = null;

            ret = await PostAsJSON<List<DataLogResult>>("search",
                JsonConvert.SerializeObject(data), null);


            return ret;
        }

        public async Task<List<DataLogList>> List(DataLogParam data)
        {
            List<DataLogList> ret = null;

            ret = await PostAsJSON<List<DataLogList>>("list",
                JsonConvert.SerializeObject(data), null);

            return ret;
        }

        public async Task<DataLogResult> Get(string id)
        {
            DataLogResult ret = null;

            object[] param = new object[1];
            param[0] = new DefaultGetParam(id);

            ret = await GetAsJSON<DataLogResult>("get", param);

            return ret;
        }

        public async Task<List<TabelasValueModel>> GetTableList()
        {
            List<TabelasValueModel> ret = null;
            
            ret = await GetAsJSON<List<TabelasValueModel>>("get", null);

            return ret;
        }

        public async Task<List<DataLogTimelineModel>> GetTimeLine(string recordID)
        {
            List<DataLogTimelineModel> ret = null;
            RequestStatus reqstatus = null;

            object[] param = new object[1];
            param[0] = new DefaultGetParam(recordID);

            ret = await GetAsJSON<List<DataLogTimelineModel>>("gettimeline", param);          

            return ret;
        }

       
        public List<DataLogItem> GetDataLogItems(string logcontent)
        {
           
            List<DataLogItem> list = new List<DataLogItem>();
            DataLogItem obj;

            dynamic aux = JsonConvert.DeserializeObject<object>(logcontent);

            JProperty jprop;
            JObject jobj = (JObject)aux;
            JToken jtk;

            foreach (JToken p in jobj.Children())
            {
                jprop = (JProperty)p;
                jtk = (JToken)jprop.Value;

                obj = new DataLogItem();
                obj.ItemName = jprop.Name;
                obj.ItemValue = jtk.ToString();
                obj.IsDifferent = false;
                list.Add(obj);
            }            

            return list;
        }

        public void GetDataLogDiff(ref List<DataLogItem> old, ref List<DataLogItem> current)
        {
            DataLogItem objlog;

            foreach (DataLogItem l in current)
            {
                objlog = old.Where(x => x.ItemName == l.ItemName).FirstOrDefault();

                if (objlog.ItemValue != l.ItemValue)
                {
                    l.IsDifferent = true;
                    objlog.IsDifferent = true;
                }
            }
        }

    }

    public class ObjectPermissionGateway : APIGatewayManagerAsync
    {

        public ObjectPermissionGateway()
        {

        }

        public async Task<List<ObjectPermissionResult>> Search(ObjectPermissionParam param)
        {
            List<ObjectPermissionResult> ret = null;

            ret = await PostAsJSON<List<ObjectPermissionResult>>("search",
                JsonConvert.SerializeObject(param), null);


            return ret;
        }

        public async Task<List<ObjectPermissionList>> List(ObjectPermissionParam param)
        {
            List<ObjectPermissionList> ret = null;

            ret = await PostAsJSON<List<ObjectPermissionList>>("list",
                JsonConvert.SerializeObject(param), null);

            return ret;
        }

        public async Task<ObjectPermissionResult> Get(string id)
        {
            ObjectPermissionResult ret = null;

            object[] param = new object[1];
            param[0] = new DefaultGetParam(id);

            ret = await GetAsJSON<ObjectPermissionResult>("get", param);

            return ret;
        }

        public async Task<ObjectPermissionEntry> Set(ObjectPermissionEntry data)
        {
            ObjectPermissionEntry ret = null;

            ret = await PostAsJSON<ObjectPermissionEntry>("set", JsonConvert.SerializeObject(data), null);

            return ret;
        }


    }

    public class PermissionGateway : APIGatewayManagerAsync
    {

        public PermissionGateway()
        {

        }

        public async Task<List<PermissionResult>> Search(PermissionParam param)
        {
            List<PermissionResult> ret = null;

            ret = await PostAsJSON<List<PermissionResult>>("search",
                JsonConvert.SerializeObject(param), null);


            return ret;
        }

        public async Task<List<PermissionList>> List(PermissionParam param)
        {
            List<PermissionList> ret = null;

            ret = await PostAsJSON<List<PermissionList>>("list",
                JsonConvert.SerializeObject(param), null);

            return ret;
        }

        public async Task<PermissionResult> Get(string id)
        {
            PermissionResult ret = null;

            object[] param = new object[1];
            param[0] = new DefaultGetParam(id);

            ret = await GetAsJSON<PermissionResult>("get", param);

            return ret;
        }

        public async Task<PermissionEntry> Set(PermissionEntry data)
        {
            PermissionEntry ret = null;

            ret = await PostAsJSON<PermissionEntry>("set", JsonConvert.SerializeObject(data), null);

            return ret;
        }

        public async Task<PermissionEntry> Delete(PermissionEntry data)
        {
            PermissionEntry ret = null;

            ret = await PostAsJSON<PermissionEntry>("delete", JsonConvert.SerializeObject(data), null);

            return ret;
        }
    }
}
