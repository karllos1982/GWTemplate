using GW.Membership.Models;
using System;
using System.Net.Sockets;
using System.Reflection;

namespace WebBlazorServer.Localization
{
    public class LocalizationBase
    {        

        public void FillTexts(List<LocalizationTextResult> textLists,
                string language)
        {
            Type t = this.GetType();
            PropertyInfo[] prop = t.GetProperties();

            string text = "";
            string auxname = "";

            foreach (PropertyInfo p in prop)
            {
                
                auxname = p.Name;
                auxname = auxname.Replace("_", "-");

                text =GetText(textLists, auxname, language); 
                    
                p.SetValue(this , text, null);
                
            }

        }

        private string GetText(List<LocalizationTextResult> textLists,
                string name, string lang )
        {
            string ret = "";

            var aux = textLists
                .Where(t => t.Name == name && t.Language == lang)
                .FirstOrDefault(); 

            if (aux != null)
            {
                ret = aux.Text; 
            }

            return ret;

        }


        public string SearchButtonLabel { get; set; }
       
        public string  SearchingLabel { get; set; }

        public string InsertingLoadingLabel { get; set; }

        public string SearchResultLabel { get; set; }

        public string DetailsLabel { get; set; }

        public string NoRecordsFound { get; set; }

        public string LoadingPage { get; set; }

        public string LoadingData { get; set; }

        public string ErrorOnExecuteSearch { get; set; }

        public string ErrorOnReturnData { get; set; }

        public string ErrorOnCreateNewRecord { get; set; }

        public string AfterSaveAnswering { get; set; }

        public string NoticeLabel { get; set; }

        public string SuccessLabel { get; set; }

    }
}
