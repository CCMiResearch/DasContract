using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
