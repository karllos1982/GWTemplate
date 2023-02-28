using GW.Common;
using GW.Membership.Models;
using Template.Gateway;

namespace Template.ViewModel
{
    public class RoleViewModel : BaseViewModel
    {

        private MembershipGateway _gateway;
        private DataCacheGateway _cache;

        public RoleViewModel(MembershipGateway service, DataCacheGateway cache,
            UserAuthenticated user)
        {
            _user = user;
            _gateway = service;
            _cache = cache;
            this.InitializeView(user);
        }

        UserAuthenticated _user;

        public RoleResult result = new RoleResult();
        public RoleParam param = new RoleParam() { };
        public List<RoleResult> searchresult = new List<RoleResult>();

        public RoleLocalization texts = null;

        public override async Task ClearSummaryValidation()
        {
            SummaryValidation = new List<InnerException>()
            {
                new InnerException("RoleName",""),                
                new InnerException("IsActive", ""),
            };
         
        }

        public override async Task InitializeModels()
        {

            await ClearSummaryValidation();

            this.texts = new RoleLocalization();
            this.texts.FillTexts(await _cache.ListLocalizationTexts(), _user.LocalizationLanguage);
        }


        public override async Task Set()
        {
            ExecutionStatus = new OperationStatus(true);

            RoleEntry entry = new RoleEntry();

            entry.RoleID = result.RoleID;
            entry.RoleName = result.RoleName;            
            entry.IsActive = result.IsActive;

            RoleEntry ret = await _gateway.Role.Set(entry);

            if (ret != null)
            {
                ExecutionStatus.Returns = ret;
            }
            else
            {
                ExecutionStatus.InnerExceptions = _gateway.Role.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
                this.ShowSummaryValidation(ExecutionStatus.InnerExceptions);
            }


        }

        public override async Task Get(object id)
        {

            ExecutionStatus = new OperationStatus(true);

            result = await _gateway.Role.Get(id.ToString());

            if (result == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.Role.GetInnerExceptions(ref ExecutionStatus.Error);
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
            result = new RoleResult();
            result.IsActive = true;
        }

        public override async Task Search()
        {

            ExecutionStatus = new OperationStatus(true);

            searchresult = await _gateway.Role.Search(param);

            if (searchresult == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.Role.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }

        }

    }
}
