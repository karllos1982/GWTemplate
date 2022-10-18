using Microsoft.AspNetCore.Mvc;
using GW.Membership.Models;
using GW.Core.Common;
using GW.Core.Helpers;
using Template.API;
using Template.Core.Manager;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

//using Template.Models;


namespace Template.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : APIControllerBase
    {      

        public AuthController(IAppSettingsManager<TemplateSettings> param,
             IWebHostEnvironment hostingEnvironment)
        {
            AppConfigs = param;
            AppConfigs.EnvironmentSettings = hostingEnvironment;
            AppConfigs.LoadSettings();
           
        }


        [HttpGet]
        [Route("index")]
        [AllowAnonymous]
        public object Index()
        {
            Init();
            UserModel ret = new UserModel();
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
        public object RegistrarUser(NewUser data)
        {
            Init();

            data.RoleID = 3; // id do perfil
           
            opsts = manager.Membership.CreateNewUser(data, true, null); 

            if (opsts.Status)
            {               
                ret = opsts.Returns;
                
            }
            else
            {
                ret = opsts.InnerExceptions;
                
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
                  mailCenter.SendEmailConfirmationCode(data.Email, data.UserName, data.Code);
                               
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
        public object Login(UserLogin data)
        {
            Init();
           
            data.Password = Utilities.ConvertFromBase64(data.Password);
            data.Password = MD5.BuildMD5(data.Password);
            opsts = manager.Membership.Login(data);

            if (opsts.Status)
            {
                UserModel userM = (UserModel)opsts.Returns;
                string permissions_content = JsonConvert.SerializeObject(userM.Permissions);
                
                AuthToken token = TokenService.GenerateToken(userM.UserID.ToString(), 
                    userM.Role.RoleName, permissions_content, int.Parse(data.SessionTimeOut));

                UserAuthenticated userA = new UserAuthenticated();
                userA.UserID = userM.UserID.ToString();
                userA.UserName = userM.UserName;
                userA.Email = userM.Email;
                userA.RoleName = userM.Role.RoleName;
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

                manager = new TemplateManager(apiConfigs);
                manager.Membership.RegisterLoginState(data, uplogin);
                
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
                    AppConfigs.Settings.SiteURL+ "auth/GetUserImageProfile?file=" + filename;
                
                ret = userA;

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
        [Route("recoverypassword")]
        [AllowAnonymous]
        public object RecoveryPassword(ChangeUserPassword data)
        {
            Init();

            opsts = manager.Membership.GetTemporaryPassword(data);

            if (opsts.Status)
            {
                mailCenter.SendTemporaryPassword(data.Email, "Usuário", opsts.Returns.ToString());                 
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
        public object RequestActiveAccountCode(ActiveUserAccount data)
        {
            Init();

            opsts = manager.Membership.GetActiveAccountCode(data); 

            if (opsts.Status)
            {
                mailCenter.SendActiveAccountCode(data.Email, "Usuário", opsts.Returns.ToString());
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
        public object ActiveAccount(ActiveUserAccount data)
        {
            Init();

            opsts = manager.Membership.UserUnit.ActiveUserAccount(data); 

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
        public object RequestChangePasswordCode(ChangeUserPassword data)
        {
            Init();
            
            opsts = manager.Membership.GetChangePasswordCode(data);

            if (opsts.Status)
            {
                mailCenter.SendChangePassowordCode(data.Email,"Usuário", 
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
        public object ChangePassword(ChangeUserPassword data)
        {
            Init();        

            opsts = manager.Membership.UserUnit.ChangeUserPassword(data); 

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
        public object ChangeUserImageProfile()
        {
            Init();

            FileStream fs;
            Stream body = Request.Body; 
            
            string userid = User.Identity.Name;
            ChangeUserImage data = new ChangeUserImage();
            data.UserID = Int64.Parse(userid);
            data.FileName = "IMG_" + userid + ".png";

            opsts = manager.Membership.ChangeUserProfileImage(data);

            
            string path = AppConfigs.Settings.ProfileImageDir + "\\" + data.FileName;
            
            if (opsts.Status)
            {
                using (fs = new FileStream(path,FileMode.OpenOrCreate ))
                {
                    body.CopyToAsync(fs);
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

            string path = AppConfigs.Settings.ProfileImageDir + "\\" + file;

            if (!System.IO.File.Exists(path))
            {
                path =  AppConfigs.Settings.ProfileImageDir + "\\user_anonymous.png";                
            }

            FileStream str = new FileStream(path,FileMode.Open);

            FileStreamResult result = new FileStreamResult(str, "application/octet-stream");

            return result;
        }


        [HttpGet]
        [Route("logout")]
        [Authorize]
        public object Logout()
        {
            Init();

            string userid = User.Identity.Name;
            manager.Membership.Logout(Int64.Parse(userid));

            FinalizeManager();

            return ret;
        }

    }
}
