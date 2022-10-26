using Microsoft.AspNetCore.Mvc;
using GW.Membership.Models;
using GW.Core.Common;
using GW.Core.Helpers;
using Template.API;
using Microsoft.AspNetCore.Authorization;
//using Template.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Template.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DataCacheController : APIControllerBase
    {
        public DataCacheController(IAppSettingsManager<TemplateSettings> param,
           IWebHostEnvironment hostingEnvironment,IMemoryCache _cache)
        {
            AppConfigs = param;
            AppConfigs.EnvironmentSettings = hostingEnvironment;
            AppConfigs.LoadSettings();
            memorycache = _cache;
        }

        [HttpGet]
        [Route("listtipoperacao")]
        public object ListTipoOperacao()
        {
            Init();
                        
            List<TipoOperacaoValueModel> list = null;

            list = memorycache.Get<List<TipoOperacaoValueModel>>("TIPOOPERACAO");

            if (list == null)
            {
                list = new List<TipoOperacaoValueModel>()
                    {
                        new TipoOperacaoValueModel(){ Value="I", Text="Inserção"},
                        new TipoOperacaoValueModel(){ Value="U", Text="Edição"},
                        new TipoOperacaoValueModel(){ Value="D", Text="Exclusão"}
                    };
            
                memorycache.Set("TIPOOPERACAO", list, this.GetMemoryCacheOptionsByHour(2)); 
            }

            ret = list;

            FinalizeManager();

            return ret;
        }

        [HttpGet]
        [Route("listtabelas")]
        public object ListTabelas()
        {
            Init();

            List<TabelasValueModel> list = null;

            list = memorycache.Get<List<TabelasValueModel>>("TABELAS");

            if (list == null)
            {
                list = manager.Membership.DataLogUnit.GetTableList();               
                
                memorycache.Set("TABELAS", list, this.GetMemoryCacheOptionsByHour(2));

            }

            ret = list;

            FinalizeManager();

            return ret;
        }

        [HttpGet]
        [Route("listroles")]
        public object ListRoles()
        {
            Init();

            List<RoleList> list = null;

            list = memorycache.Get<List<RoleList>>("ROLES");

            if (list == null)
            {
                list = manager.Membership.RoleUnit.List(new RoleParam());

                memorycache.Set("ROLES", list, this.GetMemoryCacheOptionsByHour(2));

            }

            ret = list;

            FinalizeManager();

            return ret;
        }


    }
}
