using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.UndoRedo
{
    public class UsersRolesManager
    {
        private IContractManager _contractManager;

        //Commands that have been done "in the past" and can be undone
        private Stack<ContractCommand> UndoableCommands { get; set; } = new Stack<ContractCommand>();
        //Commands that been undone - they can be redone
        private Stack<ContractCommand> RedoableCommands { get; set; } = new Stack<ContractCommand>();

        public UsersRolesManager(IContractManager contractManager)
        {
            _contractManager = contractManager;
        }

        public void AddUser()
        {
            RedoableCommands.Clear();
            var addCommand = new AddUserCommand(_contractManager);
            addCommand.Execute();
            UndoableCommands.Push(addCommand);
        }

        public void RemoveUser(ProcessUser removedUser)
        {
            RedoableCommands.Clear();
            var removeCommand = new RemoveUserCommand(_contractManager, removedUser);
            removeCommand.Execute();
            UndoableCommands.Push(removeCommand);
        }

        public void AddRole()
        {
            RedoableCommands.Clear();
            var addCommand = new AddRoleCommand(_contractManager);
            addCommand.Execute();
            UndoableCommands.Push(addCommand);
        }

        public void RemoveRole(ProcessRole removedRole)
        {
            RedoableCommands.Clear();
            var removeCommand = new RemoveRoleCommand(_contractManager, removedRole);
            removeCommand.Execute();
            UndoableCommands.Push(removeCommand);
        }

        public void Undo() 
        {
            if(UndoableCommands.Count > 0)
            {
                var undoedCommand = UndoableCommands.Pop();
                undoedCommand.Undo();
                RedoableCommands.Push(undoedCommand);
            }
        }
        public void Redo() 
        { 
            if(RedoableCommands.Count > 0)
            {
                var redoedCommand = RedoableCommands.Pop();
                redoedCommand.Execute();
                UndoableCommands.Push(redoedCommand);
            }
        }
    }
}
