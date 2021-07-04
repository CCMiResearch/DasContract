using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Components.Common;
using DasContract.Editor.Web.Services.Processes;
using DasContract.Editor.Web.Services.UndoRedo;
using DasContract.Editor.Web.Services.UserInput;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace DasContract.Editor.Web.Pages
{
    public partial class UsersEditor : ComponentBase, IDisposable
    {
        protected Dictionary<string, Select2<ProcessRole>> _select2Components = new Dictionary<string, Select2<ProcessRole>>();

        [Inject]
        protected IContractManager ContractManager { get; set; }

        [Inject]
        protected UsersRolesManager UsersRolesManager { get; set; }

        [Inject]
        protected UserInputHandler UserInputHandler { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            UserInputHandler.KeyDown += HandleKeyDown;
            ContractManager.UserRemoved += OnUserRemoved;
            ContractManager.RoleAdded += OnRoleAdded;
            ContractManager.RoleRemoved += OnRoleRemoved;
        }

        public void Dispose()
        {
            UserInputHandler.KeyDown -= HandleKeyDown;
            ContractManager.UserRemoved -= OnUserRemoved;
            ContractManager.RoleAdded -= OnRoleAdded;
            ContractManager.RoleRemoved -= OnRoleRemoved;
        }

        protected void OnUserRemoved(object sender, ProcessUser removedUser)
        {
            _select2Components.Remove(removedUser.Id);
        }

        protected void OnRoleRemoved(object sender, ProcessRole removedRole)
        {
        }

        protected void OnRoleAdded(object sender, ProcessRole addedRole)
        {
        }

        protected void OnRoleAssigned(string roleId, string userId)
        {
            UsersRolesManager.UserRoleAssigned(_select2Components[userId], roleId);
        }

        protected void OnRoleUnassigned(string roleId, string userId)
        {
            UsersRolesManager.UserRoleUnassigned(_select2Components[userId], roleId);
        }

        public void HandleKeyDown(object sender, KeyEvent e)
        {
            if (e.CtrlKey && e.Key == "z")
            {
                UsersRolesManager.Undo();
                StateHasChanged();
            }
            else if (e.CtrlKey && e.Key == "y")
            {
                UsersRolesManager.Redo();
                StateHasChanged();
            }
        }
    }
}
