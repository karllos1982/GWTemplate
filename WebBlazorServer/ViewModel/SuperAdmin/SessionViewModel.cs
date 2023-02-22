using GW.Common;
using GW.Membership.Models;
using Template.Gateway;
using WebBlazorServer.Localization;

namespace Template.ViewModel
{
    public class SessionViewModel : BaseViewModel
    {

        private MembershipGateway _gateway;
        private DataCacheGateway _cache;

        public SessionViewModel(MembershipGateway service, DataCacheGateway cache,
            UserAuthenticated user)
        {
            _user = user;
            _gateway = service;
            _cache = cache;
            this.InitializeView();
        }

        UserAuthenticated _user;


        public SessionLogEntry entry = new SessionLogEntry();
        public SessionLogResult result = new SessionLogResult();
        public SessionLogParam param = new SessionLogParam();
        public List<SessionLogResult> searchresult = new List<SessionLogResult>();

        public SessionLogLocalization texts = null; 

        public override async Task ClearSummaryValidation()
        {
            SummaryValidation = new List<InnerException>()
            {

            };

            this.texts = new SessionLogLocalization();
            this.texts.FillTexts(await _cache.ListLocalizationTexts(), _user.LocalizationLanguage);

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

            result = await _gateway.Session.Get(id.ToString());

            if (result == null)
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
