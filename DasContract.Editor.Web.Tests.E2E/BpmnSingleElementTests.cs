using NUnit.Framework;
using DasContract.Abstraction.Processes.Tasks;
using Task = System.Threading.Tasks.Task;
using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Tests.E2E.ModelerSynchronization;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Gateways;

namespace DasContract.Editor.Web.Tests.E2E
{
    [TestFixture(typeof(StartEvent))]
    [TestFixture(typeof(EndEvent))]
    [TestFixture(typeof(Abstraction.Processes.Tasks.Task))]
    [TestFixture(typeof(UserTask))]
    [TestFixture(typeof(ScriptTask))]
    [TestFixture(typeof(ServiceTask))]
    [TestFixture(typeof(BusinessRuleTask))]
    [TestFixture(typeof(CallActivity))]
    [TestFixture(typeof(ParallelGateway))]
    [TestFixture(typeof(ExclusiveGateway))]
    [TestFixture(typeof(Event))]
    [TestFixture(typeof(BoundaryEvent))]
    public class BpmnSingleElementTests<T> : ModelerSynchronizationFixture where T: ProcessElement, new()
    {
        [Test]
        public async Task AddElement_ShouldMatch()
        {
            await _commandManager.AddProcessElement<T>(Page, "Process_1");
            await CompareCreatedContracts();
        }

    }
}