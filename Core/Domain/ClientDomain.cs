using GW.Common;
using GW.Core;
using Template.Contracts.Domain;
using Template.Models;
using Template.Contracts.Data;
using GW.Helpers;

namespace Template.Domain
{
    public class ClientDomain : IClientDomain
    {
        
        public ClientDomain(IContext context, ITemplateRepositorySet repositorySet)
        {
            Context = context;
            RepositorySet = repositorySet;            
        }

        public IContext Context { get; set; }

        public ITemplateRepositorySet RepositorySet { get; set; }

        public async Task<ClientResult> FillChields(ClientResult obj)
        {
            ClientContactsParam param = new ClientContactsParam();

            param.pClientID = obj.ClientID;

            List<ClientContactsResult> list
                = await RepositorySet.ClientContacts.Search(param);

            obj.Contacts = list;

            return obj;
        }

        public async Task<ClientResult> Get(ClientParam param)
        {
            ClientResult ret = null;

            ret = await RepositorySet.Client.Read(param);

            if (ret != null) {

               await FillChields(ret); 
                   
            }

            return ret;
        }

        public async Task<List<ClientList>> List(ClientParam param)
        {
            List<ClientList> ret = null;

            ret = await RepositorySet.Client.List(param);

            return ret;
        }

        public async Task<List<ClientResult>> Search(ClientParam param)
        {
            List<ClientResult> ret = null;

            ret = await RepositorySet.Client.Search(param);

            return ret;
        }

        public async Task EntryValidation(ClientEntry obj)
        {
            OperationStatus ret = null;

            ret = PrimaryValidation.Execute(obj, new List<string>(), Context.LocalizationLanguage);

            OperationStatus aux = ContactsEntriesValidation(obj.Contacts);

            if (ret.Status)
            {
                ret = aux; 
            }
            else
            {
                if (!aux.Status)
                {
                    ret.InnerExceptions.Add(aux.InnerExceptions[0]); 
                }                
            }

            if (!ret.Status)
            {               
                ret.Error 
                    = new Exception(GW.LocalizationText.Get("Validation-Error", Context.LocalizationLanguage).Text);
            }
          
            Context.ExecutionStatus = ret;

        }

        public OperationStatus ContactsEntriesValidation(List<ClientContactsEntry> entries)
        {
            OperationStatus ret = new OperationStatus(true);

            entries = entries
                .Where(e => e.RecordState != RECORDSTATEENUM.DELETED).ToList();

            if (entries == null)
            {
                ret.Status = false;
            }
            else
            {
                if (entries.Count == 0) { ret.Status = false; }                                                                                                   
            }

            if (!ret.Status)
            {               
                if (ret.InnerExceptions == null)
                {
                    ret.InnerExceptions = new List<InnerException>();
                }

                ret.AddInnerException("Contacts",
                   "Contacts: " +  GW.LocalizationText.Get("Validation-NotNull", Context.LocalizationLanguage).Text);               
            }
            else
            {               

                foreach (ClientContactsEntry item in entries)
                {
                     if (PrimaryValidation.Execute(item, new List<string>(), Context.LocalizationLanguage).Status==false)
                     {
                        if (ret.InnerExceptions == null)
                        {
                            ret.InnerExceptions = new List<InnerException>();
                        }

                        ret.Status = false; 
                        ret.AddInnerException("Contacts",
                              "Contacts: " + GW.LocalizationText.Get("Validation-Error", Context.LocalizationLanguage).Text);

                        break;

                     }                 
                }

            }
        
            return ret;
        }

        public async Task InsertValidation(ClientEntry obj)
        {
            OperationStatus ret = new OperationStatus(true);
            ClientParam param = new ClientParam()
            {
                pClientName = obj.ClientName
            };

            List<ClientList> list
                = await RepositorySet.Client.List(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    ret.Status = false;
                    string msg
                        = string.Format(GW.LocalizationText.Get("Validation-Unique-Value", 
                            Context.LocalizationLanguage).Text, "Client Name"); 
                    ret.Error = new Exception(msg);
                    ret.AddInnerException("ClientName", msg);
                }
            }

            Context.ExecutionStatus = ret;

        }

        public async Task UpdateValidation(ClientEntry obj)
        {
            OperationStatus ret = new OperationStatus(true);
            ClientParam param = new ClientParam() { pClientName = obj.ClientName };
            List<ClientList> list
                = await RepositorySet.Client.List(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    if (list[0].ClientID != obj.ClientID)
                    {
                        ret.Status = false;
                        string msg 
                            = string.Format(GW.LocalizationText.Get("Validation-Unique-Value", 
                                Context.LocalizationLanguage).Text, "Client Name");
                        ret.Error = new Exception(msg);
                        ret.AddInnerException("ClientName", msg);
                    }
                }
            }

            Context.ExecutionStatus = ret;

        }

        public async Task DeleteValidation(ClientEntry obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task<ClientEntry> Set(ClientEntry model, object userid)
        {
            ClientEntry ret = null;
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

            await EntryValidation(model);

            if (Context.ExecutionStatus.Status)
            {

                ClientResult old
                    = await RepositorySet.Client.Read(new ClientParam() { pClientID = model.ClientID });

                if (old == null)
                {
                    await InsertValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {
                        model.CreateDate = DateTime.Now;
                        if (model.ClientID == 0) { model.ClientID = GW.Helpers.Utilities.GenerateId(); }
                        await RepositorySet.Client.Create(model);
                    }
                }
                else
                {
                    model.CreateDate = old.CreateDate;
                    operation = OPERATIONLOGENUM.UPDATE;

                    await UpdateValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {
                        await RepositorySet.Client.Update(model);
                    }

                }

                if (model.Contacts != null)
                {
                    foreach (ClientContactsEntry u in model.Contacts )
                    {
                        if (u.RecordState != RECORDSTATEENUM.NONE)
                        {
                            u.ClientID = model.ClientID;

                            if (u.RecordState == RECORDSTATEENUM.ADD)
                            {
                               await RepositorySet.ClientContacts.Create(u);
                            }

                            if (u.RecordState == RECORDSTATEENUM.EDITED)
                            {
                                await RepositorySet.ClientContacts.Update(u);
                            }

                            if (u.RecordState == RECORDSTATEENUM.DELETED)
                            {
                                await RepositorySet.ClientContacts.Delete(u);
                            }

                        }
                    }
                }


                if (Context.ExecutionStatus.Status && userid != null)
                {
                    await RepositorySet.Client.Context
                        .RegisterDataLogAsync(userid.ToString(), operation, "CLIENT",
                        model.ClientID.ToString(), old, model);

                    ret = model;
                }

            }

            return ret;
        }

       
        public async Task<ClientEntry> Delete(ClientEntry model, object userid)
        {
            ClientEntry ret = null;

            ClientResult old
                = await RepositorySet.Client.Read(new ClientParam() { pClientID = model.ClientID });

            if (old != null)
            {
                await DeleteValidation(model);

                if (Context.ExecutionStatus.Status)
                {

                    if (model.Contacts != null)
                    {
                        foreach (ClientContactsEntry u in model.Contacts)
                        {

                            await RepositorySet.ClientContacts.Delete(u);

                        }
                    }

                    await RepositorySet.Client.Delete(model);

                    if (Context.ExecutionStatus.Status && userid != null)
                    {
                        await RepositorySet.Client.Context
                            .RegisterDataLogAsync(userid.ToString(),  OPERATIONLOGENUM.DELETE, "CLIENT",
                            model.ClientID.ToString(), old, model);

                        ret = model;
                    }

                }
            }
            else
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error 
                    = new System.Exception(GW.LocalizationText.Get("Record-NotFound", Context.LocalizationLanguage).Text);

            }

            return ret;
        }

        public ClientContactsEntry ContactEntryValidation(ClientContactsEntry entry)
        {
            ClientContactsEntry ret = entry;

            Context.ExecutionStatus 
                = PrimaryValidation.Execute(entry, new List<string>(), Context.LocalizationLanguage);

            if (!Context.ExecutionStatus.Status)
            {
                Context.ExecutionStatus.Error
                    = new Exception(GW.LocalizationText.Get("Validation-Error", Context.LocalizationLanguage).Text);
                ret = null; 
            }

            return ret;

        }

    }
}
