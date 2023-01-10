using GW.Common;
using GW.Core;
using Template.Contracts.Data;

namespace Template.Data
{
    public class TemplateRepositorySet : ITemplateRepositorySet
    {

        public TemplateRepositorySet(IContext context)
        {
            this.InitializeRespositories(context);
        }

        public IClientRepository Client { get; set ; }

        public IClientContactsRepository ClientContacts { get; set ; }

        public void InitializeRespositories(IContext context)
        {
            Client= new ClientRepository(context);
            ClientContacts = new ClientContactsRespository(context);    

        }
    }

}
