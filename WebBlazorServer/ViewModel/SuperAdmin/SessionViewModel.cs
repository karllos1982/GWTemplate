using GW.Core.Common;
using GW.Membership.Models;
using Template.Gateway;


namespace Template.ViewModel
{
    public class SessionViewModel : BaseViewModel
    {

        private MembershipGateway _gateway;

        public SessionViewModel(MembershipGateway service,
            UserAuthenticated user)
        {
            _user = user;
            _gateway = service;
            this.InitializeView();
        }

        UserAuthenticated _user;


        public SessionModel model = new SessionModel();
        public SessionParam param = new SessionParam();
        public List<SessionSearchResult> searchresult = new List<SessionSearchResult>();


        public override async Task ClearSummaryValidation()
        {
            SummaryValidation = new List<InnerException>()
            {

            };

        }

        public override async Task InitializeModels()
        {
            param.pDate_Start = DateTime.Now.AddDays(-7);
            param.pData_End = DateTime.Now;

            await ClearSummaryValidation();

        }

        public override async Task Set()
        {


        }

        public override async Task Get(object id)
        {

            ExecutionStatus = new OperationStatus(true);

            model = await _gateway.Session.Get(id.ToString());

            if (model == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.Session.GetInnerExceptions(ref ExecutionStatus.Error);
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
        }

        public override async Task Search()
        {

            ExecutionStatus = new OperationStatus(true);

            searchresult = await _gateway.Session.Search(param);

            if (searchresult == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.Session.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }

        }

    }
}
