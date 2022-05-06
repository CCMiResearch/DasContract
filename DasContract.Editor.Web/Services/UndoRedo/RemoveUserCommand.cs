using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.ContractManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.UndoRedo
{
    public class RemoveUserCommand : UserModelCommand
    {
        private ProcessUser RemovedUser { get; set; }

        public RemoveUserCommand(IUserModelManager userModelManager, ProcessUser removedUser) : base(userModelManager)
        {
            RemovedUser = removedUser;
        }

        public override void Execute()
        {
            UserModelManager.RemoveUser(RemovedUser);
        }

        public override void Undo()
        {
            UserModelManager.AddUser(RemovedUser);
        }
    }
}
