using Microsoft.AspNetCore.Mvc;
using GW.Membership.Models;
using GW.Core.Common;
using Template.API;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
//using Template.Models;

namespace Template.Controllers
{

    [Route("membership/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : APIControllerBase
    {

        public UserController(IAppSettingsManager<TemplateSettings> param, 
             IWebHostEnvironment hostingEnvironment)
        {
            AppConfigs = param;
            AppConfigs.EnvironmentSettings = hostingEnvironment;
            AppConfigs.LoadSettings();
            ObjectCode = "SYSUSER";           
        }


        [HttpPost]
        [Route("search")]        
        public object Search(UserParam param)
        {
            Init(PERMISSION_CHECK_ENUM.READ, false);

            if (IsAllowed)
            {
                List<UserSearchResult> list = null;

                list = manager.Membership.UserUnit.Search(param);

                SetReturn(list);
                FinalizeManager();
            }


            return ret;
        }

        [HttpPost]
        [Route("list")]
        public object List(UserParam param)
        {
            Init(PERMISSION_CHECK_ENUM.READ, true);

            if (IsAllowed)
            {
                List<UserList> list = null;

                list = manager.Membership.UserUnit.List(param);

                SetReturn(list);
                FinalizeManager();
            }

            return ret;
        }

        [HttpGet]
        [Route("get")]        
        public object Get(string id)
        {
            Init(PERMISSION_CHECK_ENUM.READ, false);

            if (IsAllowed)
            {
                var username = User.Identity.Name;

                UserModel obj = null;

                obj = manager.Membership.UserUnit.Get(Int64.Parse(id));

                if (this.GetDefaultStatus().Status)
                {
                    if (obj != null)
                    {
                        obj.ProfileImageURL =
                             AppConfigs.Settings.SiteURL + "auth/GetUserImageProfile?file=user_anonymous.png";

                        if (obj.ProfileImage != null)
                        {
                            if (obj.ProfileImage != "")
                            {
                                obj.ProfileImageURL =
                                    AppConfigs.Settings.SiteURL + "auth/GetUserImageProfile?file=" + obj.ProfileImage;

                            }
                        }

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
        public object Set(UserModel data)
        {
            Init(PERMISSION_CHECK_ENUM.SAVE, false);

            if (IsAllowed)
            {
                var userid = User.Identity.Name;

                opsts = manager.Membership.UserUnit.Set(data, userid);

                if (opsts.Status)
                {
                    ret = opsts.Returns;
                }
                else
                {
                    Response.StatusCode = 500;
                    opsts.InnerExceptions.Insert(0, new InnerException("Error", opsts.Error.Message));
                    ret = opsts.InnerExceptions;
                }

                FinalizeManager();
            }

            return ret;        
        }

        [HttpPost]
        [Route("createnewuser")]        
        public object CreateNewUser(NewUser data)
        {
            Init(PERMISSION_CHECK_ENUM.SAVE, false);

            if (IsAllowed)
            {
                var userid = User.Identity.Name;
                opsts = manager.Membership.CreateNewUser(data, true, userid);

                if (opsts.Status)
                {
                    ret = opsts.Returns;

                }
                else
                {
                    opsts.InnerExceptions.Insert(0, new InnerException("Error", opsts.Error.Message));
                    ret = opsts.InnerExceptions;

                    Response.StatusCode = 500;

                }
                FinalizeManager();
            }

            return ret;
        }

        [HttpPost]
        [Route("addtorole")]        
        public object AddToRole(UserRolesModel data)
        {
            Init(PERMISSION_CHECK_ENUM.SAVE, false);

            if (IsAllowed)
            {
                opsts = manager.Membership
                .UserUnit.AddRoleToUser(data.UserID,data.RoleID, true);

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
        [Route("removefromrole")]        
        public object RemoveFromRole(UserRolesModel data)
        {
            Init(PERMISSION_CHECK_ENUM.DELETE, false);

            if (IsAllowed)
            {
                opsts = manager.Membership
                .UserUnit.RemoveRoleFromUser(data.UserID, data.RoleID, true);

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
        [Route("changestate")]        
        public object ChangeState(UserChangeState data)
        {
            Init(PERMISSION_CHECK_ENUM.SAVE, false);

            if (IsAllowed)
            {
                opsts = manager.Membership
                .UserUnit.ChangeState(data);

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
     

    }
}
