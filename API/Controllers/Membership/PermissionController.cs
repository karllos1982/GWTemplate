using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GW.Membership.Models;
using Template.Core.Manager;
using GW.Common;
using GW.Helpers;
using Template.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Core.Data;
using GW.Core;
using GW.Membership.Contracts.Domain;

namespace Template.Controllers
{
    [Route("membership/[controller]")]
    [ApiController]
    [Authorize]
    public class PermissionController : APIControllerBase
    {

        public PermissionController(IMembershipManager membership,
                IContextBuilder contextbuilder)
        {
            Context = membership.Context;
            contextbuilder.BuilderContext(Context);
            this.Membership = membership;
            ObjectCode = "SYSPERMISSION";
        }

        [HttpPost]
        [Route("search")]
        [Authorize]
        public async Task<object> Search(PermissionParam param)
        {
            Init(PERMISSION_CHECK_ENUM.READ, false);

            if (IsAllowed)
            {
                List<PermissionResult> list = null;

                list = await Membership.Permission.Search(param);

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
        public async Task<object> List(PermissionParam param)
        {
           
            Init(PERMISSION_CHECK_ENUM.READ, true);

            if (IsAllowed)
            {
                List<PermissionList> list = null;

                list = await Membership.Permission.List(param);

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
        public async Task<object> Get(string id)
        {
            Init(PERMISSION_CHECK_ENUM.READ, false);

            if (IsAllowed)
            {
                PermissionResult obj = null;

                obj = await Membership.Permission.Get( new PermissionParam() 
                    { pPermissionID = Int64.Parse(id) });

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
        public async Task<object> Set(PermissionEntry data)
        {
            Init(PERMISSION_CHECK_ENUM.SAVE, false);

            if (IsAllowed)
            {
                PermissionEntry obj = await Membership.Permission.Set(data, this.UserID);

                if (this.GetDefaultStatus().Status)
                {
                    ret = obj; 
                }
                else
                {
                    Response.StatusCode = 500;
                    ret = this.GetDefaultStatus().InnerExceptions;
                }
                FinalizeManager();

            }

            return ret;
        }

        [HttpPost]
        [Route("delete")]
        [Authorize]
        public async Task<object> Delete(PermissionEntry data)
        {
            Init(PERMISSION_CHECK_ENUM.DELETE, false);

            if (IsAllowed)
            {
                var userid = User.Identity.Name;

                PermissionEntry obj = 
                    await Membership.Permission.Delete(data, userid);

                if (this.GetDefaultStatus().Status)
                {
                    ret = this.GetDefaultStatus().Returns;
                }
                else
                {
                    Response.StatusCode = 500;
                    ret = this.GetDefaultStatus().InnerExceptions;
                }
                FinalizeManager();

            }

            return ret;
        }

    }
}
