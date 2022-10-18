using System;
using GW.Core.Data;
using GW.Core.Data.SQLServer;
using GW.Core.Manager;
using GW.Core.Service;
using GW.Membership.Domain.Interfaces;
using GW.Membership.Domain;
using Template.Core.Domain.Interfaces;
using Template.Core.Domain;

namespace Template.Core.Manager
{
    public class TemplateManager : ITemplateManager
    {
        public IMembershipDomain Membership { get ; set ; }

        public ITemplateDomain Admin { get; set; } 

        public IManagerConfigs Configs { get; set; }

        public IDbContext[] DbContext { get; set; }
        
        public IMailingManager Mailing { get; set; }

        public TemplateManager(IManagerConfigs configs)
        {
            Configs = configs;
            Mailing = new TemplateMailCenter(configs.MailSettings);

            InitializeContext();
            InitializeDomain(); 
        }

        public void InitializeContext()
        {
            DbContext = new IDbContext[1];
            SQLServerContext ctx1;
          
            ctx1 = new SQLServerContext(Configs.Connections[0].Value);
        
            ctx1.Begin();

            DbContext[0] = ctx1;

        }

        public void FinalizeContext()
        {
            foreach (var context in DbContext)
            {                
                 context.End(true);
               
            }

        }

        public void InitializeDomain()
        {
            Membership = new MembershipDomain(DbContext[0]);
            Admin = new AdminDomain(DbContext[0]);
            
        }
    }
}
