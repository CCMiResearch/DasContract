using DasContract.Abstraction.Processes;
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

        public RemoveRoleCommand(IContractManager contractManager, ProcessRole removedRole) : base(contractManager)
        {
            RemovedRole = removedRole;
        }

        public override void Execute()
        {
            ContractManager.RemoveRole(RemovedRole);
        }

        public override void Undo()
        {
            ContractManager.AddRole(RemovedRole);
        }
    }
}
