using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Components.Select2;
using DasContract.Editor.Web.Services.ContractManagement;
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
        protected IUserModelManager UserModelManager { get; set; }

        [Inject]
        protected UsersRolesFacade UsersRolesFacade { get; set; }

        protected IEnumerable<ProcessUser> FilteredUsers { get; set; } = new List<ProcessUser>();
        protected string UsersFilter { get; set; }

        protected IEnumerable<ProcessRole> FilteredRoles { get; set; } = new List<ProcessRole>();
        protected string RolesFilter { get; set; }

        [Inject]
        protected UserInputHandler UserInputHandler { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            UserInputHandler.KeyDown += HandleKeyDown;
            UserModelManager.UserRemoved += OnUserRemoved;
            UserModelManager.UserAdded += OnUserAdded;
            UserModelManager.RoleAdded += OnRoleAdded;
            UserModelManager.RoleRemoved += OnRoleRemoved;

            FilterUsers(UsersFilter);
            FilterRoles(RolesFilter);
        }

        public void Dispose()
        {
            UserInputHandler.KeyDown -= HandleKeyDown;
            UserModelManager.UserRemoved -= OnUserRemoved;
            UserModelManager.UserAdded -= OnUserAdded;
            UserModelManager.RoleAdded -= OnRoleAdded;
            UserModelManager.RoleRemoved -= OnRoleRemoved;
        }

        protected void FilterUsers(string keyword)
        {
            UsersFilter = keyword;
            if (string.IsNullOrWhiteSpace(keyword))
                FilteredUsers = UserModelManager.GetProcessUsers().ToList();
            else
            {
                FilteredUsers = UserModelManager.GetProcessUsers()
                    .Where(u => UserFilterPredicate(u, keyword)).ToList();
            }
        }

        protected void FilterRoles(string keyword)
        {
            RolesFilter = keyword;
            if (string.IsNullOrWhiteSpace(keyword))
                FilteredRoles = UserModelManager.GetProcessRoles();
            else
            {
                FilteredRoles = UserModelManager.GetProcessRoles()
                    .Where(r => RolesFilterPredicate(r, keyword));
            }
        }

        private static bool UserFilterPredicate(ProcessUser u, string keyword)
        {
            return (u.Address?.Contains(keyword, StringComparison.InvariantCultureIgnoreCase) ?? false)
                || (u.Description?.Contains(keyword, StringComparison.InvariantCultureIgnoreCase) ?? false)
                || (string.IsNullOrWhiteSpace(u.Name) ? true : u.Name.Contains(keyword, StringComparison.InvariantCultureIgnoreCase))
                || u.Roles.Any(r => r.Name.Contains(keyword, StringComparison.InvariantCultureIgnoreCase));
        }

        private static bool RolesFilterPredicate(ProcessRole r, string keyword)
        {
            return (r.Name?.Contains(keyword, StringComparison.InvariantCultureIgnoreCase) ?? true)
                || (r.Description?.Contains(keyword, StringComparison.InvariantCultureIgnoreCase) ?? false);
        }

        protected void OnUserRemoved(object sender, ProcessUser removedUser)
        {
            _select2Components.Remove(removedUser.Id);
            FilterUsers(UsersFilter);
        }

        protected void OnUserAdded(object sender, ProcessUser addedUser)
        {
            FilterUsers(UsersFilter);
            StateHasChanged();
        }

        protected void OnRoleRemoved(object sender, ProcessRole removedRole)
        {
            FilterRoles(RolesFilter);
        }

        protected void OnRoleAdded(object sender, ProcessRole addedRole)
        {
            FilterRoles(RolesFilter);
        }

        protected void OnRoleAssigned(string roleId, string userId)
        {
            UsersRolesFacade.OnUserRoleAssign(_select2Components[userId], roleId);
        }

        protected void OnRoleUnassigned(string roleId, string userId)
        {
            UsersRolesFacade.OnUserRoleUnassign(_select2Components[userId], roleId);
        }

        public void HandleKeyDown(object sender, KeyEvent e)
        {
            if (e.CtrlKey && e.Key == "z")
            {
                UsersRolesFacade.Undo();
                StateHasChanged();
            }
            else if (e.CtrlKey && e.Key == "y")
            {
                UsersRolesFacade.Redo();
                StateHasChanged();
            }
        }
    }
}
