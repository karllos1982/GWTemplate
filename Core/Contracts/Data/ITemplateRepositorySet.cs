using GW.Common;
using GW.Core;
using Template.Contracts.Data; 

namespace Template.Contracts.Data
{
    public interface ITemplateRepositorySet: IRepositorySet
    {

        IClientRepository Client { get; set; } 

        IClientContactsRepository ClientContacts { get; set; }

    }
}
