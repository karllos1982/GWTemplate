using GW.Common;
using GW.Core;
using Template.Models;

namespace Template.Contracts.Data
{
    public interface IClientRepository:
        IRepository<ClientParam, ClientEntry, ClientResult, ClientList>
    {
     

    }
}
