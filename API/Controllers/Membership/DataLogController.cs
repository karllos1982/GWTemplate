using Microsoft.AspNetCore.Mvc;
using GW.Membership.Models;
using GW.Common;
using Template.API;
using Microsoft.AspNetCore.Authorization;
using GW.Core;
using GW.Membership.Contracts.Domain;


namespace Template.Controllers
{
    [Route("membership/[controller]")]
    [ApiController]
    [Authorize]
    public class DataLogController : APIControllerBase
    {
        public DataLogController(IMembershipManager membership,
                IContextBuilder contextbuilder)
        {
            Context = membership.Context;
            contextbuilder.BuilderContext(Context);
            this.Membership = membership;
            ObjectCode = "SYSDATALOG";

        }

        [HttpPost]
        [Route("search")]        
        public async Task<object> Search(DataLogParam param)
        {
            Init(PERMISSION_CHECK_ENUM.READ, false);

            if (IsAllowed)
            {
                List<DataLogResult> list = null;

                list = await Membership.DataLog.Search(param);

                SetReturn(list);
                FinalizeManager();
            }

            return ret;
        }

        [HttpPost]
        [Route("list")]        
        public async Task<object> List(DataLogParam param)
        {
            Init(PERMISSION_CHECK_ENUM.READ, true);

            if (IsAllowed)
            {
                List<DataLogList> list = null;

                list = await Membership.DataLog.List(param);

                SetReturn(list);
                FinalizeManager();
            }

            return ret;
        }

        [HttpGet]
        [Route("get")]        
        public async Task<object> Get(string id)
        {
            Init(PERMISSION_CHECK_ENUM.READ, false);

            if (IsAllowed)
            {
                DataLogResult obj = null;

                obj = await Membership.DataLog.Get(new DataLogParam() { pDataLogID = Int64.Parse(id) });

                if (this.GetDefaultStatus().Status)
                {
                    if (obj != null)
                    {
                        ret = obj;
                    }
                    else
                    {
                        Response.StatusCode = 500;
                        ret = GetInnerExceptions("Nenhum registro encontrado.");
                    }
                }
                else
                {
                    Response.StatusCode = 500;
                    ret = GetInnerExceptions(this.GetDefaultStatus().Error.Message);
                }
                FinalizeManager();
            }


            return ret;
        }

        [HttpGet]
        [Route("gettimeline")]       
        public async Task<object> GetTimeLine(string id)
        {
            Init(PERMISSION_CHECK_ENUM.READ, false);

            if (IsAllowed)
            {
                List<DataLogTimelineModel> list = null;

                list = await Membership.DataLog.GetTimeLine(Int64.Parse(id));

                SetReturn(list);
                FinalizeManager();
            }

            return ret;
        }


        [HttpGet]
        [Route("gettablelist")]        
        public async Task<object> GetTableList()
        {
            Init(PERMISSION_CHECK_ENUM.READ, false);

            if (IsAllowed)
            {
                List<TabelasValueModel> list = null;

                list =  await Membership.DataLog.GetTableList();

                SetReturn(list);
                FinalizeManager();
            }

            return ret;
        }
        


    }
}
