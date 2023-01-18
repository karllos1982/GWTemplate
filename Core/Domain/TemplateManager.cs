using GW.Core;
using Template.Contracts.Domain;
using Template.Contracts.Data;
using GW.Common;
using GW.Helpers;
using GW.Membership.Domain;

namespace Template.Domain
{
    public class TemplateManager : ITemplateManager
    {
        
        public TemplateManager(IContext context, ITemplateRepositorySet repositorySet)
        {
            Context = context;
            InitializeDomains(context, repositorySet);
         
        }

        public void InitializeDomains(IContext context, IRepositorySet repositorySet)
        {            
            Client = new ClientDomain(context,(ITemplateRepositorySet)repositorySet);
        }

        public IContext Context { get; set; }

        public IClientDomain Client { get ; set ; }
    }
}
