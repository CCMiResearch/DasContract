using DasContract.Abstraction.Processes.Tasks;
using DasContract.Editor.Web.Services.EditElement;
using Moq;
using System;
using System.Linq.Expressions;
using Xunit;

namespace DasContract.Editor.Web.Tests.Unit.EditElement
{
    public class EditElementTests
    {
        [Fact]
        public void AssignNewElement_ShouldNotify()
        {
            var editElementService = new EditElementService();
            var element = new ScriptTask() { Id = "Script-1" };
            var mock = new Mock<IEditElementConsumer>();
            Expression<Action<IEditElementConsumer>> call = 
                m => m.ConsumeEditElementAssigned(editElementService, It.Is<EditElementEventArgs>(editArgs => editArgs.processElement == element));
            mock.Setup(call).Verifiable();
            editElementService.EditElementAssigned += mock.Object.ConsumeEditElementAssigned;
            // ACT
            editElementService.EditElement = element;
            // ASSERT
            mock.Verify(call, Times.Once);
        }

        [Fact]
        public void AssignNewElementMultipleTimes_ShouldNotifyOnce()
        {
            var editElementService = new EditElementService();
            var element = new ScriptTask() { Id = "Script-1" };
            var mock = new Mock<IEditElementConsumer>();
            Expression<Action<IEditElementConsumer>> call = 
                m => m.ConsumeEditElementAssigned(editElementService, It.Is<EditElementEventArgs>(editArgs => editArgs.processElement == element));
            mock.Setup(call).Verifiable();
            editElementService.EditElementAssigned += mock.Object.ConsumeEditElementAssigned;
            // ACT
            editElementService.EditElement = element;
            editElementService.EditElement = element;
            editElementService.EditElement = element;
            // ASSERT
            mock.Verify(call, Times.Once);
        }

        [Fact]
        public void AssignNewElementAndChangeIt_ShouldNotify()
        {
            var editElementService = new EditElementService();
            var element = new ScriptTask() { Id = "Script-1" };
            var mock = new Mock<IEditElementConsumer>();
            Expression<Action<IEditElementConsumer>> call = m => m.ConsumeEditElementChanged(editElementService, EventArgs.Empty);
            mock.Setup(call).Verifiable();
            editElementService.EditElementModified += mock.Object.ConsumeEditElementChanged;
            // ACT
            editElementService.EditElement = element;
            element.Name = "Hello";
            editElementService.EditedElementModified();
            // ASSERT
            mock.Verify(call, Times.Once);
        }
    }
}
