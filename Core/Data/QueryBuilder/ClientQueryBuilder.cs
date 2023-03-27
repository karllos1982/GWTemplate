using GW.Helpers;

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

            string ret = ""; 

            SelectBuilder.Clear();
            SelectBuilder.AddTable("Client", "c", true, "ClientID", "", JOINTYPE.NONE, null);                        
            SelectBuilder.AddField("c", "ClientID", "@pClientID", false, "0", null, ORDERBYTYPE.ASC);

            ret = SelectBuilder.BuildQuery();

            return ret;
        }

        public override string QueryForList(object param)
        {

            string ret = "";

            SelectBuilder.Clear();
            SelectBuilder.AddTable("Client", "c", false, "ClientID", "", JOINTYPE.NONE, null);
            SelectBuilder.AddField("c", "ClientID", "@pClientID", true, "0", null, ORDERBYTYPE.NONE);
            SelectBuilder.AddField("c", "ClientName", "@pClientName", true, "''", null, ORDERBYTYPE.ASC);
            SelectBuilder.AddField("c", "Email", "@pEmail", false, "''", null, ORDERBYTYPE.NONE);

            ret = SelectBuilder.BuildQuery();

            return ret;
        }

        public override string QueryForSearch(object param)
        {

            string ret = "";

            SelectBuilder.Clear();
            SelectBuilder.AddTable("Client", "c", true, "ClientID", "", JOINTYPE.NONE, null);
            SelectBuilder.AddField("c", "ClientID", "@pClientID", false, "0", null, ORDERBYTYPE.NONE);
            SelectBuilder.AddField("c", "ClientName", "@pClientName", false, "''", null, ORDERBYTYPE.ASC);
            SelectBuilder.AddField("c", "Email", "@pEmail", false, "''", null, ORDERBYTYPE.NONE);

            ret = SelectBuilder.BuildQuery();

            return ret;

        }


    }
}
