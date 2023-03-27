using GW.Helpers;
using GW.Common;

namespace Template.Models
{
    public class ClientContactsParam
    {
        public ClientContactsParam()
        {
            pContactName = "";
            pEmail = ""; 

        }

        public Int64 pClientContactID { get; set; }

        public Int64 pClientID { get; set; }

        public string pContactName { get; set; }

        public string pEmail { get; set;}

    }

    public class ClientContactsEntry
    {
        public ClientContactsEntry()
        {

        }

        public ClientContactsEntry(ClientContactsResult result)
        {
            this.ClientContactID= result.ClientContactID;
            this.ClientID= result.ClientID;
            this.ContactName= result.ContactName;
            this.Email= result.Email;
            this.CellPhoneNumber= result.CellPhoneNumber;
            this.RecordState = result.RecordState;   
        }
               
        public Int64 ClientContactID { get; set; }

        public Int64 ClientID { get; set; }

        [PrimaryValidationConfig("ContactName", "Contact Name", FieldType.TEXT, false, 50)]
        public string ContactName { get; set; }

        [PrimaryValidationConfig("Email", "E-mail", FieldType.EMAIL, false, 255)]
        public string Email { get; set; }

        [PrimaryValidationConfig("CellPhoneNumber", "CellPhoneNumber", FieldType.CELLPHONENUMBER, false, 15)]
        public string CellPhoneNumber { get; set; }

        public RECORDSTATEENUM RecordState { get; set; }

    }

    public class ClientContactsList
    {
        public Int64 ClientContactID { get; set; }

        public Int64 ClientID { get; set; }

        public string ContactName { get; set; }

    }

    public class ClientContactsResult
    {
      
        public Int64 ClientContactID { get; set; }

        public Int64 ClientID { get; set; }

        public string ContactName { get; set; }

        public string Email { get; set; }

        public string CellPhoneNumber { get; set; }

        public RECORDSTATEENUM RecordState { get; set; }

    }

}
