using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.UndoRedo
{
    public class AddRoleCommand : ContractCommand
    {
        private ProcessRole AddedRole { get; set; }

        public AddRoleCommand(IContractManager contractManager) : base(contractManager)
        {

        }
        public override void Execute()
        {
            if (AddedRole != null)
            {
                ContractManager.AddRole(AddedRole);
            }
            else
            {
                AddedRole = ContractManager.AddNewRole();
            }
        }

        public override void Undo()
        {
            ContractManager.RemoveRole(AddedRole);
        }
    }
}
