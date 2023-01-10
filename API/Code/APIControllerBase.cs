using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GW.Membership.Models;
using Template.Core.Manager;
using GW.Common;
using GW.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Security.Permissions;
using GW.Core;
using GW.Membership.Contracts.Domain;
using GW.ApplicationHelpers;
using Template.Contracts.Domain;

namespace Template.API
{
    public class APIControllerBase: ControllerBase
    {
        protected bool IsAllowed = false; 
        protected PERMISSION_STATE_ENUM PermissionState = PERMISSION_STATE_ENUM.NONE;

        public string ObjectCode = "";
        public OperationStatus opsts = new OperationStatus();
        public object ret = null;
        public int contextindex = 0;       
        public IUserPermissionsManager<UserPermissions> PermissionsManager;

        public IMemoryCache memorycache = null;
        public IContext Context; 
        public IMembershipManager Membership;
        public MailManager MailCenter;
        public ITemplateManager Manager;

        protected MemoryCacheEntryOptions GetMemoryCacheOptionsByHour(int time)
        {
            MemoryCacheEntryOptions ret = new MemoryCacheEntryOptions();

            ret.SetSlidingExpiration(TimeSpan.FromHours(time)); 
                
            return ret;
        }

        protected void Init_()
        {
                       
            IsAllowed = true;
            PermissionState = PERMISSION_STATE_ENUM.NONE;
        }

        protected void Init( PERMISSION_CHECK_ENUM? checking = null, bool? allownone = null)
        {
           
            if (checking != null && User.Claims.ToList().Count > 0)
            {
                 
                string content = User.Claims.ToList()[2].Value;

                List<UserPermissions> permissions = JsonConvert.DeserializeObject<List<UserPermissions>>(content);               
                                      
                PermissionState = Utilities.CheckPermission(permissions, ObjectCode, checking.Value);
                IsAllowed = false;
                if (PermissionState == PERMISSION_STATE_ENUM.ALLOWED)
                {
                    IsAllowed = true;
                }

                if (allownone.Value && PermissionState == PERMISSION_STATE_ENUM.NONE)
                {
                    IsAllowed = true;
                }

                if (!IsAllowed)
                {
                    Response.StatusCode = 403;
                    Context.ExecutionStatus = new OperationStatus(false);
                    Context.ExecutionStatus.Error =
                        new Exception("Acesso negado ao recurso: " + ObjectCode + " / " + checking.ToString());
                    ret = GetInnerExceptions("Acesso negado ao recurso: " + ObjectCode + " / " + checking.ToString());
                }
            }

        }


        protected void FinalizeManager()
        {
            Context.End(); 
        }

        protected void SetReturn(object returncontent)
        {
            if (Context.ExecutionStatus.Status)
            {
                ret = returncontent;
            }
            else
            {
                Response.StatusCode = 500;
                ret = GetInnerExceptions(Context.ExecutionStatus.Error.Message);

            }
        }

        protected OperationStatus GetDefaultStatus()
        {
            return Context.ExecutionStatus;
        }

        protected List<InnerException> GetInnerExceptions(string errormessage)
        {
            List<InnerException> ret = new List<InnerException>();

            ret.Add(new InnerException("Error", errormessage)); 

            return ret;
        }

        protected string SerializeObjectToJSON_String(object obj)
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
