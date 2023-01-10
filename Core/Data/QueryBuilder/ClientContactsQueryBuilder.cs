using GW.Helpers;
using System.Collections.Generic;

namespace Template.Data
{
    public class ClientContactsQueryBuilder : QueryBuilder
    {
        public ClientContactsQueryBuilder()
        {
            Initialize();
        }

        public override void Initialize()
        {
            Keys = new List<string>();
            ExcludeFields = new List<string>();

            Keys.Add("ClientContactID");

            ExcludeFields.Add("RecordState"); 

        }

        public override string QueryForGet(object param)
        {
            string ret = @"Select s.* 
            from ClientContacts s
            where ClientContactID=@pClientContactID";

            return ret;
        }

        public override string QueryForList(object param)
        {
            string ret = @"select ClientContactID, ClientID, ContactName             
             from ClientContacts s                           
             where 1=1 
             and (@pClientContactID=0 or s.ClientContactID=@pClientContactID)
             and (@pClientID=0 or s.ClientID=@pClientID)
             and (@pContactName='' or s.ContactName=@pContactName)
             and (@pEmail='' or s.Email=@pEmail)
             order by ContactName
             ";

            return ret;
        }

        public override string QueryForSearch(object param)
        {

            string ret = @"select *             
             from ClientContacts s             
             where 1=1 
             and (@pClientContactID=0 or s.ClientContactID=@pClientContactID)
             and (@pClientID=0 or s.ClientID=@pClientID)
             and (@pContactName='' or s.ContactName=@pContactName)
             and (@pEmail='' or s.Email=@pEmail)
             order by ContactName
             ";

            return ret;

        }


    }
}
