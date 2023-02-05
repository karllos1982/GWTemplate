using GW.Common;
using GW.Membership.Models;
using Template.Gateway;
using WebBlazorServer.Pages.SuperAdmin;

namespace Template.ViewModel
{
    public class InstanceViewModel : BaseViewModel
    {

        private MembershipGateway _gateway;

        public InstanceViewModel(MembershipGateway service,
            UserAuthenticated user)
        {
            _gateway = service;
            this.InitializeView(user);
        }

        UserAuthenticated _user;

        public InstanceResult result = new InstanceResult();
        public InstanceParam param = new InstanceParam() {  };
        public List<InstanceResult> searchresult = new List<InstanceResult>();


        public override async Task ClearSummaryValidation()
        {
            SummaryValidation = new List<InnerException>()
            {
                new InnerException("InstanceTypeName",""),
                new InnerException("InstanceName", ""),
                new InnerException("IsActive", ""),
            };

        }

        public override async Task InitializeModels()
        {

            await ClearSummaryValidation();

        }


        public override async Task Set()
        {
            ExecutionStatus = new OperationStatus(true);

            InstanceEntry entry = new InstanceEntry();

            entry.InstanceID = result.InstanceID;
            entry.InstanceName = result.InstanceName;
            entry.InstanceTypeName = result.InstanceTypeName;         
            entry.IsActive = result.IsActive;

            InstanceEntry ret = await _gateway.Instance.Set(entry);

            if (ret != null)
            {
                ExecutionStatus.Returns = ret;
            }
            else
            {
                ExecutionStatus.InnerExceptions = _gateway.Instance.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
                this.ShowSummaryValidation(ExecutionStatus.InnerExceptions);
            }


        }

        public override async Task Get(object id)
        {

            ExecutionStatus = new OperationStatus(true);

            result = await _gateway.Instance.Get(id.ToString());

            if (result == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.Instance.GetInnerExceptions(ref ExecutionStatus.Error);
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
            result = new InstanceResult();
            result.IsActive = 1; 
        }

        public override async Task Search()
        {

            ExecutionStatus = new OperationStatus(true);

            searchresult = await _gateway.Instance.Search(param);

            if (searchresult == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.Instance.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }

        }

    }
}
