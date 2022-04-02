using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.ContractManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.UndoRedo
{
    public class AddRoleCommand : ContractCommand
    {
        private ProcessRole AddedRole { get; set; }

        public AddRoleCommand(IUserModelManager userModelManager) : base(userModelManager)
        {

        }
        public override void Execute()
        {
            if (AddedRole != null)
            {
                UserModelManager.AddRole(AddedRole);
            }
            else
            {
                AddedRole = UserModelManager.AddNewRole();
            }
        }

        public override void Undo()
        {
            UserModelManager.RemoveRole(AddedRole);
        }

        public string GetRoleId()
        {
            return AddedRole?.Id;
        }
    }
}
