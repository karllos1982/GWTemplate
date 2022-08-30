using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GW.Core.Common;
using GW.Core.Helpers;
using GW.Membership.Models;
using Newtonsoft.Json;
using GW.Core.APIGateway;
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

        public MembershipGateway()
        {

        }

        public void Init(HttpClient http, string baseurl, string token)
        {
            User = new UserGateway();
            Role = new RoleGateway();
            Session = new SessionGateway();
            DataLog = new DataLogGateway();

            User.InitializeAPI(http, baseurl + "/membership/user/", token);
            User.IsAuthenticated = true;

            Role.InitializeAPI(http, baseurl + "/membership/role/", token);
            Role.IsAuthenticated = true;

            Session.InitializeAPI(http, baseurl + "/membership/session/", token);
            Session.IsAuthenticated = true;

            DataLog.InitializeAPI(http, baseurl + "/membership/datalog/", token);
            DataLog.IsAuthenticated = true;
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

        public async Task<List<UserSearchResult>> Search(UserParam data)
        {
            List<UserSearchResult> ret = null;
            
            ret = await PostAsJSON<List<UserSearchResult>>("search", 
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

        public async Task<UserModel> Get(string id)
        {
            UserModel ret = null;

            object[] param = new object[1];       
            param[0] = new DefaultGetParam(id) ;
            
            ret = await GetAsJSON<UserModel>("get", param);
           
            return ret;
        }

        public async Task<UserModel> Set(UserModel data)
        {
            UserModel ret = null;

            ret = await PostAsJSON<UserModel>("set", JsonConvert.SerializeObject(data),null);            

            return ret;
        }

        public async Task<UserModel> CreateNewUser(NewUser data)
        {
            UserModel ret = null;
            
            ret = await this.PostAsJSON<UserModel>("createnewuser", 
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

    public class RoleGateway : APIGatewayManagerAsync
    {
     
        public RoleGateway()
        {

        }

        public async Task<List<RoleSearchResult>> Search(RoleParam param)
        {
            List<RoleSearchResult> ret = null;

            ret = await PostAsJSON<List<RoleSearchResult>>("search",
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

        public async Task<RoleModel> Get(string id)
        {
            RoleModel ret = null;

            object[] param = new object[1];
            param[0] = new DefaultGetParam(id);

            ret = await GetAsJSON<RoleModel>("get", param);

            return ret;
        }

        public async Task<RoleModel> Set(RoleModel data)
        {
            RoleModel ret = null;

            ret = await PostAsJSON<RoleModel>("set", JsonConvert.SerializeObject(data), null);

            return ret;
        }


    }

    public class SessionGateway : APIGatewayManagerAsync
    {

        public SessionGateway()
        {

        }

        public async Task<List<SessionSearchResult>> Search(SessionParam data)
        {
            List<SessionSearchResult> ret = null;

            ret = await PostAsJSON<List<SessionSearchResult>>("search",
                JsonConvert.SerializeObject(data), null);


            return ret;
        }


        public async Task<List<SessionList>> List(SessionParam data)
        {
            List<SessionList> ret = null;

            ret = await PostAsJSON<List<SessionList>>("list",
                JsonConvert.SerializeObject(data), null);

            return ret;
        }


        public async Task<SessionModel> Get(string id)
        {
            SessionModel ret = null;

            object[] param = new object[1];
            param[0] = new DefaultGetParam(id);

            ret = await GetAsJSON<SessionModel>("get", param);

            return ret;
        }
            

    }

    public class DataLogGateway : APIGatewayManagerAsync
    {

        public DataLogGateway()
        {

        }

        public async Task<List<DataLogSearchResult>> Search(DataLogParam data)
        {
            List<DataLogSearchResult> ret = null;

            ret = await PostAsJSON<List<DataLogSearchResult>>("search",
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

        public async Task<DataLogModel> Get(string id)
        {
            DataLogModel ret = null;

            object[] param = new object[1];
            param[0] = new DefaultGetParam(id);

            ret = await GetAsJSON<DataLogModel>("get", param);

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
}
