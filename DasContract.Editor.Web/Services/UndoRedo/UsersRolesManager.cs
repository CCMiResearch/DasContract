using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Components.Select2;
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

        private IDictionary<string, bool> AccordionStates { get; set; } = new Dictionary<string, bool>();

        public UsersRolesManager(IContractManager contractManager)
        {
            _contractManager = contractManager;
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

        public void UserRoleAssigned(Select2<ProcessRole> select, string roleId)
        {
            Console.WriteLine("Assigned user role");
            RedoableCommands.Clear();
            var assignedRole = _contractManager.GetProcessRoles().Where(r => r.Id == roleId).FirstOrDefault();
            var assignedCommand = new AssignRoleCommand(_contractManager, assignedRole, select);
            UndoableCommands.Push(assignedCommand);
        }

        public void UserRoleUnassigned(Select2<ProcessRole> select, string roleId)
        {
            Console.WriteLine("Unassigned user role");
            RedoableCommands.Clear();
            var unassignedRole = _contractManager.GetProcessRoles().Where(r => r.Id == roleId).FirstOrDefault();
            var unassignedCommand = new UnassignRoleCommand(_contractManager, unassignedRole, select);
            UndoableCommands.Push(unassignedCommand);

        }

        public void AddUser()
        {
            RedoableCommands.Clear();
            var addCommand = new AddUserCommand(_contractManager);
            addCommand.Execute();
            FlipAccordionState(addCommand.GetUserId());
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
            FlipAccordionState(addCommand.GetRoleId());
            UndoableCommands.Push(addCommand);
        }

        public void RemoveRole(ProcessRole removedRole, IDictionary<string, Select2<ProcessRole>> select2Components)
        {
            RedoableCommands.Clear();
            var filteredSelect2Components = select2Components.Values.Where(s => s.Selected.Contains(removedRole)).ToList();
            var removeCommand = new RemoveRoleCommand(_contractManager, removedRole, filteredSelect2Components);
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
