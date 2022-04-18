using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Components.Select2;
using DasContract.Editor.Web.Services.ContractManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.UndoRedo
{
    public class UsersRolesFacade
    {
        private IUserModelManager _userModelManager;

        //Commands that have been done "in the past" and can be undone
        private Stack<UserModelCommand> UndoableCommands { get; set; } = new Stack<UserModelCommand>();
        //Commands that been undone - they can be redone
        private Stack<UserModelCommand> RedoableCommands { get; set; } = new Stack<UserModelCommand>();

        private IDictionary<string, bool> AccordionStates { get; set; } = new Dictionary<string, bool>();

        public UsersRolesFacade(IUserModelManager userModelManager)
        {
            _userModelManager = userModelManager;
        }

        public bool GetAccordionState(string id)
        {
            if (AccordionStates.TryGetValue(id, out var state))
                return state;
            return false;
        }

        public void FlipAccordionState(string id)
        {
            if (!AccordionStates.TryGetValue(id, out var state))
                AccordionStates[id] = true;
            else
                AccordionStates[id] = !state;
        }

        public void OnUserRoleAssign(Select2<ProcessRole> select, string roleId)
        {
            RedoableCommands.Clear();
            var assignedRole = _userModelManager.GetProcessRoles().Where(r => r.Id == roleId).FirstOrDefault();
            var assignedCommand = new AssignRoleCommand(_userModelManager, assignedRole, select);
            UndoableCommands.Push(assignedCommand);
        }

        public void OnUserRoleUnassign(Select2<ProcessRole> select, string roleId)
        {
            RedoableCommands.Clear();
            var unassignedRole = _userModelManager.GetProcessRoles().Where(r => r.Id == roleId).FirstOrDefault();
            var unassignedCommand = new UnassignRoleCommand(_userModelManager, unassignedRole, select);
            UndoableCommands.Push(unassignedCommand);

        }

        public void OnUserAdd()
        {
            RedoableCommands.Clear();
            var addCommand = new AddUserCommand(_userModelManager);
            addCommand.Execute();
            FlipAccordionState(addCommand.GetUserId());
            UndoableCommands.Push(addCommand);
        }

        public void OnUserRemove(ProcessUser removedUser)
        {
            RedoableCommands.Clear();
            var removeCommand = new RemoveUserCommand(_userModelManager, removedUser);
            removeCommand.Execute();
            UndoableCommands.Push(removeCommand);
        }

        public void OnRoleAdd()
        {
            RedoableCommands.Clear();
            var addCommand = new AddRoleCommand(_userModelManager);
            addCommand.Execute();
            FlipAccordionState(addCommand.GetRoleId());
            UndoableCommands.Push(addCommand);
        }

        public void OnRoleRemove(ProcessRole removedRole, IDictionary<string, Select2<ProcessRole>> select2Components)
        {
            RedoableCommands.Clear();
            var filteredSelect2Components = select2Components.Values.Where(s => s.Selected.Contains(removedRole)).ToList();
            var removeCommand = new RemoveRoleCommand(_userModelManager, removedRole, filteredSelect2Components);
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
