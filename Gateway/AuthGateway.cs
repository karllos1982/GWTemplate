using GW.Common;
using GW.Membership.Models;
using Newtonsoft.Json;
using GW.ApplicationHelpers;


namespace Template.Gateway
{

    public interface IAuthGatewayManager
    {
        void Init(HttpClient http, string baseurl, string token);

        List<InnerException> GetDefaultError(ref Exception defaulterror);

        Task<List<LocalizationTextResult>> ListLocalizationTexts();

    }

    public class AuthGateway: APIGatewayManagerAsync, IAuthGatewayManager
    {
        
      
        public AuthGateway()
        {
              
        }

        public void Init(HttpClient http, string baseurl, string token)
        {
            this.InitializeAPI(http,baseurl + "/auth/",token);
            
        }

        public List<InnerException> GetDefaultError(ref Exception defaulterror)
        {
            return base.GetInnerExceptions(ref defaulterror); 
        }

        public async Task<List<LocalizationTextResult>> ListLocalizationTexts()
        {
            List<LocalizationTextResult> ret = null;

            ret = 
                await this.GetAsJSON<List<LocalizationTextResult>>("listlocalizationtexts", null);
                        

            return ret;
        }

        public async Task<UserEntry> Registrar(NewUser data)
        {
            UserEntry ret = null;
            
            ret = await this.PostAsJSON<UserEntry>("registraruser", JsonConvert.SerializeObject(data),null);
                       
            return ret;
        }

        public async Task EnviarEmailConfirmacao(EmailConfirmation data)
        {
            object ret = null;
                                
            ret = await this.PostAsJSON<object>("sendemailconfirmation", JsonConvert.SerializeObject(data), null);            
            
        }

        public async Task<UserAuthenticated> Login(UserLogin data)
        {
            UserAuthenticated ret = null;

            string jsoncontent = JsonConvert.SerializeObject(data);

            ret = await this.PostAsJSON<UserAuthenticated>("login",jsoncontent , null);

            return ret;
        }
       
        public async Task RecoveryPassword(string email)
        {
            object ret = null;

            ChangeUserPassword param = new ChangeUserPassword();
            param.Email = email; 
                       
            ret = await this.PostAsJSON<object>("recoverypassword", JsonConvert.SerializeObject(param), null);
            
        }

        public async Task RequestActiveAccountCode(string email)
        {
            object ret = null;

            ActiveUserAccount param = new ActiveUserAccount();
            param.Email = email;            

            ret = await this.PostAsJSON<object>("requestactiveaccountcode", JsonConvert.SerializeObject(param), null);
            
        }

        public async Task ActiveAccount(ActiveUserAccount param)
        {
            object ret = null;            

            ret = await this.PostAsJSON<object>("activeaccount", JsonConvert.SerializeObject(param), null);
            
        }

        public async Task RequestChangePasswordCode(string email)
        {
            object ret = null;
            this.IsAuthenticated = true; 

            ChangeUserPassword param = new ChangeUserPassword();
            param.Email = email;            

            ret = await this.PostAsJSON<object>("requestchangepasswordcode", JsonConvert.SerializeObject(param), null);
            
        }

        public async Task ChangePassword(ChangeUserPassword param)
        {
            object ret = null;
            this.IsAuthenticated = true;           

            ret = await this.PostAsJSON<object>("changepassword", JsonConvert.SerializeObject(param), null);
            
        }

        public async Task<ChangeUserImage> ChangeUserImage(byte[] data)
        {
            ChangeUserImage ret = null;
            this.IsAuthenticated = true;
           
            ret = await this.PostAsStream<ChangeUserImage>("changeuserimageprofile",data, null);

            return ret;
        }

        public async Task Logout()
        {            
           
            this.IsAuthenticated = true;

            object ret = await this.GetAsJSON<object>("changepassword", null);            
            
        }

    }
}
