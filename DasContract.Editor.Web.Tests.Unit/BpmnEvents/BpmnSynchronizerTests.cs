using DasContract.Abstraction;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Editor.Web.Services.BpmnEvents;
using DasContract.Editor.Web.Services.ContractManagement;
using DasContract.Editor.Web.Services.EditElement;
using Microsoft.JSInterop;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DasContract.Editor.Web.Tests.Unit.BpmnEvents
{
    public class BpmnSynchronizerTests
    {
        private readonly BpmnEventHandler _eventHandler;
        private readonly BpmnSynchronizer _bpmnSynchronizer;

        private readonly Mock<IContractManager> _contractManagerMock;
        private readonly Mock<IJSRuntime> _jsRuntimeMock;
        private readonly Mock<IProcessModelManager> _processModelManagerMock;
        private readonly Mock<IEditElementService> _editElementServiceMock;
        public BpmnSynchronizerTests()
        {
            _contractManagerMock = new Mock<IContractManager>();
            _jsRuntimeMock = new Mock<IJSRuntime>();
            _processModelManagerMock = new Mock<IProcessModelManager>();
            _editElementServiceMock = new Mock<IEditElementService>();

            _processModelManagerMock.Setup(m => m.TranslateBpmnProcessId(It.IsAny<string>())).Returns<string>(id => $"{id}-translated");
            _eventHandler = new BpmnEventHandler(_jsRuntimeMock.Object, _processModelManagerMock.Object);
            _bpmnSynchronizer = new BpmnSynchronizer(_eventHandler, _processModelManagerMock.Object,
                _contractManagerMock.Object, _editElementServiceMock.Object, _jsRuntimeMock.Object);

            _bpmnSynchronizer.InitializeOrRestoreBpmnEditor("");
        }

        [Fact]
        public void ElementClickEvent_ShouldCallElementService()
        {
            const string ELEMENT_ID = "ELEMENT1";
            const string PROCESS_ID = "PROCESS1";
            IProcessElement element = new ScriptTask { Id = ELEMENT_ID};
            _processModelManagerMock.Setup(p => p.TryRetrieveIElementById(ELEMENT_ID, $"{PROCESS_ID}-translated", out element)).Returns(true);
            _editElementServiceMock.SetupSet(e => e.EditElement = element).Verifiable();

            _eventHandler.HandleBpmnElementEvent(
                new BpmnElementEvent
                {
                    Element = new BpmnElement
                    {
                        Id = ELEMENT_ID,
                        ProcessId = PROCESS_ID,
                        Type = "bpmn:ScriptTask"
                    },
                    Type = BpmnConstants.BPMN_EVENT_CLICK
                });

            _editElementServiceMock.Verify();
        }

        [Fact]
        public void ProcessClickEvent_ShouldCallElementService()
        {
            const string PROCESS_ID = "PROCESS1";
            string PROCESS_ID_TRANSLATED = $"{PROCESS_ID}-translated";
            Process process = new Process { Id =  PROCESS_ID_TRANSLATED};
            _processModelManagerMock.Setup(c => c.TryGetProcess(PROCESS_ID_TRANSLATED, out process));
            _editElementServiceMock.SetupSet(e => e.EditElement = process).Verifiable();

            _eventHandler.HandleBpmnElementEvent(
                new BpmnElementEvent
                {
                    Element = new BpmnElement
                    {
                        Id = PROCESS_ID,
                        Type = BpmnConstants.BPMN_ELEMENT_PROCESS
                    },
                    Type = BpmnConstants.BPMN_EVENT_CLICK
                });

            _editElementServiceMock.Verify();
        }
    }
}
