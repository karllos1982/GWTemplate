using GW.Helpers;

namespace Template.Models
{
    public class ClientParam
    {
        public ClientParam()
        {
            pEmail = "";
            pClientName = "";
            pClientID = 0;
        }

        public Int64 pClientID { get; set; }

        public string pClientName { get; set; }

        public string pEmail { get; set; }

    }

    public class ClientEntry
    {

        public ClientEntry()
        {

        }

        public ClientEntry(ClientResult fromobj)
        {
            ClientID = fromobj.ClientID;
            ClientName = fromobj.ClientName;
            Email = fromobj.Email;
            PhoneNamber = fromobj.PhoneNamber;
            IsActive = fromobj.IsActive;
            CreateDate = fromobj.CreateDate;
            Contacts = new List<ClientContactsEntry>();

            foreach (ClientContactsResult c in fromobj.Contacts)
            {
                Contacts.Add(new ClientContactsEntry(c)); 
            }
            
        }

        public Int64 ClientID { get; set; }

        [PrimaryValidationConfig("ClientName", "Client Name", FieldType.TEXT, false, 50)]
        public string ClientName { get; set; }

        [PrimaryValidationConfig("Email", "E-mail", FieldType.EMAIL, false, 255)]
        public string Email { get; set; }

        [PrimaryValidationConfig("PhoneNamber", "Phone Number", FieldType.PHONENUMBER, false, 15)]
        public string PhoneNamber { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public List<ClientContactsEntry> Contacts { get; set; }
        

    }

    public class ClientList
    {
        public Int64 ClientID { get; set; }

        public string ClientName { get; set; }        

    }

    public class ClientResult
    {
        public Int64 ClientID { get; set; }
       
        public string ClientName { get; set; }
                
        public string Email { get; set; }

        public string PhoneNamber { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public List<ClientContactsResult> Contacts { get; set; }
    }

}
