using DasContract.Abstraction;
using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.BpmnEvents.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.ContractManagement
{
    public class UserModelManager : IUserModelManager
    {
        private Contract Contract { get; set; }

        public event EventHandler<ProcessUser> UserRemoved;
        public event EventHandler<ProcessRole> RoleRemoved;
        public event EventHandler<ProcessUser> UserAdded;
        public event EventHandler<ProcessRole> RoleAdded;

        public void SetContract(Contract contract)
        {
            Contract = contract;
        }

        public IList<ProcessUser> GetProcessUsers()
        {
            return Contract.Users;
        }

        public IList<ProcessRole> GetProcessRoles()
        {
            return Contract.Roles;
        }

        public ProcessUser AddNewUser()
        {
            var user = new ProcessUser { Id = Guid.NewGuid().ToString() };
            Contract.Users.Add(user);
            UserAdded?.Invoke(this, user);
            return user;
        }

        public void AddUser(ProcessUser user)
        {
            if (Contract.Users.Any(u => user.Id == u.Id))
            {
                throw new DuplicateIdException($"Contract already contains user id {user.Id}");
            }
            Contract.Users.Add(user);
            UserAdded?.Invoke(this, user);
        }

        public void RemoveUser(ProcessUser user)
        {
            if (!Contract.Users.Contains(user))
            {
                throw new InvalidIdException($"User id {user.Id} could not be removed, contract does not contain user");
            }
            Contract.Users.Remove(user);
            UserRemoved?.Invoke(this, user);
        }

        public ProcessRole AddNewRole()
        {
            var role = new ProcessRole { Id = Guid.NewGuid().ToString() };
            Contract.Roles.Add(role);
            RoleAdded?.Invoke(this, role);
            return role;
        }

        public void AddRole(ProcessRole role)
        {
            Contract.Roles.Add(role);
            RoleAdded?.Invoke(this, role);
        }
        public void RemoveRole(ProcessRole role)
        {
            if (!Contract.Roles.Contains(role))
            {
                throw new InvalidIdException($"User id {role.Id} could not be removed, contract does not contain user");
            }
            Contract.Roles.Remove(role);
            RoleRemoved?.Invoke(this, role);
        }

    }
}
