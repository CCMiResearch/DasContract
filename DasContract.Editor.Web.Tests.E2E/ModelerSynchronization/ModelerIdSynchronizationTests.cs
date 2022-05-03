using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Tasks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Tests.E2E.ModelerSynchronization
{
    public class ModelerIdSynchronizationTests : ModelerSynchronizationFixture
    {
        [Test]
        public async System.Threading.Tasks.Task ChangeIdOfSequenceFlow_ShouldMatch()
        {
            var taskId1 = await _commandManager.AddProcessElement<UserTask>(Page, ModelerCommandManager.DEFAULT_PROCESS_ID);
            var taskId2 = await _commandManager.AddProcessElement<UserTask>(Page, ModelerCommandManager.DEFAULT_PROCESS_ID);
            var sequenceFlowId = await _commandManager.ConnectElements(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, taskId1, taskId2);
            await CompareCreatedContracts();
            await _commandManager.EditConnectionId(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, sequenceFlowId, "MyCustomLittleId");
            await CompareCreatedContracts();
        }
        
        [Test]
        public async System.Threading.Tasks.Task ChangeIdOfSequenceFlowUndoRedo_ShouldMatch()
        {
            var taskId1 = await _commandManager.AddProcessElement<UserTask>(Page, ModelerCommandManager.DEFAULT_PROCESS_ID);
            var taskId2 = await _commandManager.AddProcessElement<UserTask>(Page, ModelerCommandManager.DEFAULT_PROCESS_ID);
            var sequenceFlowId = await _commandManager.ConnectElements(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, taskId1, taskId2);
            await CompareCreatedContracts();
            await _commandManager.EditConnectionId(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, sequenceFlowId, "MyCustomLittleId");
            await CompareCreatedContracts();
        }

        [Test]
        public async System.Threading.Tasks.Task ChangeIdOfTask_ShouldMatch()
        {
            var taskId1 = await _commandManager.AddProcessElement<UserTask>(Page, ModelerCommandManager.DEFAULT_PROCESS_ID);
            var taskId2 = await _commandManager.AddProcessElement<UserTask>(Page, ModelerCommandManager.DEFAULT_PROCESS_ID);
            var sequenceFlowId = await _commandManager.ConnectElements(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, taskId1, taskId2);
            var sequenceFlowId2 = await _commandManager.ConnectElements(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, taskId1, taskId2);
            var sequenceFlowId3 = await _commandManager.ConnectElements(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, taskId1, taskId2);
            await CompareCreatedContracts();
            await _commandManager.EditProcessElementId(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, taskId1, "Sauce");
            await _commandManager.EditProcessElementId(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, taskId2, "Tawget");
            await CompareCreatedContracts();
        }

        [Test]
        public async System.Threading.Tasks.Task ChangeIdOfTaskWithUndoRedo_ShouldMatch()
        {
            var taskId1 = await _commandManager.AddProcessElement<UserTask>(Page, ModelerCommandManager.DEFAULT_PROCESS_ID);
            var taskId2 = await _commandManager.AddProcessElement<UserTask>(Page, ModelerCommandManager.DEFAULT_PROCESS_ID);
            var sequenceFlowId = await _commandManager.ConnectElements(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, taskId1, taskId2);
            var sequenceFlowId2 = await _commandManager.ConnectElements(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, taskId1, taskId2);
            var sequenceFlowId3 = await _commandManager.ConnectElements(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, taskId1, taskId2);
            await CompareCreatedContracts();
            await _commandManager.EditProcessElementId(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, taskId1, "Sauce");
            await _commandManager.EditProcessElementId(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, taskId2, "Tawget");
            await CompareCreatedContracts();
            await _commandManager.Undo(Page);
            await _commandManager.Undo(Page);
            await CompareCreatedContracts();
            await _commandManager.Redo(Page);
            await _commandManager.Redo(Page);
            await CompareCreatedContracts();
        }
    }
}
