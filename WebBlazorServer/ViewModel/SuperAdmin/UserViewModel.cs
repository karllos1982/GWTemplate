using GW.Core.Common;
using GW.Membership.Models;
using Template.Gateway;

namespace Template.ViewModel
{
    public class UserViewModel : BaseViewModel
    {

        private MembershipGateway _gateway;

        public UserViewModel(MembershipGateway service,
            UserAuthenticated user)
        {
            _user = user;
            _gateway = service;
            this.InitializeView(_user,"SYSUSER",false);
        }

        UserAuthenticated _user;


        public UserModel model = new UserModel();
        public NewUser newModel = new NewUser();
        public UserParam param = new UserParam();
        public List<UserSearchResult> searchresult = new List<UserSearchResult>();
        public List<RoleList> listRoles = new List<RoleList>();

        public bool isUserActive { get; set; }
        public bool isUserLocked { get; set; }

        public override async Task ClearSummaryValidation()
        {
            SummaryValidation = new List<InnerException>()
            {
                new InnerException("RoleID",""),
                new InnerException("Email",""),
                new InnerException("UserName",""),
                new InnerException("Password","")
            };

        }

        public override async Task InitializeModels()
        {

            await ClearSummaryValidation();

            await LoadRolesList();
        }

        public async Task LoadRolesList()
        {
            listRoles = new List<RoleList>();

            ExecutionStatus = new OperationStatus(true);
            listRoles = await _gateway.Role.List(new RoleParam());

            if (listRoles == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.Role.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }
            else
            {
                listRoles.Insert(0, new RoleList() { RoleID = 0, RoleName = "Selecione uma Role" });
            }

        }

        public override async Task Set()
        {
            ExecutionStatus = new OperationStatus(true);

            UserModel ret = await _gateway.User.Set(model);

            if (ret != null)
            {
                ExecutionStatus.Returns = ret;
            }
            else
            {
                ExecutionStatus.InnerExceptions = _gateway.User.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }


        }

        public override async Task Get(object id)
        {

            ExecutionStatus = new OperationStatus(true);

            model = await _gateway.User.Get(id.ToString());

            if (model == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.User.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }
            else
            {
                this.isUserActive = Convert.ToBoolean(model.IsActive);
                this.isUserLocked = Convert.ToBoolean(model.IsLocked);
            }

        }

        public async Task CreateNew()
        {
            ExecutionStatus = new OperationStatus(true);

            UserModel ret = await _gateway.User.CreateNewUser(newModel);

            if (ret != null)
            {
                ExecutionStatus.Returns = ret;
            }
            else
            {
                ExecutionStatus.InnerExceptions = _gateway.User.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
                this.ShowSummaryValidation(ExecutionStatus.InnerExceptions);
            }

        }

        public async Task ChangeState()
        {
            UserChangeState state = new UserChangeState();
            ExecutionStatus = new OperationStatus(true);

            state.UserID = model.UserID;
            state.ActiveValue = 0;
            state.LockedValue = 0;

            if (this.isUserActive) { state.ActiveValue = 1; }
            if (this.isUserLocked) { state.LockedValue = 1; }

            await _gateway.User.ChangeState(state);

            if (!_gateway.User.APIResponse.StatusOK)
            {
                ExecutionStatus.InnerExceptions = _gateway.User.GetInnerExceptions(ref ExecutionStatus.Error);
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
            newModel = new NewUser();
        }

        public override async Task Search()
        {

            ExecutionStatus = new OperationStatus(true);

            searchresult = await _gateway.User.Search(param);

            if (searchresult == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.User.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }

        }

    }
}
