﻿using Microsoft.AspNetCore.Mvc;
using GW.Membership.Models;
using GW.Core.Common;
using Template.API;
using Microsoft.AspNetCore.Authorization;
//using Template.Models;

namespace Template.Controllers
{
    [Route("membership/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class DataLogController : APIControllerBase
    {
        public DataLogController(IAppSettingsManager<TemplateSettings> param,
           IWebHostEnvironment hostingEnvironment)
        {
                AppConfigs = param;
                AppConfigs.EnvironmentSettings = hostingEnvironment;
                AppConfigs.LoadSettings();

        }

        [HttpPost]
        [Route("search")]        
        public object Search(DataLogParam param)
        {
            Init();

            List<DataLogSearchResult> list = null;

            list = manager.Membership.DataLogUnit.Search(param);

            SetReturn(list);
            FinalizeManager();

            return ret;
        }

        [HttpPost]
        [Route("list")]        
        public object List(DataLogParam param)
        {
            Init();

            List<DataLogList> list = null;

            list = manager.Membership.DataLogUnit.List(param);

            SetReturn(list);
            FinalizeManager();

            return ret;
        }

        [HttpGet]
        [Route("get")]        
        public object Get(string id)
        {
            Init();

            DataLogModel obj = null;

            obj = manager.Membership.DataLogUnit.Get(Int64.Parse(id));

            if (this.GetDefaultStatus().Status)
            {
                if (obj != null)
                {                    
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

            return ret;
        }

        [HttpGet]
        [Route("gettimeline")]       
        public object GetTimeLine(string id)
        {
            Init();

            List<DataLogTimelineModel> list = null;

            list = manager.Membership.DataLogUnit.GetTimeLine(Int64.Parse(id));

            SetReturn(list);
            FinalizeManager();

            return ret;
        }


        [HttpGet]
        [Route("gettablelist")]        
        public object GetTableList()
        {
            Init();

            List<TabelasValueModel> list = null;

            list = manager.Membership.DataLogUnit.GetTableList();

            SetReturn(list);
            FinalizeManager();

            return ret;
        }
        


    }
}