using Microsoft.AspNetCore.Mvc;
using GW.Membership.Models;
using GW.Core.Common;
using Template.API;
using Microsoft.AspNetCore.Authorization;
//using Template.Models;

namespace Template.Controllers
{
    [Route("membership/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class SessionController : APIControllerBase
    {

        public SessionController(IAppSettingsManager<TemplateSettings> param,
        IWebHostEnvironment hostingEnvironment)
        {
            AppConfigs = param;
            AppConfigs.EnvironmentSettings = hostingEnvironment;
            AppConfigs.LoadSettings();

        }

        [HttpPost]
        [Route("search")]        
        public object Search(SessionParam param)
        {
            Init();

            List<SessionSearchResult> list = null;

            list = manager.Membership.SessionUnit.Search(param);

            SetReturn(list);
            FinalizeManager();

            return ret;
        }

        [HttpPost]
        [Route("list")]        
        public object List(SessionParam param)
        {
            Init();

            List<SessionList> list = null;

            list = manager.Membership.SessionUnit.List(param);

            SetReturn(list);
            FinalizeManager();

            return ret;
        }

        [HttpGet]
        [Route("get")]        
        public object Get(string id)
        {
            Init();

            SessionModel obj = null;

            obj = manager.Membership.SessionUnit.Get(Int64.Parse(id));

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
            return ret;
        }
      

    }
}
