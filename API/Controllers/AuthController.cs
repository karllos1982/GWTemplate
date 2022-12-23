﻿using Microsoft.AspNetCore.Mvc;
using GW.Membership.Models;
using GW.Common;
using GW.Helpers;
using Template.API;
using Template.Core.Manager;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using GW.Core;
using GW.Membership.Contracts.Domain;
using GW.ApplicationHelpers;
using Template.Core;
using GW.Membership.Data;

//using Template.Models;


namespace Template.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : APIControllerBase
    {      

        public AuthController(IMembershipManager membership,
                IContextBuilder contextbuilder, MailManager mail  )
        {
            Context = membership.Context;
            contextbuilder.BuilderContext(Context);
            this.Membership = membership;
            this.MailCenter = mail;            

        }


        [HttpGet]
        [Route("index")]
        [AllowAnonymous]
        public object Index()
        {
            Init();
            UserEntry ret = new UserEntry();
            List<InnerException> list = new List<InnerException>();

            opsts = new OperationStatus(true);

            if (opsts.Status)
            {
               
                ret.UserName = "Teste";
            }
            else
            {
                Response.StatusCode = 500;
            }

            return ret;
        }



        [HttpPost]
        [Route("registraruser")]
        [Authorize]
        public async Task<object> RegistrarUser(NewUser data)
        {
            Init();

            data.RoleID = 3; // id do perfil
           
            UserEntry obj = await Membership.CreateNewUser(data, true, null); 

            if (Context.ExecutionStatus.Status)
            {
                ret = obj;
                
            }
            else
            {
                ret = Context.ExecutionStatus.InnerExceptions;
                
                Response.StatusCode = 500;

            }

            FinalizeManager();

            return ret;
        }


        [HttpPost]
        [Route("sendemailconfirmation")]
        [AllowAnonymous]
        public object SendEmailConfirmation(EmailConfirmation data) //EmailConfirmation
        {
            Init();
            
            Validations val = new Validations();

            if (val.ValidateEmail(data.Email))
            {
                data.Code = Utilities.GenerateCode(6);

                opsts =
                  ((TemplateMailCenter)MailCenter).SendEmailConfirmationCode(data.Email, data.UserName, data.Code);
                               
                if (opsts.Status)
                {
                    ret = data;
                }
                else
                {
                    Response.StatusCode = 500;
                    ret = GetInnerExceptions(opsts.Error.Message);
                }
            }
            else
            {
                Response.StatusCode = 500;
                 ret = GetInnerExceptions("O e-mail informado é inválido."); 
            }

            FinalizeManager();

            return ret;
        }


        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<object> Login(UserLogin data)
        {
            Init();
           
            data.Password = Utilities.ConvertFromBase64(data.Password);
            data.Password = MD5.BuildMD5(data.Password);
            UserResult userM = await Membership.Login(data);

            if (Context.ExecutionStatus.Status)
            {                
                string permissions_content =  JsonConvert.SerializeObject(userM.Permissions);
                
                AuthToken token = TokenService.GenerateToken(userM.UserID.ToString(),
                    userM.Roles[0].RoleName, permissions_content, int.Parse(data.SessionTimeOut));
                           
                UserAuthenticated userA = new UserAuthenticated();
                userA.UserID = userM.UserID.ToString();
                userA.UserName = userM.UserName;
                userA.Email = userM.Email;
                userA.RoleName = userM.Roles[0].RoleName;
                userA.InstanceName = userM.Instances[0].InstanceName;
                userA.Token = token.TokenValue;
                userA.Expires = token.ExpiresDate;
                userA.Permissions = userM.Permissions;                 

                UpdateUserLogin uplogin = new UpdateUserLogin()
                {
                    UserID = userM.UserID,
                    LastLoginDate = DateTime.Now,
                    AuthToken = token.TokenValue,
                    AuthTokenExpires = token.ExpiresDate
                };
            
               await Membership.RegisterLoginState(data, uplogin);
                
                switch (userA.RoleName)
                {
                    case "Admin":
                    {
                            userA.HomeURL = "superadmin/home";
                            break;
                    }

                    case "SuperAdmin":
                    {
                        userA.HomeURL = "superadmin/home";
                        break;
                    }
                  

                }

                string filename = "IMG_" + userM.UserID.ToString() + ".png";
                userA.ProfileImageURL =
                    Context.Settings.SiteURL+ "auth/GetUserImageProfile?file=" + filename;
                
                ret = userA;

            }
            else
            {
                Response.StatusCode = 500;
                ret = GetInnerExceptions(Context.ExecutionStatus.Error.Message);
            }

            ((DapperContext)Context).Commit();
            ((DapperContext)Context).Dispose();

            return ret;
        }
   

        [HttpPost]
        [Route("recoverypassword")]
        [AllowAnonymous]
        public async Task<object> RecoveryPassword(ChangeUserPassword data)
        {
            Init();

            opsts = await Membership.GetTemporaryPassword(data);

            if (opsts.Status)
            {
                ((TemplateMailCenter)MailCenter).SendTemporaryPassword(data.Email, "Usuário", opsts.Returns.ToString());                 
            }
            else
            {
                Response.StatusCode = 500;
                ret = GetInnerExceptions(opsts.Error.Message);
            }

            FinalizeManager();

            return ret;
        }


        [HttpPost]
        [Route("requestactiveaccountcode")]
        [AllowAnonymous]
        public async Task<object> RequestActiveAccountCode(ActiveUserAccount data)
        {
            Init();

            opsts = await Membership.GetActiveAccountCode(data); 

            if (opsts.Status)
            {
                ((TemplateMailCenter)MailCenter).SendActiveAccountCode(data.Email, "Usuário", opsts.Returns.ToString());
            }
            else
            {
                Response.StatusCode = 500;
                ret = GetInnerExceptions(opsts.Error.Message);
            }

            FinalizeManager();

            return ret;
        }

        [HttpPost]
        [Route("activeaccount")]
        [AllowAnonymous]
        public async Task<object> ActiveAccount(ActiveUserAccount data)
        {
            Init();

            opsts = await Membership.User.ActiveUserAccount(data); 

            if (opsts.Status)
            {
            }
            else
            {
                Response.StatusCode = 500;
                ret = GetInnerExceptions(opsts.Error.Message);
            }
            FinalizeManager();

            return ret;
        }


        [HttpPost]
        [Route("requestchangepasswordcode")]
        [Authorize]
        public async Task<object> RequestChangePasswordCode(ChangeUserPassword data)
        {
            Init();
            
            opsts = await Membership.GetChangePasswordCode(data);

            if (opsts.Status)
            {
                ((TemplateMailCenter)MailCenter).SendChangePassowordCode(data.Email,"Usuário", 
                    opsts.Returns.ToString());
            }
            else
            {
                Response.StatusCode = 500;
                ret = GetInnerExceptions(opsts.Error.Message);
            }
            FinalizeManager();

            return ret;
        }

        [HttpPost]
        [Route("changepassword")]
        [Authorize]
        public async Task<object> ChangePassword(ChangeUserPassword data)
        {
            Init();        

            opsts = await Membership.User.ChangeUserPassword(data); 

            if (opsts.Status)
            {                
            }
            else
            {
                Response.StatusCode = 500;
                ret = GetInnerExceptions(opsts.Error.Message);
            }
            FinalizeManager();

            return ret;
        }

        [HttpPost]
        [Route("changeuserimageprofile")]
        [Authorize]
        public async Task<object> ChangeUserImageProfile()
        {
            Init();

            FileStream fs;
            Stream body = Request.Body; 
            
            string userid = User.Identity.Name;
            ChangeUserImage data = new ChangeUserImage();
            data.UserID = Int64.Parse(userid);
            data.FileName = "IMG_" + userid + ".png";

            opsts = await Membership.ChangeUserProfileImage(data);

            
            string path = Context.Settings.ProfileImageDir + "\\" + data.FileName;
            
            if (opsts.Status)
            {
                using (fs = new FileStream(path,FileMode.OpenOrCreate ))
                {
                   await body.CopyToAsync(fs);
                }

                ret = data; 
            }
            else
            {
                Response.StatusCode = 500;
                ret = GetInnerExceptions(opsts.Error.Message);
            }
            FinalizeManager();

            return ret;
        }

        [HttpGet]
        [Route("getuserimageprofile")]       
        public FileStreamResult GetUserImageProfile(string file)
        {
            Init();

            string path = Membership.Context.Settings.ProfileImageDir + "\\" + file;

            if (!System.IO.File.Exists(path))
            {
                path = Context.Settings.ProfileImageDir + "\\user_anonymous.png";                
            }

            FileStream str = new FileStream(path,FileMode.Open);

            FileStreamResult result = new FileStreamResult(str, "application/octet-stream");

            return result;
        }


        [HttpGet]
        [Route("logout")]
        [Authorize]
        public async Task<object> Logout()
        {
            Init();

            string userid = User.Identity.Name;
            await Membership.Logout(Int64.Parse(userid));

            FinalizeManager();

            return ret;
        }

    }
}
