using GW.Common;
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
            this.InitializeView(_user);
        }

        UserAuthenticated _user;


        public UserResult result = new UserResult();
        public NewUser newModel = new NewUser();
        public UserParam param = new UserParam();
        public List<UserResult> searchresult = new List<UserResult>();
        public List<RoleList> listRoles = new List<RoleList>();
        public List<InstanceList> listInstances = new List<InstanceList>();
        public UserRolesResult RoleSelected = new UserRolesResult();
        public UserInstancesResult InstanceSelected = new UserInstancesResult(); 

        public bool isUserActive { get; set; }
        public bool isUserLocked { get; set; }

        public override async Task ClearSummaryValidation()
        {
            SummaryValidation = new List<InnerException>()
            {
                new InnerException("InstanceID",""),
                new InnerException("RoleID",""),
                new InnerException("Email",""),
                new InnerException("UserName",""),
                new InnerException("Password",""),
                new InnerException("DefaultLanguage","")
            };

        }

        public override async Task InitializeModels()
        {

            await ClearSummaryValidation();

            await LoadRolesList();
            await LoadInstancesList(); 
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

        public async Task LoadInstancesList()
        {
            listInstances= new List<InstanceList>();

            ExecutionStatus = new OperationStatus(true);
            listInstances = await _gateway.Instance.List(new InstanceParam());

            if (listRoles == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.Instance.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }
            else
            {
                listInstances.Insert(0, new InstanceList() { InstanceID = 0,InstanceName = "Selecione uma Instancia" });
            }

        }


        public override async Task Set()
        {
            ExecutionStatus = new OperationStatus(true);

            UserEntry entry = new UserEntry();
            

            UserEntry ret = await _gateway.User.Set(entry);

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

            result = await _gateway.User.Get(id.ToString());

            if (result == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.User.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }
            else
            {
                this.isUserActive = result.IsActive;
                this.isUserLocked = result.IsLocked;
                RoleSelected = result.Roles[0];
                InstanceSelected = result.Instances[0];  
            }

        }

        public async Task CreateNew()
        {
            ExecutionStatus = new OperationStatus(true);

            UserEntry ret = await _gateway.User.CreateNewUser(newModel);

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

            state.UserID = result.UserID;
            state.ActiveValue = false;
            state.LockedValue = false;

            state.ActiveValue = this.isUserActive; 
            state.LockedValue = this.isUserLocked; 

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

        public  async Task AlterInstance()
        {
            ExecutionStatus = new OperationStatus(true);

            bool ret
               = await _gateway.User.AlterInstance(InstanceSelected);

            if (ret)
            {
                ExecutionStatus.Returns = ret;
            }
            else
            {
                ExecutionStatus.InnerExceptions = _gateway.User.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }

        }

        public async Task AlterRole()
        {
            ExecutionStatus = new OperationStatus(true);

            bool ret
               = await _gateway.User.AlterRole(RoleSelected);

            if (ret)
            {
                ExecutionStatus.Returns = ret;
            }
            else
            {
                ExecutionStatus.InnerExceptions = _gateway.User.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }

        }

    }
}
