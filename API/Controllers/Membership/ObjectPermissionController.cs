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
    public class ObjectPermissionController : APIControllerBase
    {

        public ObjectPermissionController(IMembershipManager membership,
                IContextBuilder contextbuilder)
        {
            Context = membership.Context;
            contextbuilder.BuilderContext(Context);
            this.Membership = membership;
            ObjectCode = "SYSOBJECTPERMISSION";
        }

        [HttpPost]
        [Route("search")]
        [Authorize]
        public async Task<object> Search(ObjectPermissionParam param)
        {
            Init(PERMISSION_CHECK_ENUM.READ, false);

            if (IsAllowed)
            {
                List<ObjectPermissionResult> list = null;

                list = await Membership.ObjectPermission.Search(param);

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
        public async Task<object> List(ObjectPermissionParam param)
        {
           
            Init(PERMISSION_CHECK_ENUM.READ, true);

            if (IsAllowed)
            {
                List<ObjectPermissionList> list = null;

                list = await Membership.ObjectPermission.List(param);

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
                ObjectPermissionResult obj = null;

                obj = await Membership.ObjectPermission.Get(new ObjectPermissionParam()
                {
                    pObjectPermissionID = Int64.Parse(id)
                });

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
        public async Task<object> Set(ObjectPermissionEntry data)
        {
            Init(PERMISSION_CHECK_ENUM.SAVE, false);

            if (IsAllowed)
            {
                ObjectPermissionEntry obj = await Membership.ObjectPermission.Set(data, this.UserID);

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

    }
}
