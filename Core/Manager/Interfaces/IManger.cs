using GW.Membership.Domain.Interfaces;
using GW.Core.Manager;
using Template.Core.Domain.Interfaces;

namespace Template.Core.Manager
{
    public interface ITemplateManager : IManager
    {
        
       IMembershipDomain Membership { get; set; }

        ITemplateDomain Admin { get; set; }
     
            
    }
}
