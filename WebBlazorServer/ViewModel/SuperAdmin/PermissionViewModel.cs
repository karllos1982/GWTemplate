﻿using GW.Core.Common;
using GW.Membership.Models;
using Microsoft.AspNetCore.Components.Web;
using System.Runtime.InteropServices;
using Template.Gateway;
using WebBlazorServer.Pages.SuperAdmin;

namespace Template.ViewModel
{
    public class PermissionViewModel : BaseViewModel
    {

        private MembershipGateway _gateway;
        private DataCacheGateway _cacheGateway; 

        public PermissionViewModel(MembershipGateway service,
            DataCacheGateway cache ,  UserAuthenticated user)
        {       
            _gateway = service;
            _cacheGateway = cache;  
            this.InitializeView(user);         
        }

    
        public PermissionModel model = new PermissionModel();
        public PermissionParam param = new PermissionParam() { };
        public List<PermissionSearchResult> searchresult = new List<PermissionSearchResult>();
        public List<RoleList> listRoles = new List<RoleList>();
        public List<UserList> listUsers = new List<UserList>();
        public List<ObjectPermissionList> listObject = new List<ObjectPermissionList>();
        public List<UIBaseItem> listTypeGrant = new List<UIBaseItem>();
        public List<SelectBaseItem> listPermissionValue = new List<SelectBaseItem>();
        
        public string pTypeGrant = "";

        public override async Task ClearSummaryValidation()
        {
            SummaryValidation = new List<InnerException>()
            {
                new InnerException("ObjectPermissionID",""),
                new InnerException("RoleID",""),
                new InnerException("UserID","")
            };

        }

        public override async Task InitializeModels()
        {

            await ClearSummaryValidation();

            await LoadRolesList();
            await LoadUsersList();
            await LoadObjectPermissionList();
            LoadTypeGrantList();
            LoadListPermissionValue();
        }

        public void LoadTypeGrantList()
        {
            listTypeGrant.Add(new UIBaseItem() { ID = "R", Value = "ByRole" });
            listTypeGrant.Add(new UIBaseItem() { ID = "U", Value = "ByUser" });
        }

        public void LoadListPermissionValue()
        {
            listPermissionValue.Add(new SelectBaseItem() {  Value = 1, Text="Allowed" });
            listPermissionValue.Add(new SelectBaseItem() {  Value = -1, Text = "Denied" });
        }


        public async Task LoadRolesList()
        {
            listRoles = new List<RoleList>();

            ExecutionStatus = new OperationStatus(true);
            listRoles = await _cacheGateway.ListRoles();

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

        public async Task LoadUsersList()
        {
            listUsers = new List<UserList>();

            ExecutionStatus = new OperationStatus(true);
            listUsers = await _gateway.User.List(new UserParam());

            if (listUsers == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.Role.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }
            else
            {
                listUsers.Insert(0, new UserList() { UserID = 0, UserName = "Selecione um Usuário" });
            }

        }

        public async Task LoadObjectPermissionList()
        {
            listObject = new List<ObjectPermissionList>();

            ExecutionStatus = new OperationStatus(true);
            listObject = await _gateway.ObjectPermission.List(new ObjectPermissionParam());

            if (listObject == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.Role.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }
            else
            {
                listObject.Insert(0, new ObjectPermissionList() { ObjectPermissionID = 0, ObjectName = "Selecione um Objeto de Permissão" });
            }

        }

        public override async Task Set()
        {
            ExecutionStatus = new OperationStatus(true);

            if (model.UserID == 0) { model.UserID = null; }
            if (model.RoleID == 0) { model.RoleID = null; }

            PermissionModel ret = await _gateway.Permission.Set(model);

            if (ret != null)
            {
                ExecutionStatus.Returns = ret;
            }
            else
            {
                ExecutionStatus.InnerExceptions = _gateway.Permission.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
                this.ShowSummaryValidation(ExecutionStatus.InnerExceptions);
            }


        }

        public override async Task Get(object id)
        {

            ExecutionStatus = new OperationStatus(true);

            model = await _gateway.Permission.Get(id.ToString());

            if (model == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.Permission.GetInnerExceptions(ref ExecutionStatus.Error);
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
            model = new PermissionModel();
            model.PermissionID = 0;
            model.ReadStatus = 1;
            model.SaveStatus = 1;
            model.DeleteStatus = 1;
            model.TypeGrant = "";

        }

        public override async Task Search()
        {

            ExecutionStatus = new OperationStatus(true);

            searchresult = await _gateway.Permission.Search(param);

            if (searchresult == null)
            {
                ExecutionStatus.InnerExceptions = _gateway.Permission.GetInnerExceptions(ref ExecutionStatus.Error);
                ExecutionStatus.Status = false;
            }

        }

        public bool GetDisabledStatus(string expected, string typegrant )
        {
            bool ret = false;

            if (typegrant != "")
            {
                if (expected != typegrant)
                {
                    ret = true;
                }
            }

            return ret;
        }

    }
}