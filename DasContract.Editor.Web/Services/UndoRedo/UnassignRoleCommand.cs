using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Components.Select2;
using DasContract.Editor.Web.Services.ContractManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.UndoRedo
{
    public class UnassignRoleCommand : ContractCommand
    {
        protected ProcessRole AssignedRole { get; set; }
        protected Select2<ProcessRole> RoleSelect { get; set; }

        public UnassignRoleCommand(IUserModelManager userModelManager, ProcessRole assignedRole, Select2<ProcessRole> roleSelect) : base(userModelManager)
        {
            AssignedRole = assignedRole;
            RoleSelect = roleSelect;
        }

        public override void Execute()
        {
            RoleSelect.UnselectItem(AssignedRole);
        }

        public override void Undo()
        {
            RoleSelect.SelectItem(AssignedRole);
        }
    }
}
