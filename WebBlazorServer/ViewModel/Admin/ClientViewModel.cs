using GW.Common;
using Template.Models;
using Template.Gateway;
using GW.Membership.Models;
using GW.Helpers;

namespace Template.ViewModel
{
    public class ClientViewModel : BaseViewModel
    {

        private TemplateGateway _gateway;

        public ContactSummaryManager ContactSummary;

        public ClientViewModel(TemplateGateway service,
            UserAuthenticated user)
        {
            _gateway = service;
            ContactSummary = new ContactSummaryManager(); 
            this.InitializeView(user);
        }

        UserAuthenticated _user;

        public ClientResult result = new ClientResult();
        public ClientParam param = new ClientParam() { };
        public List<ClientResult> searchresult = new List<ClientResult>();


        public override async Task ClearSummaryValidation()
        {
            SummaryValidation = new List<InnerException>()
            {
                new InnerException("ClientName",""),
                new InnerException("Email",""),
                new InnerException("PhoneNamber",""),

            };

        }

        public override async Task InitializeModels()
        {

            await ClearSummaryValidation();

            ContactSummary.ClearSummaryValidation();
        }


        public override async Task Set()
        {
            ExecutionStatus = new OperationStatus(true);

            ClientEntry entry = new ClientEntry(result);

            ClientEntry ret = await _gateway.Client.Set(entry);

            if (ret != null)
            {
                ExecutionStatus.Returns = ret;
            }
            else
            {
                ExecutionStatus.InnerExceptions = _gateway.Client.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
                this.ShowSummaryValidation(ExecutionStatus.InnerExceptions);
            }


        }

        public override async Task Get(object id)
        {

            ExecutionStatus = new OperationStatus(true);

            result = await _gateway.Client.Get(id.ToString());

            if (result == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.Client.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }

        }

        public override void BackToSearch()
        {
            this.BaseBack();

        }

        public override void InitEdit()
        {
            this.BaseInitEdit();

        }

        public override void InitNew()
        {
            this.BaseInitNew();
            result = new ClientResult();
            result.CreateDate = DateTime.Now;
            result.ClientID = Utilities.GenerateId();
        }

        public override async Task Search()
        {

            ExecutionStatus = new OperationStatus(true);

            searchresult = await _gateway.Client.Search(param);

            if (searchresult == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.Client.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }

        }

        // contacts functions        
        public ClientContactsResult contact = new ClientContactsResult();
        public string contactstate = ""; 

        public void GetContactToEdit(Int64 id)
        {
            ContactSummary.ClearSummaryValidation();  

            contact = result.Contacts.Where(c => c.ClientContactID == id).FirstOrDefault();
            contactstate = "Editando Contato";
        }      

        public void InitNewContact()
        {
            ContactSummary.ClearSummaryValidation();

            contactstate = "Inserindo novo Contato";
            contact = new ClientContactsResult();
            contact.RecordState = RECORDSTATEENUM.ADD;
            contact.ClientContactID = Utilities.GenerateId();
        }

        public async Task SaveContact()
        {
            ExecutionStatus = new OperationStatus(true);

            ContactSummary.ClearSummaryValidation();

            ClientContactsEntry ret 
                    = await _gateway.Client.ContactEntryValidation(new ClientContactsEntry(contact));

            if (ret != null)
            {              

                if (contact.RecordState == RECORDSTATEENUM.NONE)
                {
                    contact.RecordState = RECORDSTATEENUM.EDITED;
                }

                if (contact.RecordState == RECORDSTATEENUM.ADD)
                {
                    if (result.Contacts == null)
                    {
                        result.Contacts = new List<ClientContactsResult>();
                    }

                    result.Contacts.Add(contact);
                }
            }
            else
            {
                ExecutionStatus.InnerExceptions = _gateway.Client.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
                ContactSummary.ShowSummaryValidation(ExecutionStatus.InnerExceptions);
            }


        }


    }


    public class ContactSummaryManager : SummaryManager
    {
        public override void ClearSummaryValidation()
        {
            SummaryValidation = new List<InnerException>()
            {
                new InnerException("ContactName",""),
                new InnerException("Email",""),
                new InnerException("CellPhoneNumber",""),

            };

        }
    }
}


