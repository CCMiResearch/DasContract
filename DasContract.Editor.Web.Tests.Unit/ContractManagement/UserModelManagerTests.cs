using DasContract.Abstraction;
using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.ContractManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DasContract.Editor.Web.Tests.Unit.ContractManagement
{
    public class UserModelManagerTests
    {
        private readonly Contract _contract;
        private readonly IUserModelManager _userModelManager;
        public UserModelManagerTests()
        {
            _contract = new Contract();
            _userModelManager = new UserModelManager();
            _userModelManager.SetContract(_contract);
        }

        [Fact]
        public void AddNewUser_ShouldAdd()
        {
            var addedUser = _userModelManager.AddNewUser();

            Assert.Single(_contract.Users);
            Assert.Contains(addedUser, _contract.Users);
            Assert.Equal(_contract.Users, _userModelManager.GetProcessUsers());
        }

        [Fact]
        public void AddNewRole_ShouldAdd()
        {
            var addedRole = _userModelManager.AddNewRole();

            Assert.Single(_contract.Roles);
            Assert.Contains(addedRole, _contract.Roles);
            Assert.Equal(_contract.Roles, _userModelManager.GetProcessRoles());
        }

        [Fact]
        public void AddUser_ShouldAdd()
        {
            var addedUser = new ProcessUser { Id = "User1" };
            _userModelManager.AddUser(addedUser);

            Assert.Single(_contract.Users);
            Assert.Contains(addedUser, _contract.Users);
            Assert.Equal(_contract.Users, _userModelManager.GetProcessUsers());
        }

        [Fact]
        public void AddRole_ShouldAdd()
        {
            var addedRole = new ProcessRole { Id = "Role1" };
            _userModelManager.AddRole(addedRole);

            Assert.Single(_contract.Roles);
            Assert.Contains(addedRole, _contract.Roles);
            Assert.Equal(_contract.Roles, _userModelManager.GetProcessRoles());
        }
    }
}
