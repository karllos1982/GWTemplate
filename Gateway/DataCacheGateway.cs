using GW.Membership.Models;
using GW.ApplicationHelpers;


namespace Template.Gateway
{
    public interface IDataCacheGatewayManager
    {
        void Init(HttpClient http, string baseurl, string token);
       
     }

    public class DataCacheGateway : APIGatewayManagerAsync, IDataCacheGatewayManager
    {


        public DataCacheGateway()
        {

        }

        public void Init(HttpClient http, string baseurl, string token)
        {
            this.InitializeAPI(http, baseurl + "/datacache/", token);

        }      

        public async Task<List<TipoOperacaoValueModel>> ListTipoOperacao()
        {
            List<TipoOperacaoValueModel> ret = null;

            ret = await this.GetAsJSON<List<TipoOperacaoValueModel>>("listtipoperacao", 
                 null);

            return ret;
        }

       
        public async Task<List<TabelasValueModel>> ListTabelas()
        {
            List<TabelasValueModel> ret = null;

            ret = await this.GetAsJSON<List<TabelasValueModel>>("listtabelas",
                 null);

            return ret;
        }

        public async Task<List<RoleList>> ListRoles()
        {
            List<RoleList> ret = null;

            ret = await this.GetAsJSON<List<RoleList>>("listroles", null);                

            return ret;
        }

        public async Task<List<LocalizationTextList>> ListLanguages()
        {
            List<LocalizationTextList> ret = null;

            ret = await this.GetAsJSON<List<LocalizationTextList>>("listlangs", null);

            return ret;
        }

        public async Task<List<LocalizationTextResult>> ListLocalizationTexts()
        {
            List<LocalizationTextResult> ret = null;

            ret = await this.GetAsJSON<List<LocalizationTextResult>>("listlocalizationtexts", null);

            return ret;
        }

    }

}
