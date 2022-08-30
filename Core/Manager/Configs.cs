using GW.Core.Helpers;
using GW.Core.Common;
using Template.Core.Manager;

namespace Template.Core.Manager
{
    public class TemplateConfigs : ITemplateConfigs
    {
        public List<ConnectionString> Connections  { get ; set ; }
        public string ServiceBaseURL { get; set; }
        public MailSettings MailSettings { get; set; }
       

    }
}
