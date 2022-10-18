using GW.Core.Common;
using GW.Membership.Models;
using Template.Gateway;

namespace Template.ViewModel
{
    public class ObjectPermissionViewModel : BaseViewModel
    {

        private MembershipGateway _gateway;

        public ObjectPermissionViewModel(MembershipGateway service,
            UserAuthenticated user)
        {       
            _gateway = service;
            this.InitializeView(user, "SYSOBJECTPERMISSION", false);
        }

    
        public ObjectPermissionModel model = new ObjectPermissionModel();
        public ObjectPermissionParam param = new ObjectPermissionParam() { pObjectCode="",pObjectName=""};
        public List<ObjectPermissionSearchResult> searchresult = new List<ObjectPermissionSearchResult>();
   

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
           
        }

    
        public override async Task Set()
        {
            ExecutionStatus = new OperationStatus(true);

            ObjectPermissionModel ret = await _gateway.ObjectPermission.Set(model);

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

            model = await _gateway.ObjectPermission.Get(id.ToString());

            if (model == null)
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
            model = new ObjectPermissionModel();
           
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
