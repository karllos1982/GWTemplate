using Microsoft.AspNetCore.Mvc;
using GW.Membership.Models;
using GW.Common;
using Template.API;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GW.Core;
using GW.Membership.Contracts.Domain;
//using Template.Models;

namespace Template.Controllers
{

    [Route("membership/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : APIControllerBase
    {

        public UserController(IMembershipManager membership,
                IContextBuilder contextbuilder)
        {
            Context = membership.Context;         
            contextbuilder.BuilderContext(Context);
            this.Membership = membership;
            ObjectCode = "SYSUSER";           
        }


        [HttpPost]
        [Route("search")]        
        public async Task<object> Search(UserParam param)
        {
            Init(PERMISSION_CHECK_ENUM.READ, false);

            if (IsAllowed)
            {
                List<UserResult> list = null;

                list = await Membership.User.Search(param);

                SetReturn(list);
                FinalizeManager();
            }


            return ret;
        }

        [HttpPost]
        [Route("list")]
        public async Task<object> List(UserParam param)
        {
            Init(PERMISSION_CHECK_ENUM.READ, true);

            if (IsAllowed)
            {
                List<UserList> list = null;

                list = await Membership.User.List(param);

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
                var username = User.Identity.Name;

                UserResult obj = null;

                obj = await Membership.User.Get( new UserParam() { pUserID = Int64.Parse(id) });

                if (this.GetDefaultStatus().Status)
                {
                    if (obj != null)
                    {
                        obj.ProfileImageURL =
                                    Context.Settings.SiteURL + "auth/GetUserImageProfile?file=" + obj.ProfileImage;                      

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

        [HttpPost]
        [Route("set")]        
        public async Task<object> Set(UserEntry data)
        {
            Init(PERMISSION_CHECK_ENUM.SAVE, false);

            if (IsAllowed)
            {
                
                UserEntry obj = await Membership.User.Set(data, this.UserID);

                if (this.GetDefaultStatus().Status)
                {
                    ret = obj;
                }
                else
                {
                    Response.StatusCode = 500;
                    this.GetDefaultStatus().InnerExceptions.Insert(0, new InnerException("Error", this.GetDefaultStatus().Error.Message));
                    ret = this.GetDefaultStatus().InnerExceptions;
                }

                FinalizeManager();
            }

            return ret;        
        }

        [HttpPost]
        [Route("createnewuser")]        
        public async Task<object> CreateNewUser(NewUser data)
        {
            Init(PERMISSION_CHECK_ENUM.SAVE, false);

            if (IsAllowed)
            {
                var userid = User.Identity.Name;
                UserEntry obj = await Membership.CreateNewUser(data, true, userid);

                if (this.GetDefaultStatus().Status)
                {
                    ret = obj;

                }
                else
                {
                    this.GetDefaultStatus().InnerExceptions
                        .Insert(0, new InnerException("Error", this.GetDefaultStatus().Error.Message));
                    ret = this.GetDefaultStatus().InnerExceptions;

                    Response.StatusCode = 500;

                }
                FinalizeManager();
            }

            return ret;
        }

        [HttpPost]
        [Route("addtorole")]        
        public async Task<object> AddToRole(UserRolesEntry data)
        {
            Init(PERMISSION_CHECK_ENUM.SAVE, false);

            if (IsAllowed)
            {
                UserRolesEntry obj = await Membership
                    .User.AddRoleToUser(data.UserID,data.RoleID);

                if (this.GetDefaultStatus().Status)
                {
                    ret = obj;
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
        [Route("removefromrole")]        
        public async Task<object> RemoveFromRole(UserRolesEntry data)
        {
            Init(PERMISSION_CHECK_ENUM.DELETE, false);

            if (IsAllowed)
            {
                UserRolesEntry obj = await Membership
                    .User.RemoveRoleFromUser(data.UserID, data.RoleID);

                if (this.GetDefaultStatus().Status)
                {
                    ret = obj;
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
        [Route("changestate")]        
        public async Task<object> ChangeState(UserChangeState data)
        {
            Init(PERMISSION_CHECK_ENUM.SAVE, false);

            if (IsAllowed)
            {
                opsts = await Membership
                    .User.ChangeState(data);

                if (opsts.Status)
                {
                    ret = opsts.Returns;
                }
                else
                {
                    Response.StatusCode = 500;
                    ret = GetInnerExceptions(opsts.Error.Message);
                }

                FinalizeManager();
            }

            return ret;
        }

        [HttpPost]
        [Route("alterinstance")]
        public async Task<object> AlterInstance(UserInstancesResult data)
        {
            Init(PERMISSION_CHECK_ENUM.SAVE, false);
            ret = false;

            if (IsAllowed)
            {
                await Membership
                   .User.AlterInstance(data.UserInstanceID, data.InstanceID);

                if (!this.GetDefaultStatus().Status)
                {               
                    Response.StatusCode = 500;
                    ret = GetInnerExceptions(this.GetDefaultStatus().Error.Message);
                }
                else
                {
                    ret = true;
                }

                FinalizeManager();
            }

            return ret;
        }

        [HttpPost]
        [Route("alterrole")]
        public async Task<object> AlterRole(UserRolesResult data)
        {
            Init(PERMISSION_CHECK_ENUM.SAVE, false);
            ret = false;

            if (IsAllowed)
            {
                await Membership
                   .User.AlterRole(data.UserRoleID, data.RoleID);

                if (!this.GetDefaultStatus().Status)
                {
                    Response.StatusCode = 500;
                    ret = GetInnerExceptions(this.GetDefaultStatus().Error.Message);
                }
                else
                {
                    ret = true;
                }

                FinalizeManager();
            }

            return ret;
        }
    }
}
