using DasContract.Editor.Web.Services.ContractManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.UndoRedo
{
    public abstract class UserModelCommand
    {
        protected IUserModelManager UserModelManager { get; set; }

        public UserModelCommand(IUserModelManager userModelManager)
        {
            UserModelManager = userModelManager;
        }

        public abstract void Execute();
        public abstract void Undo();
    }
}
