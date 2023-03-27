using GW.Core;

namespace Template.Contracts.Data
{
    public interface ITemplateRepositorySet: IRepositorySet
    {

        IClientRepository Client { get; set; } 

        IClientContactsRepository ClientContacts { get; set; }

    }
}
