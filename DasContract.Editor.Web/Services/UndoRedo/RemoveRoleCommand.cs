using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Components.Select2;
using DasContract.Editor.Web.Services.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.UndoRedo
{
    public class RemoveRoleCommand : ContractCommand
    {
        private ProcessRole RemovedRole { get; set; }
        private IEnumerable<Select2<ProcessRole>> RoleContainers { get; set; }

        public RemoveRoleCommand(IContractManager contractManager, ProcessRole removedRole, IEnumerable<Select2<ProcessRole>> roleContainers) : base(contractManager)
        {
            RoleContainers = roleContainers;
            RemovedRole = removedRole;
        }

        public override void Execute()
        {
            //Unselect the role in all users that contain it
            foreach (var roleContainer in RoleContainers)
            {
                roleContainer.UnselectItem(RemovedRole);
            }
            ContractManager.RemoveRole(RemovedRole);
        }

        public override void Undo()
        {
            ContractManager.AddRole(RemovedRole);
            foreach (var roleContainer in RoleContainers)
            {
                roleContainer.SelectItem(RemovedRole);
            }
        }
    }
}
