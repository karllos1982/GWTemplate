using GW.Core;

namespace Template.Contracts.Domain
{
    public interface  ITemplateManager: IManager
    {
        IClientDomain Client { get; set; }

    }
}
