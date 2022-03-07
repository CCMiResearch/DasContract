using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.UndoRedo
{
    public class RemoveUserCommand : ContractCommand
    {
        private ProcessUser RemovedUser { get; set; }

        public RemoveUserCommand(IContractManager contractManager, ProcessUser removedUser) : base(contractManager)
        {
            RemovedUser = removedUser;
        }

        public override void Execute()
        {
            ContractManager.RemoveUser(RemovedUser);
        }

        public override void Undo()
        {
            ContractManager.AddUser(RemovedUser);
        }
    }
}
