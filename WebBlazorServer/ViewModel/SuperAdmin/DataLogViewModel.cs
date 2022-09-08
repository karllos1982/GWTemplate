using GW.Core.Common;
using GW.Membership.Models;
using Template.Gateway;

namespace Template.ViewModel
{
    public class DataLogViewModel : BaseViewModel
    {

        private MembershipGateway _gateway;

        public DataLogViewModel(MembershipGateway service,
            UserAuthenticated user)
        {
            _user = user;
            _gateway = service;
            this.InitializeView();
        }

        UserAuthenticated _user;


        public DataLogModel model = new DataLogModel();
        public DataLogParam param = new DataLogParam();
        public List<DataLogSearchResult> searchresult = new List<DataLogSearchResult>();
        public List<TipoOperacaoValueModel> listTipoOperacao = new List<TipoOperacaoValueModel>();
        public List<TabelasValueModel> listTabelas = new List<TabelasValueModel>();

        public List<DataLogItem> logold_content;
        public List<DataLogItem> logcurrent_content;
        public List<DataLogTimelineModel> timeline = null;
        public bool ShowTimeline = false;

        public override async Task ClearSummaryValidation()
        {
            SummaryValidation = new List<InnerException>()
            {

            };

        }

        public override async Task InitializeModels()
        {

            await ClearSummaryValidation();

        }

        public async Task LoadTipoOperacaoList(DataCacheGateway cache)
        {

            ExecutionStatus = new OperationStatus(true);
            listTipoOperacao = await cache.ListTipoOperacao();

            if (listTipoOperacao != null)
            {
                listTipoOperacao.Insert(0, new TipoOperacaoValueModel() { Value = "0", Text = "Todas" });
            }
            else
            {
                listTipoOperacao = new List<TipoOperacaoValueModel>();
            }

        }

        public async Task LoadTabelaList(DataCacheGateway cache)
        {

            ExecutionStatus = new OperationStatus(true);
            listTabelas = await cache.ListTabelas();

            if (listTabelas != null)
            {
                listTabelas.Insert(0, new TabelasValueModel() { Value = "0", Text = "Todas" });
            }
            else
            {
                listTabelas = new List<TabelasValueModel>();
            }

        }

        public override async Task Set()
        {


        }

        public override async Task Get(object id)
        {

            ExecutionStatus = new OperationStatus(true);

            model = await _gateway.DataLog.Get(id.ToString());

            if (model == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.DataLog.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }

        }

        public async Task GetTimeline()
        {

            ExecutionStatus = new OperationStatus(true);

            timeline = await _gateway.DataLog.GetTimeLine(model.ID.ToString());

            if (timeline == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.DataLog.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }

        }

        public override void BackToSearch()
        {
            this.BaseBack();
            this.ShowTimeline = false;
        }

        public override void InitEdit()
        {
            this.BaseInitEdit();

            if (model != null)
            {
                GetDataLogContent(model);

            }

        }

        public void GetDataLogContent(DataLogModel log)
        {
            logold_content = new List<DataLogItem>();
            logcurrent_content = new List<DataLogItem>();

            if (log.LogOldData != null)
            {
                if (log.LogOldData != "")
                {
                    logold_content = _gateway.DataLog
                        .GetDataLogItems(log.LogOldData);
                }
            }

            if (log.LogCurrentData != null)
            {
                if (log.LogCurrentData != "")
                {
                    logcurrent_content = _gateway.DataLog
                        .GetDataLogItems(log.LogCurrentData);
                }
            }

            if (log.Operation == "U")
            {
                _gateway.DataLog
                    .GetDataLogDiff(ref logold_content, ref logcurrent_content);
            }

        }

        public override void InitNew()
        {
            this.BaseInitNew();
        }

        public override async Task Search()
        {

            ExecutionStatus = new OperationStatus(true);

            searchresult = await _gateway.DataLog.Search(param);

            if (searchresult == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.DataLog.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }

        }

    }

}
