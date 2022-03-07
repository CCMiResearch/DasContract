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
    public class ModelerConnectionSynchronizationTests : ModelerSynchronizationFixture
    {
        [Test]
        public async Task AddConnection_ShouldMatch()
        {
            var sourceId = await _commandManager.AddProcessElement<UserTask>(Page, ModelerCommandManager.DEFAULT_PROCESS_ID);
            var targetId = await _commandManager.AddProcessElement<UserTask>(Page, ModelerCommandManager.DEFAULT_PROCESS_ID);
            await _commandManager.ConnectElements(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, sourceId, targetId);
            await CompareCreatedContracts();
        }

        [Test]
        public async Task AddAndRenameConnection_ShouldMatch()
        {
            var sourceId = await _commandManager.AddProcessElement<UserTask>(Page, ModelerCommandManager.DEFAULT_PROCESS_ID);
            var targetId = await _commandManager.AddProcessElement<UserTask>(Page, ModelerCommandManager.DEFAULT_PROCESS_ID);
            var connectionId = await _commandManager.ConnectElements(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, sourceId, targetId);
            await _commandManager.RenameConnection(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, connectionId, "Hello world!");
            await CompareCreatedContracts();
        }

        [Test]
        public async Task UndoRedo_ShouldMatch()
        {
            var sourceId = await _commandManager.AddProcessElement<UserTask>(Page, ModelerCommandManager.DEFAULT_PROCESS_ID);
            var targetId = await _commandManager.AddProcessElement<UserTask>(Page, ModelerCommandManager.DEFAULT_PROCESS_ID);
            var connectionId = await _commandManager.ConnectElements(Page, ModelerCommandManager.DEFAULT_PROCESS_ID, sourceId, targetId);
            
            await CompareCreatedContracts();
            await _commandManager.Undo(Page);
            await CompareCreatedContracts();
            await _commandManager.Undo(Page);
            await CompareCreatedContracts();
            await _commandManager.Redo(Page);
            await CompareCreatedContracts();
            await _commandManager.Redo(Page);
            await CompareCreatedContracts();
        }
    }
}
