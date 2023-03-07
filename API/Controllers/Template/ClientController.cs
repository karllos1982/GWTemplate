using Microsoft.AspNetCore.Mvc;
using GW.Common;
using Template.API;
using Microsoft.AspNetCore.Authorization;
using GW.Core;
using Template.Models;
using Template.Contracts.Domain;

namespace Template.Controllers
{
    [Route("template/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientController : APIControllerBase
    {

        public ClientController(ITemplateManager manager,
                IContextBuilder contextbuilder)
        {
            Context = manager.Context;
            contextbuilder.BuilderContext(Context);
            this.Manager = manager;
            ObjectCode = "CLIENT";
        }

        [HttpPost]
        [Route("search")]
        [Authorize]
        public async Task<object> Search(ClientParam param)
        {
            Init(PERMISSION_CHECK_ENUM.READ, false);

            if (IsAllowed)
            {
                List<ClientResult> list = null;

                list = await Manager.Client.Search(param);

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
        public async Task<object> List(ClientParam param)
        {
           
            Init(PERMISSION_CHECK_ENUM.READ, true);

            if (IsAllowed)
            {
                List<ClientList> list = null;

                list = await Manager.Client.List(param);

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
                ClientResult obj = null;

                obj =  await Manager.Client.Get(new ClientParam() { pClientID = Int64.Parse(id) });

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
        public async Task<object> Set(ClientEntry data)
        {
            Init(PERMISSION_CHECK_ENUM.SAVE, false);

            if (IsAllowed)
            {                

                ClientEntry obj = await Manager.Client.Set(data, this.UserID);

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
        [Route("contactsentryvalidation")]
        [Authorize]
        public object ContactEntryValidation(ClientContactsEntry data)
        {
            Init(PERMISSION_CHECK_ENUM.SAVE, false);

            if (IsAllowed)
            {              
                               
                ClientContactsEntry obj = Manager.Client.ContactEntryValidation(data);

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
