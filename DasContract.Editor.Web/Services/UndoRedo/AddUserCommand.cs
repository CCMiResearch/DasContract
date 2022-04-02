using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.ContractManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.UndoRedo
{
    public class AddUserCommand : ContractCommand
    {
        private ProcessUser AddedUser { get; set; }

        public AddUserCommand(IUserModelManager userModelManager) : base(userModelManager)
        {

        }

        public override void Execute()
        {
            if(AddedUser != null)
            {
                UserModelManager.AddUser(AddedUser);
            }
            else {
                AddedUser = UserModelManager.AddNewUser();
            }
        }

        public override void Undo()
        {
            UserModelManager.RemoveUser(AddedUser);
        }

        public string GetUserId()
        {
            return AddedUser?.Id;
        }
    }
}
