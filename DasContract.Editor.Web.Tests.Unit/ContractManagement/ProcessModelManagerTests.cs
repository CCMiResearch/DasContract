using DasContract.Abstraction;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Gateways;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Editor.Web.Services.BpmnEvents;
using DasContract.Editor.Web.Services.BpmnEvents.Exceptions;
using DasContract.Editor.Web.Services.ContractManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DasContract.Editor.Web.Tests.Unit.ContractManagement
{
    public class ProcessModelManagerTests
    {
        private readonly Contract _contract;
        private readonly IProcessModelManager _processModelManager;

        public ProcessModelManagerTests()
        {
            _contract = new Contract();
            _processModelManager = new ProcessModelManager();
            _processModelManager.SetContract(_contract);
        }

        [Fact]
        public void AddNewProcess_ShouldAdd()
        {
            const string processId = "Process1";
            _processModelManager.AddNewProcess(processId, null);

            var result = _processModelManager.TryGetProcess(processId, out var process);
            Assert.True(result);
            Assert.Equal(processId, process.Id);
            Assert.Equal(processId, process.BpmnId);

            Assert.Single(_processModelManager.GetAllProcessIds());
            Assert.True(_processModelManager.ProcessExists(processId));

        }

        [Fact]
        public void AddDuplicateProcess_ShouldThrow()
        {
            const string processId = "Process1";

            _processModelManager.AddNewProcess(processId, "Part1");
            Assert.Throws<DuplicateIdException>(() => _processModelManager.AddNewProcess(processId, "Part1"));
        }

        [Fact]
        public void RemoveProcess_ShouldRemove()
        {
            const string processId = "Process1";

            _processModelManager.AddNewProcess(processId);
            _processModelManager.RemoveProcess(processId);

            Assert.False(_processModelManager.TryGetProcess(processId, out var _));
            Assert.Empty(_processModelManager.GetAllProcessIds());
        }

        [Fact]
        public void ReaddProcess_ShouldPersistObject()
        {
            const string processId = "Process1";

            _processModelManager.AddNewProcess(processId);
            _processModelManager.TryGetProcess(processId, out var expectedProcess);

            _processModelManager.RemoveProcess(processId);
            _processModelManager.AddNewProcess(processId);
            _processModelManager.TryGetProcess(processId, out var actualProcess);

            Assert.Equal(expectedProcess, actualProcess);
        }

        [Fact]
        public void ChangeProcessId_ShouldChange()
        {
            const string processId = "Process1";
            const string newProcessId = "NewProcess1";
            _processModelManager.AddNewProcess(processId);

            var result = _processModelManager.TryGetProcess(processId, out var process);

            _processModelManager.UpdateProcessId(process, newProcessId);

            result = _processModelManager.TryGetProcess(processId, out process);

            Assert.False(result);
            Assert.Null(process);

            result = _processModelManager.TryGetProcess(newProcessId, out process);

            Assert.True(result);
            Assert.Equal(newProcessId, process.Id);
            Assert.Equal(processId, process.BpmnId);
        }

        [Theory]
        [InlineData(BpmnConstants.BPMN_ELEMENT_CALL_ACTIVITY, typeof(CallActivity))]
        [InlineData(BpmnConstants.BPMN_ELEMENT_SCRIPT_TASK, typeof(ScriptTask))]
        [InlineData(BpmnConstants.BPMN_ELEMENT_USER_TASK, typeof(UserTask))]
        [InlineData(BpmnConstants.BPMN_ELEMENT_SERVICE_TASK, typeof(ServiceTask))]
        [InlineData(BpmnConstants.BPMN_ELEMENT_BUSINESS_RULE_TASK, typeof(BusinessRuleTask))]
        [InlineData(BpmnConstants.BPMN_ELEMENT_EXCLUSIVE_GATEWAY, typeof(ExclusiveGateway))]
        [InlineData(BpmnConstants.BPMN_ELEMENT_PARALLEL_GATEWAY, typeof(ParallelGateway))]
        [InlineData(BpmnConstants.BPMN_ELEMENT_START_EVENT, typeof(StartEvent))]
        [InlineData(BpmnConstants.BPMN_ELEMENT_END_EVENT, typeof(EndEvent))]
        [InlineData(BpmnConstants.BPMN_ELEMENT_TIMER_BOUNDARY_EVENT, typeof(TimerBoundaryEvent))]
        public void AddAndRetrieveProcessElement_ShouldOk(string elementType, Type netType)
        {
            const string processId = "Process1";
            const string elementId = "Element1";
            _processModelManager.AddNewProcess(processId);

            _processModelManager.AddElement(elementType, elementId, processId);

            var result = _processModelManager.TryRetrieveElementById(elementId, processId, out var element);

            Assert.True(result);
            Assert.Equal(elementId, element.Id);
            Assert.True(netType.IsAssignableTo(element.GetType()));
        }

        [Fact]
        public void AddAndRetrieveSequenceFlow_ShouldOk()
        {
            const string processId = "Process1";
            const string sourceId = "Element1";
            const string targetId = "Element2";
            const string flowId = "Flow1";

            _processModelManager.AddNewProcess(processId);
            _processModelManager.AddElement(BpmnConstants.BPMN_ELEMENT_SCRIPT_TASK, sourceId, processId);
            _processModelManager.AddElement(BpmnConstants.BPMN_ELEMENT_SCRIPT_TASK, targetId, processId);
            _processModelManager.AddSequenceFlow(flowId, targetId, sourceId, processId);

            var result = _processModelManager.TryRetrieveSequenceFlowById(flowId, processId, out var sequenceFlow);
            Assert.True(result);
            Assert.Equal(flowId, sequenceFlow.Id);
        }

        [Fact]
        public void AddSequenceFlowInvalidTarget_ShouldThrow()
        {
            const string processId = "Process1";
            const string sourceId = "Element1";
            const string targetId = "Element2";
            const string flowId = "Flow1";

            _processModelManager.AddNewProcess(processId);
            _processModelManager.AddElement(BpmnConstants.BPMN_ELEMENT_SCRIPT_TASK, sourceId, processId);

            Assert.Throws<InvalidIdException>(() => _processModelManager.AddSequenceFlow(flowId, targetId, sourceId, processId));
        }

        [Fact]
        public void AddSequenceFlowInvalidSource_ShouldThrow()
        {
            const string processId = "Process1";
            const string sourceId = "Element1";
            const string targetId = "Element2";
            const string flowId = "Flow1";

            _processModelManager.AddNewProcess(processId);
            _processModelManager.AddElement(BpmnConstants.BPMN_ELEMENT_SCRIPT_TASK, targetId, processId);

            Assert.Throws<InvalidIdException>(() => _processModelManager.AddSequenceFlow(flowId, targetId, sourceId, processId));
        }

        [Fact]
        public void SequenceFlowChangeSourceTarget_ShouldOk()
        {
            const string processId = "Process1";
            const string sourceId = "Element1";
            const string newSourceId = "NewElement1";
            const string targetId = "Element2";
            const string newTargetId = "NewElement2";
            const string flowId = "Flow1";

            _processModelManager.AddNewProcess(processId);
            _processModelManager.AddElement(BpmnConstants.BPMN_ELEMENT_SCRIPT_TASK, sourceId, processId);
            _processModelManager.AddElement(BpmnConstants.BPMN_ELEMENT_SCRIPT_TASK, targetId, processId);
            _processModelManager.AddElement(BpmnConstants.BPMN_ELEMENT_SCRIPT_TASK, newSourceId, processId);
            _processModelManager.AddElement(BpmnConstants.BPMN_ELEMENT_SCRIPT_TASK, newTargetId, processId);
            _processModelManager.AddSequenceFlow(flowId, targetId, sourceId, processId);

            _processModelManager.TryRetrieveSequenceFlowById(flowId, processId, out var sequenceFlow);

            _processModelManager.UpdateSequenceFlowSourceAndTarget(sequenceFlow, newSourceId, newTargetId, processId);

            Assert.Equal(newSourceId, sequenceFlow.SourceId);
            Assert.Equal(newTargetId, sequenceFlow.TargetId);
        }

        [Fact]
        public void ChangeProcessOfElement_ShouldOk()
        {
            const string processId1 = "Process1";
            const string processId2 = "Process2";
            const string elementId = "Element1";

            _processModelManager.AddNewProcess(processId1, "Part1");
            _processModelManager.AddNewProcess(processId2, "Part2");
            _processModelManager.AddElement(BpmnConstants.BPMN_ELEMENT_SCRIPT_TASK, elementId, processId1);
            _processModelManager.TryRetrieveIElementById(elementId, out var element);
            _processModelManager.ChangeProcessOfElement(element, processId1, processId2);

            Assert.False(_processModelManager.TryRetrieveElementById(elementId, processId1, out var _));
            Assert.True(_processModelManager.TryRetrieveElementById(elementId, processId2, out var _));
        }
    }
}
