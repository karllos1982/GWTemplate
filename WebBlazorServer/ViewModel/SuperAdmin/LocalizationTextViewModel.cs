using GW.Common;
using GW.Membership.Models;
using Template.Gateway;
using WebBlazorServer.Pages.SuperAdmin;

namespace Template.ViewModel
{
    public class LocalizationTextViewModel : BaseViewModel
    {

        private MembershipGateway _gateway;
        private DataCacheGateway _cache;

        public LocalizationTextViewModel(MembershipGateway service, DataCacheGateway cache,
            UserAuthenticated user)
        {
            _user = user;
            _gateway = service;
            _cache = cache;
            this.InitializeView(user);         
        }

        UserAuthenticated _user;
        
        public LocalizationTextResult result = new LocalizationTextResult();
        public LocalizationTextParam param = new LocalizationTextParam() ;
        public List<LocalizationTextResult> searchresult = new List<LocalizationTextResult>();

        public LocalizationTextLocalization texts = null;

        public override async Task ClearSummaryValidation()
        {
            SummaryValidation = new List<InnerException>()
            {
                new InnerException("Language",""),
                new InnerException("Code",""),
                new InnerException("Name",""),
                new InnerException("Text","")
            };
          
        }

        public override async Task InitializeModels()
        {

            await ClearSummaryValidation();

            this.texts = new LocalizationTextLocalization();
            this.texts.FillTexts(await _cache.ListLocalizationTexts(), _user.LocalizationLanguage);
        }


        public override async Task Set()
        {
            ExecutionStatus = new OperationStatus(true);

            LocalizationTextEntry entry = new LocalizationTextEntry();

            entry.LocalizationTextID = result.LocalizationTextID;
            entry.Language = result.Language;
            entry.Code = result.Code;
            entry.Name = result.Name;
            entry.Text = result.Text;
            
            LocalizationTextEntry ret = await _gateway.LocalizationText.Set(entry);

            if (ret != null)
            {
                ExecutionStatus.Returns = ret;
            }
            else
            {
                ExecutionStatus.InnerExceptions 
                    = _gateway.LocalizationText.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
                this.ShowSummaryValidation(ExecutionStatus.InnerExceptions);
            }


        }

        public override async Task Get(object id)
        {

            ExecutionStatus = new OperationStatus(true);

            result = await _gateway.LocalizationText.Get(id.ToString());

            if (result == null)
            {
                ExecutionStatus.InnerExceptions 
                    = _gateway.LocalizationText.GetInnerExceptions(ref ExecutionStatus.Error);
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
            result = new LocalizationTextResult();
           
        }

        public override async Task Search()
        {

            ExecutionStatus = new OperationStatus(true);

            searchresult = await _gateway.LocalizationText.Search(param);

            if (searchresult == null)
            {
                ExecutionStatus.InnerExceptions
                    = _gateway.LocalizationText.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }

        }

    }
}
