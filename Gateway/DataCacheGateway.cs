using GW.Core.Common;
using GW.Membership.Models;
using System.Linq; 
using Newtonsoft.Json;
using GW.Core.APIGateway;
//using Template.Models;

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

             
    }

}
