using GW.Common;
using GW.Membership.Models;
using Template.Gateway;

namespace Template.ViewModel
{
    public class ObjectPermissionViewModel : BaseViewModel
    {

        private MembershipGateway _gateway;
        private DataCacheGateway _cache;

        public ObjectPermissionViewModel(MembershipGateway service, DataCacheGateway cache,
            UserAuthenticated user)
        {
            _user = user;
            _gateway = service;
            _cache = cache;
            this.InitializeView(user);         
        }

        UserAuthenticated _user;
        
        public ObjectPermissionResult result = new ObjectPermissionResult();
        public ObjectPermissionParam param = new ObjectPermissionParam() { pObjectCode="",pObjectName=""};
        public List<ObjectPermissionResult> searchresult = new List<ObjectPermissionResult>();

        public ObjectPermissionLocalization texts = null; 

        public override async Task ClearSummaryValidation()
        {
            SummaryValidation = new List<InnerException>()
            {
                new InnerException("ObjectName",""),
                new InnerException("ObjectCode",""),              
            };
         
        }

        public override async Task InitializeModels()
        {

            await ClearSummaryValidation();

            this.texts = new ObjectPermissionLocalization();
            this.texts.FillTexts(await _cache.ListLocalizationTexts(), _user.LocalizationLanguage);

        }


        public override async Task Set()
        {
            ExecutionStatus = new OperationStatus(true);

            ObjectPermissionEntry entry = new ObjectPermissionEntry();

            entry.ObjectPermissionID = result.ObjectPermissionID;
            entry.ObjectName = result.ObjectName;
            entry.ObjectCode = result.ObjectCode;
            
            ObjectPermissionEntry ret = await _gateway.ObjectPermission.Set(entry);

            if (ret != null)
            {
                ExecutionStatus.Returns = ret;
            }
            else
            {
                ExecutionStatus.InnerExceptions = _gateway.ObjectPermission.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
                this.ShowSummaryValidation(ExecutionStatus.InnerExceptions);
            }


        }

        public override async Task Get(object id)
        {

            ExecutionStatus = new OperationStatus(true);

            result = await _gateway.ObjectPermission.Get(id.ToString());

            if (result == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.ObjectPermission.GetInnerExceptions(ref ExecutionStatus.Error);
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
            result = new ObjectPermissionResult();
           
        }

        public override async Task Search()
        {

            ExecutionStatus = new OperationStatus(true);

            searchresult = await _gateway.ObjectPermission.Search(param);

            if (searchresult == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.ObjectPermission.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }

        }

    }
}
