using GW.Core;
using GW.Common;

namespace API.Code
{

    public interface ILocalizationInitializer
    {        
    }

    public class LocalizationInitializer: ILocalizationInitializer
    {
        public LocalizationInitializer(IContext context)
        {
            GW.LocalizationText.LoadData(context);

        }
      
    }
}
