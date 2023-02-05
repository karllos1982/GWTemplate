using Microsoft.AspNetCore.Mvc;
using GW.Membership.Models;
using GW.Common;
using GW.Helpers;
using Template.API;
using Microsoft.AspNetCore.Authorization;
//using Template.Models;
using Microsoft.Extensions.Caching.Memory;
using GW.Membership.Contracts.Domain;
using Core.Data;
using GW.Core;
using GW;

namespace Template.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DataCacheController : APIControllerBase
    {
        public DataCacheController(IMembershipManager membership,
           IWebHostEnvironment hostingEnvironment,
           IMemoryCache _cache, IContextBuilder contextbuilder)
        {
            Context = membership.Context;
            contextbuilder.BuilderContext(Context);
            this.Membership = membership;
            memorycache = _cache;
            ObjectCode = "SYSROLES";
        }

        [HttpGet]
        [Route("listtipoperacao")]
        public object ListTipoOperacao()
        {
            Init(PERMISSION_CHECK_ENUM.READ, true);

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
        public async Task<object> ListTabelas()
        {
            Init(PERMISSION_CHECK_ENUM.READ, true);

            List<TabelasValueModel> list = null;

            list = memorycache.Get<List<TabelasValueModel>>("TABELAS");

            if (list == null)
            {
                list = await Membership.DataLog.GetTableList();               
                
                memorycache.Set("TABELAS", list, this.GetMemoryCacheOptionsByHour(2));

            }

            ret = list;

            FinalizeManager();

            return ret;
        }

        [HttpGet]
        [Route("listroles")]
        public async Task<object> ListRoles()
        {
            Init(PERMISSION_CHECK_ENUM.READ, true);

            List<RoleList> list = null;

            list = memorycache.Get<List<RoleList>>("ROLES");

            if (list == null)
            {
                list = await Membership.Role.List(new RoleParam());

                memorycache.Set("ROLES", list, this.GetMemoryCacheOptionsByHour(2));

            }

            ret = list;

            FinalizeManager();

            return ret;
        }

        [HttpGet]
        [Route("listlangs")]
        public async Task<object> ListLanguages()
        {
            Init(PERMISSION_CHECK_ENUM.READ, true);

            List<LocalizationTextList> list = null;

            list = memorycache.Get<List<LocalizationTextList>>("LANGS");

            if (list == null)
            {
                list = await Membership.LocalizationText.GetListOfLanguages();

                memorycache.Set("LANGS", list, this.GetMemoryCacheOptionsByHour(2));

            }

            ret = list;

            FinalizeManager();

            return ret;
        }

        [HttpGet]
        [Route("listlocalizationtexts")]
        public async Task<object> ListLocalizationTexts()
        {
            Init(PERMISSION_CHECK_ENUM.READ, true);

            List<LocalizationTextResult> list = null;

            list = memorycache.Get<List<LocalizationTextResult>>("LOCALIZATIONTEXTS");

            if (list == null)
            {
                list = await Membership.LocalizationText.Search(new LocalizationTextParam()); 

                memorycache.Set("LOCALIZATIONTEXTS", list, this.GetMemoryCacheOptionsByHour(2));

            }

            ret = list;

            FinalizeManager();

            return ret;
        }

    }
}
