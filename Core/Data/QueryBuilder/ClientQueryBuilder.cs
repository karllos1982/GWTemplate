using GW.Helpers;
using System.Collections.Generic;

namespace Template.Data
{
    public class ClientQueryBuilder : QueryBuilder
    {
        public ClientQueryBuilder()
        {
            Initialize();
        }

        public override void Initialize()
        {
            Keys = new List<string>();
            ExcludeFields = new List<string>();

            Keys.Add("ClientID");
            ExcludeFields.Add("Contacts"); 

        }

        public override string QueryForGet(object param)
        {
            string ret = @"Select * from Client where ClientID=@pClientID";            
               
            return ret;
        }

        public override string QueryForList(object param)
        {
            string ret = @"select ClientID, ClientName             
             from Client s                           
             where 1=1 
             and (@pClientID=0 or s.ClientID=@pClientID)
             and (@pClientName='' or s.ClientName=@pClientName)
             and (@pEmail='' or s.Email=@pEmail)
             order by ClientName
             ";

            return ret;
        }

        public override string QueryForSearch(object param)
        {

            string ret = @"select *             
             from Client s              
             where 1=1 
             and (@pClientID=0 or s.ClientID=@pClientID)
             and (@pClientName='' or s.ClientName=@pClientName)
             and (@pEmail='' or s.Email=@pEmail)
             order by ClientName
             ";

            return ret;

        }


    }
}
