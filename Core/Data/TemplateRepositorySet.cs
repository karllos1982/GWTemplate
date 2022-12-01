using GW.Common;
using GW.Core;
using Core.Contracts.Data;

namespace Core.Data
{
    public class TemplateRepositorySet : ITemplateRepositorySet
    {

        public TemplateRepositorySet(IContext context)
        {
            this.InitializeRespositories(context);
        }

        public void InitializeRespositories(IContext context)
        {
            
        }
    }

}
