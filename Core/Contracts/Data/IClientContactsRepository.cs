using GW.Core;
using Template.Models;

namespace Template.Contracts.Data
{
    public interface IClientContactsRepository :
        IRepository<ClientContactsParam, ClientContactsEntry, ClientContactsResult, ClientContactsList>
    {


    }
}