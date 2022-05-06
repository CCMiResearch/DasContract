using DasContract.Abstraction;
using DasContract.Abstraction.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.ContractManagement
{
    public interface IUserModelManager
    {
        event EventHandler<ProcessUser> UserRemoved;
        event EventHandler<ProcessRole> RoleRemoved;
        event EventHandler<ProcessUser> UserAdded;
        event EventHandler<ProcessRole> RoleAdded;

        void SetContract(Contract contract);
        ProcessUser AddNewUser();
        ProcessRole AddNewRole();
        void AddUser(ProcessUser user);
        void RemoveUser(ProcessUser user);
        void AddRole(ProcessRole role);
        void RemoveRole(ProcessRole role);
        IList<ProcessUser> GetProcessUsers();
        IList<ProcessRole> GetProcessRoles();
    }
}
