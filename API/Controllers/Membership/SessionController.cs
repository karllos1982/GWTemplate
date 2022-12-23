using Microsoft.AspNetCore.Mvc;
using GW.Membership.Models;
using GW.Common;
using Template.API;
using Microsoft.AspNetCore.Authorization;
using GW.Core;
using GW.Membership.Contracts.Domain;
//using Template.Models;

namespace Template.Controllers
{
    [Route("membership/[controller]")]
    [ApiController]
    [Authorize]
    public class SessionController : APIControllerBase
    {

        public SessionController(IMembershipManager membership,
                IContextBuilder contextbuilder)
        {
            Context = membership.Context;
            contextbuilder.BuilderContext(Context);
            this.Membership = membership;
            ObjectCode = "SYSSESSION"; 
        }

        [HttpPost]
        [Route("search")]        
        public async Task<object> Search(SessionLogParam param)
        {

            Init(PERMISSION_CHECK_ENUM.READ, false);

            if (IsAllowed)
            {
                List<SessionLogResult> list = null;

                list = await Membership.SessionLog.Search(param);

                SetReturn(list);
                FinalizeManager();
            }

            return ret;
        }

        [HttpPost]
        [Route("list")]        
        public async Task<object> List(SessionLogParam param)
        {
            Init(PERMISSION_CHECK_ENUM.READ, true);

            if (IsAllowed)
            {
                List<SessionLogList> list = null;

                list = await Membership.SessionLog.List(param);

                SetReturn(list);
                FinalizeManager();
            }

            return ret;
        }

        [HttpGet]
        [Route("get")]        
        public async Task<object> Get(string id)
        {
            Init( PERMISSION_CHECK_ENUM.READ,false);

            if (IsAllowed)
            {
                SessionLogResult obj = null;

                obj = await Membership.SessionLog.Get( 
                    new SessionLogParam() { pSessionID = Int64.Parse(id) });

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
      

    }
}
