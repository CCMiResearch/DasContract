using DasContract.Abstraction.Processes.Tasks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace DasContract.Editor.Web.Tests.E2E.ModelerSynchronization
{
    public class ModelerTaskSynchronizationTests : ModelerSynchronizationFixture
    {
        [Test]
        public async Task AddAndRenameTask_ShouldMatch()
        {
            var taskId = await _commandManager.AddProcessElement<UserTask>(Page, DEFAULT_PROCESS_ID);
            await _commandManager.RenameElement(Page, DEFAULT_PROCESS_ID, taskId, "Hello world!");
            await CompareCreatedContracts();
        }

        [Test]
        public async Task AddAndRemoveTask_ShouldMatch()
        {
            var elementId = await _commandManager.AddProcessElement<UserTask>(Page, DEFAULT_PROCESS_ID);
            await CompareCreatedContracts();
            await _commandManager.RemoveProcessElement(Page, DEFAULT_PROCESS_ID, elementId);
            await CompareCreatedContracts();
        }

        [Test]
        public async Task AddAndSetLoopCharacteristics_ShouldMatch()
        {
            var elementId = await _commandManager.AddProcessElement<UserTask>(Page, DEFAULT_PROCESS_ID);
            await _commandManager.SetTaskLoopCharacteristic(Page, DEFAULT_PROCESS_ID, elementId, InstanceType.Parallel);
            await CompareCreatedContracts();
            await _commandManager.SetTaskLoopCharacteristic(Page, DEFAULT_PROCESS_ID, elementId, InstanceType.Single);
        }
    }
}
