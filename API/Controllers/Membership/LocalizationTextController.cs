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
    public class LocalizationTextController : APIControllerBase
    {

        public LocalizationTextController(IMembershipManager membership,
                IContextBuilder contextbuilder)
        {
            Context = membership.Context;
            contextbuilder.BuilderContext(Context);
            this.Membership = membership;
            ObjectCode = "SYSLOCALIZATIONTEXT";
        }

        [HttpPost]
        [Route("search")]
        [Authorize]
        public async Task<object> Search(LocalizationTextParam param)
        {
            Init(PERMISSION_CHECK_ENUM.READ, false);

            if (IsAllowed)
            {
                List<LocalizationTextResult> list = null;

                list = await Membership.LocalizationText.Search(param);

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
        public async Task<object> List(LocalizationTextParam param)
        {
           
            Init(PERMISSION_CHECK_ENUM.READ, true);

            if (IsAllowed)
            {
                List<LocalizationTextList> list = null;

                list = await Membership.LocalizationText.List(param);

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
                LocalizationTextResult obj = null;

                obj =  await Membership.LocalizationText.Get(new LocalizationTextParam() { pLocalizationTextID = Int64.Parse(id) });

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
        public async Task<object> Set(LocalizationTextEntry data)
        {
            Init(PERMISSION_CHECK_ENUM.SAVE, false);

            if (IsAllowed)
            {
                var userid = User.Identity.Name;

                LocalizationTextEntry obj = await Membership.LocalizationText.Set(data, userid);

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
