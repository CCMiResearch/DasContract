using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Components.Select2;
using DasContract.Editor.Web.Services.ContractManagement;
using DasContract.Editor.Web.Services.UndoRedo;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DasContract.Editor.Web.Tests.Unit.UndoRedo
{
    public class UndoRedoTests
    {
        private Mock<IUserModelManager> _userModelManagerMock;
        private UsersRolesFacade _usersRolesFacade;

        public UndoRedoTests()
        {
            _userModelManagerMock = new Mock<IUserModelManager>();
            _usersRolesFacade = new UsersRolesFacade(_userModelManagerMock.Object);
        }

        [Fact]
        public void AddUserAndUndoRedo_ShouldMatch()
        {
            var addedUser = new ProcessUser { Id = "USER1" };

            _userModelManagerMock.Setup(x => x.AddNewUser()).Returns(addedUser).Verifiable();
            _userModelManagerMock.Setup(x => x.RemoveUser(addedUser)).Verifiable();
            _userModelManagerMock.Setup(x => x.AddUser(addedUser)).Verifiable();

            _usersRolesFacade.OnUserAdd();
            _userModelManagerMock.Verify(x => x.AddNewUser(), Times.Once);

            _usersRolesFacade.Undo();
            _userModelManagerMock.Verify(x => x.RemoveUser(addedUser), Times.Once);

            _usersRolesFacade.Redo();
            _userModelManagerMock.Verify(x => x.AddUser(addedUser), Times.Once);
        }

        [Fact]
        public void RemoveUserAndUndo_ShouldMatch()
        {
            var addedUser = new ProcessUser { Id = "USER1" };

            _userModelManagerMock.Setup(x => x.AddNewUser()).Returns(addedUser).Verifiable();
            _userModelManagerMock.Setup(x => x.AddUser(addedUser)).Verifiable();
            _userModelManagerMock.Setup(x => x.AddUser(addedUser)).Verifiable();

            _usersRolesFacade.OnUserAdd();
            _userModelManagerMock.Verify(x => x.AddNewUser(), Times.Once);

            _usersRolesFacade.OnUserRemove(addedUser);
            _userModelManagerMock.Verify(x => x.RemoveUser(addedUser), Times.Once);

            _usersRolesFacade.Undo();
            _userModelManagerMock.Verify(x => x.AddUser(addedUser), Times.Once);

        }

        [Fact]
        public void AddRoleAndUndoRedo_ShouldMatch()
        {
            var addedRole = new ProcessRole { Id = "ROLE1" };

            _userModelManagerMock.Setup(x => x.AddNewRole()).Returns(addedRole).Verifiable();
            _userModelManagerMock.Setup(x => x.RemoveRole(addedRole)).Verifiable();
            _userModelManagerMock.Setup(x => x.AddRole(addedRole)).Verifiable();

            _usersRolesFacade.OnRoleAdd();
            _userModelManagerMock.Verify(x => x.AddNewRole(), Times.Once);

            _usersRolesFacade.Undo();
            _userModelManagerMock.Verify(x => x.RemoveRole(addedRole), Times.Once);

            _usersRolesFacade.Redo();
            _userModelManagerMock.Verify(x => x.AddRole(addedRole), Times.Once);
        }

        [Fact]
        public void RemoveRoleAndUndo_ShouldMatch()
        {
            var addedRole = new ProcessRole { Id = "ROLE1" };

            _userModelManagerMock.Setup(x => x.AddNewRole()).Returns(addedRole).Verifiable();
            _userModelManagerMock.Setup(x => x.RemoveRole(addedRole)).Verifiable();
            _userModelManagerMock.Setup(x => x.AddRole(addedRole)).Verifiable();

            _usersRolesFacade.OnRoleAdd();
            _userModelManagerMock.Verify(x => x.AddNewRole(), Times.Once);

            _usersRolesFacade.OnRoleRemove(addedRole, new Dictionary<string, Select2<ProcessRole>>());
            _userModelManagerMock.Verify(x => x.RemoveRole(addedRole), Times.Once);

            _usersRolesFacade.Undo();
            _userModelManagerMock.Verify(x => x.AddRole(addedRole), Times.Once);
        }

        [Fact]
        public void AssignRoleAndUndo_ShouldMatch()
        {
            var addedRole = new ProcessRole { Id = "ROLE1" };
            var addedUser = new ProcessUser { Id = "USER1"};

            var select2Mock = new Mock<Select2<ProcessRole>>();
            select2Mock.Setup(x => x.UnselectItem(addedRole)).Verifiable();

            _userModelManagerMock.Setup(x => x.AddNewRole()).Returns(addedRole).Verifiable();
            _userModelManagerMock.Setup(x => x.AddNewUser()).Returns(addedUser).Verifiable();
            _userModelManagerMock.Setup(x => x.GetProcessRoles()).Returns(new List<ProcessRole>() { addedRole });


            _usersRolesFacade.OnRoleAdd();
            _userModelManagerMock.Verify(x => x.AddNewRole(), Times.Once);

            _usersRolesFacade.OnUserAdd();
            _userModelManagerMock.Verify(x => x.AddNewUser(), Times.Once);

            _usersRolesFacade.OnUserRoleAssign(select2Mock.Object, "ROLE1");
            _usersRolesFacade.Undo();
            select2Mock.Verify(x => x.UnselectItem(addedRole), Times.Once);
        }
    }
}
