using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.UndoRedo
{
    public class AddUserCommand : ContractCommand
    {
        private ProcessUser AddedUser { get; set; }

        public AddUserCommand(IContractManager contractManager) : base(contractManager)
        {

        }

        public override void Execute()
        {
            if(AddedUser != null)
            {
                ContractManager.AddUser(AddedUser);
            }
            else {
                AddedUser = ContractManager.AddNewUser();
            }
        }

        public override void Undo()
        {
            ContractManager.RemoveUser(AddedUser);
        }
    }
}
