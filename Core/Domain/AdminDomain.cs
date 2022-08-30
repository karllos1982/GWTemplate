using System;
using System.Collections.Generic;
using GW.Core.Common;
using GW.Core.Data;
using GW.Core.Service;
using GW.Core.Domain;
using GW.Core.Helpers;
using Template.Core.Domain; 
using Template.Core.Domain.Interfaces;

namespace Template.Core.Domain
{
    public class AdminDomain :ITemplateDomain
    {       

        public IDbContext DbContext { get; set; }

        
        public AdminDomain(IDbContext dbContext)
        {
            this.DbContext = dbContext;
          
            InitializeUnits();
        }

        public void InitializeUnits()
        {
            
        }


       
    }
}
