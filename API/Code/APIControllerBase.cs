using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GW.Membership.Models;
using Template.Core.Manager;
using GW.Core.Common;
using GW.Core.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;

namespace Template.API
{
    public class APIControllerBase: ControllerBase
    {
        
        public OperationStatus opsts = new OperationStatus();
        public object ret = null;

        public MailSettings mailSettings = null;
        public TemplateConfigs apiConfigs = null;
        public TemplateMailCenter mailCenter = null;
        public TemplateManager manager = null;
        public int contextindex = 0; 
        public IAppSettingsManager<TemplateSettings> AppConfigs;

        public IMemoryCache memorycache = null; 

        public MemoryCacheEntryOptions GetMemoryCacheOptionsByHour(int time)
        {
            MemoryCacheEntryOptions ret = new MemoryCacheEntryOptions();

            ret.SetSlidingExpiration(TimeSpan.FromHours(time)); 
                
            return ret;
        }

        public void Init()
        {
            mailSettings = AppConfigs.Settings.MailSettings;
            mailSettings.IsBodyHtml = true;
            apiConfigs = new TemplateConfigs();            
            apiConfigs.ServiceBaseURL = "";
            apiConfigs.MailSettings = mailSettings;
            apiConfigs.Connections = AppConfigs.Settings.Connections;
            mailCenter = new TemplateMailCenter(mailSettings);
            manager = new TemplateManager(apiConfigs);

        }

        public void FinalizeManager()
        {
            if (manager != null)
            {
                manager.FinalizeContext();
            }
        }

        public void SetReturn(object returncontent)
        {
            if (manager.DbContext[contextindex].ExecutionStatus.Status)
            {
                ret = returncontent;
            }
            else
            {
                Response.StatusCode = 500;
                ret = GetInnerExceptions(manager.DbContext[contextindex].ExecutionStatus.Error.Message);

            }
        }

        public OperationStatus GetDefaultStatus()
        {
            return manager.DbContext[contextindex].ExecutionStatus;
        }

        public List<InnerException> GetInnerExceptions(string errormessage)
        {
            List<InnerException> ret = new List<InnerException>();

            ret.Add(new InnerException("Error", errormessage)); 

            return ret;
        }

        public string SerializeObjectToJSON_String(object obj)
        {
            string ret = "[]";

            if (obj != null)
            {
                ret = JsonConvert.SerializeObject(obj); 
            }

            return ret;
        }

    }
}
