using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GW.Membership.Models;
using Template.Core.Manager;
using GW.Core.Common;
using GW.Core.Helpers;
using Template.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Template.Controllers
{
    [Route("membership/[controller]")]
    [ApiController]
    [Authorize]
    public class PermissionController : APIControllerBase
    {

        public PermissionController(IAppSettingsManager<TemplateSettings> param,
            IWebHostEnvironment hostingEnvironment)
        {
            AppConfigs = param;
            AppConfigs.EnvironmentSettings = hostingEnvironment;
            AppConfigs.LoadSettings();
            ObjectCode = "SYSPERMISSION";
        }

        [HttpPost]
        [Route("search")]
        [Authorize]
        public object Search(PermissionParam param)
        {
            Init(PERMISSION_CHECK_ENUM.READ, false);

            if (IsAllowed)
            {
                List<PermissionSearchResult> list = null;

                list = manager.Membership.PermissionUnit.Search(param);

                if (this.GetDefaultStatus().Status)
                {
                    ret = list;
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

        [HttpPost]
        [Route("list")]
        [Authorize]
        public object List(PermissionParam param)
        {
           
            Init(PERMISSION_CHECK_ENUM.READ, true);

            if (IsAllowed)
            {
                List<PermissionList> list = null;

                list = manager.Membership.PermissionUnit.List(param);

                if (this.GetDefaultStatus().Status)
                {
                    ret = list;
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
        [Route("get")]
        [Authorize]
        public object Get(string id)
        {
            Init(PERMISSION_CHECK_ENUM.READ, false);

            if (IsAllowed)
            {
                PermissionModel obj = null;

                obj = manager.Membership.PermissionUnit.Get(Int64.Parse(id));

                if (this.GetDefaultStatus().Status)
                {
                    ret = obj;
                }
                else
                {
                    Response.StatusCode = 500;
                    ret = GetInnerExceptions("Nenhum registro encontrado.");
                }

                FinalizeManager();

            }
            return ret;
        }

        [HttpPost]
        [Route("set")]
        [Authorize]
        public object Set(PermissionModel data)
        {
            Init(PERMISSION_CHECK_ENUM.SAVE, false);

            if (IsAllowed)
            {
                var userid = User.Identity.Name;

                opsts = manager.Membership.PermissionUnit.Set(data, userid);

                if (opsts.Status)
                {
                    ret = opsts.Returns;
                }
                else
                {
                    Response.StatusCode = 500;
                    ret = opsts.InnerExceptions;
                }
                FinalizeManager();

            }

            return ret;
        }

        [HttpPost]
        [Route("delete")]
        [Authorize]
        public object Delete(PermissionModel data)
        {
            Init(PERMISSION_CHECK_ENUM.DELETE, false);

            if (IsAllowed)
            {
                var userid = User.Identity.Name;

                opsts = manager.Membership.PermissionUnit.Delete(data, userid);

                if (opsts.Status)
                {
                    ret = opsts.Returns;
                }
                else
                {
                    Response.StatusCode = 500;
                    ret = opsts.InnerExceptions;
                }
                FinalizeManager();

            }

            return ret;
        }

    }
}
