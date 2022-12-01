using GW.Core;
using Core.Contracts.Domain;
using Core.Contracts.Data;
using GW.Common;
using GW.Helpers;

namespace Core.Domain
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

        }

        public IContext Context { get; set; }
                

    }
}
