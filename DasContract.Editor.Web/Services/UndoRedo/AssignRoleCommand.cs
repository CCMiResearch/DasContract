﻿using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Components.Common;
using DasContract.Editor.Web.Services.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.UndoRedo
{
    public class AssignRoleCommand : ContractCommand
    {
        protected ProcessRole AssignedRole { get; set; }
        protected Select2<ProcessRole> RoleSelect { get; set; }

        public AssignRoleCommand(IContractManager contractManager, ProcessRole assignedRole, Select2<ProcessRole> roleSelect) : base(contractManager)
        {
            AssignedRole = assignedRole;
            RoleSelect = roleSelect;
        }

        public override void Execute()
        {
            RoleSelect.SelectItem(AssignedRole);
        }

        public override void Undo()
        {
            RoleSelect.UnselectItem(AssignedRole);
        }
    }
}