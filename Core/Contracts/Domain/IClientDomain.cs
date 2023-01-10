using GW.Common;
using GW.Core;
using Template.Models;
using Template.Contracts.Data;

namespace Template.Contracts.Domain
{
    public interface IClientDomain :
        IDomain<ClientParam, ClientEntry, ClientResult, ClientList>
    {
        ITemplateRepositorySet RepositorySet { get; set; }

        ClientContactsEntry ContactEntryValidation(ClientContactsEntry entry);
        
    }
}
