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
    public class ModelerProcessSynchronizationTests : ModelerSynchronizationFixture
    {
        [Test]
        public async Task AddTwoProcesses_ShouldMatch()
        {
            await _commandManager.CreateNewProcess(Page, true);
            await _commandManager.CreateNewProcess(Page, false);
            await CompareCreatedContracts();
        }

        [Test]
        public async Task AddProcessesAndElements_ShouldMatch()
        {
            var proc1Id = await _commandManager.CreateNewProcess(Page, true);
            var proc2Id = await _commandManager.CreateNewProcess(Page, false);

            await _commandManager.AddProcessElement<UserTask>(Page, proc2Id);
            await _commandManager.AddProcessElement<UserTask>(Page, proc1Id);
            await CompareCreatedContracts();
        }
    }
}
