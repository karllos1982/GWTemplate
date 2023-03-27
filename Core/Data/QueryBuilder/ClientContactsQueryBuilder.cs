using GW.Helpers;

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
            string ret = "";

            SelectBuilder.Clear();
            SelectBuilder.AddTable("ClientContacts", "s", true, "ClientContactID", "", JOINTYPE.NONE, null);
            SelectBuilder.AddField("s", "ClientContactID", "@pClientContactID", false, "0", null, ORDERBYTYPE.ASC);

            ret = SelectBuilder.BuildQuery();          

            return ret;
        }

        public override string QueryForList(object param)
        {

            string ret = "";

            SelectBuilder.Clear();
            SelectBuilder.AddTable("ClientContacts", "s", true, "ClientContactID", "", JOINTYPE.NONE, null);
            SelectBuilder.AddField("s", "ClientContactID", "@pClientContactID", true, "0", null, ORDERBYTYPE.NONE);
            SelectBuilder.AddField("s", "ClientID", "@pClientID", true, "0", null, ORDERBYTYPE.NONE);
            SelectBuilder.AddField("s", "ContactName", "@pContactName", true, "''", null, ORDERBYTYPE.ASC );
            SelectBuilder.AddField("s", "Email", "@pEmail", false, "''", null, ORDERBYTYPE.NONE);

            ret = SelectBuilder.BuildQuery();
           

            return ret;
        }

        public override string QueryForSearch(object param)
        {

            string ret = "";

            SelectBuilder.Clear();
            SelectBuilder.AddTable("ClientContacts", "s", true, "ClientContactID", "", JOINTYPE.NONE, null);
            SelectBuilder.AddField("s", "ClientContactID", "@pClientContactID", false, "0", null, ORDERBYTYPE.NONE);
            SelectBuilder.AddField("s", "ClientID", "@pClientID", false, "0", null, ORDERBYTYPE.NONE);
            SelectBuilder.AddField("s", "ContactName", "@pContactName", false, "''", null, ORDERBYTYPE.ASC);
            SelectBuilder.AddField("s", "Email", "@pEmail", false, "''", null, ORDERBYTYPE.NONE);

            ret = SelectBuilder.BuildQuery();          

            return ret;

        }


    }
}
