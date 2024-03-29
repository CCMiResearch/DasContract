﻿using DasContract.Abstraction;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Editor.Web.Services.BpmnEvents.Exceptions;
using DasContract.Editor.Web.Services.EditElement;
using DasContract.Editor.Web.Services.ContractManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.BpmnEvents
{
    public class BpmnSynchronizer : IBpmnSynchronizer, IDisposable
    {

        private readonly IBpmnEventListener _bpmnEventHandler;
        private readonly IContractManager _contractManager;
        private readonly IProcessModelManager _processModelManager;
        private readonly IEditElementService _editElementService;
        private readonly IJSRuntime _jsRuntime;

        protected string CurrentContractId { get; set; }

        protected bool IsInitialized { get; set; } = false;

        public BpmnSynchronizer(
            IBpmnEventListener bpmnEventHandler,
            IProcessModelManager processManager,
            IContractManager contractManager,
            IEditElementService editElementService,
            IJSRuntime jsRuntime)
        {
            _bpmnEventHandler = bpmnEventHandler;
            _processModelManager = processManager;
            _editElementService = editElementService;
            _contractManager = contractManager;
            _jsRuntime = jsRuntime;
        }

        private void InitiliazeEventHandlers()
        {
            _bpmnEventHandler.ShapeAdded += OnShapeAdded;
            _bpmnEventHandler.ShapeRemoved += OnShapeRemoved;
            _bpmnEventHandler.ElementIdUpdated += OnElementIdUpdated;
            _bpmnEventHandler.ElementClick += OnElementClicked;
            _bpmnEventHandler.ElementChanged += OnElementChanged;
            _bpmnEventHandler.ConnectionAdded += OnConnectionAdded;
            _bpmnEventHandler.ConnectionRemoved += OnConnectionRemoved;
            _bpmnEventHandler.RootAdded += OnRootAdded;
            _bpmnEventHandler.RootRemoved += OnRootRemoved;
        }

        public async void InitializeOrRestoreBpmnEditor(string canvasElementId)
        {
            if (!IsInitialized)
            {
                await _bpmnEventHandler.InitializeHandler();
                InitiliazeEventHandlers();
                IsInitialized = true;
            }
            //Initial configuration and startup of the bpmn js component and its services
            if (_contractManager.GetContractId() != CurrentContractId)
            {
                var bpmnEditorDiagram = _processModelManager.GetProcessBpmnDefinition();
                await _jsRuntime.InvokeVoidAsync("modellerLib.createModeler", bpmnEditorDiagram ?? "", canvasElementId);
                CurrentContractId = _contractManager.GetContractId();
            }

            //If the contract didn't change, then the modeler is just reinjected into the canvas element
            else
            {
                await _jsRuntime.InvokeVoidAsync("modellerLib.restoreModelerElement", canvasElementId);
            }
        }

        public void Dispose()
        {
            _bpmnEventHandler.ShapeAdded -= OnShapeAdded;
            _bpmnEventHandler.ShapeRemoved -= OnShapeRemoved;
            _bpmnEventHandler.ElementIdUpdated -= OnElementIdUpdated;
            _bpmnEventHandler.ElementClick -= OnElementClicked;
            _bpmnEventHandler.ElementChanged -= OnElementChanged;
            _bpmnEventHandler.ConnectionAdded -= OnConnectionAdded;
            _bpmnEventHandler.ConnectionRemoved -= OnConnectionRemoved;
            _bpmnEventHandler.RootAdded -= OnRootAdded;
            _bpmnEventHandler.RootRemoved -= OnRootRemoved;
        }

        /// <summary>
        /// Handles a bpmn root added event, by adding an appropriate process into the data model. 
        /// This event occurs whenever a root is added into the model.
        /// This occurs in only two scenarios - the first participant (pool) is added, or the last participant is removed.
        /// Only the case when the last participant is removed is handled in here, as the other case is handled 
        /// by the participant added event
        /// </summary>
        private void OnRootAdded(object sender, BpmnElementEvent e)
        {
            if (e.Element.Type == "bpmn:Process")
            {
                _processModelManager.AddNewProcess(e.Element.Id);
            }
        }

        private void OnRootRemoved(object sender, BpmnElementEvent e)
        {
            if (e.Element.Type == "bpmn:Process")
            {
                _processModelManager.RemoveProcess(e.Element.Id);
            }
        }

        private void OnShapeAdded(object sender, BpmnElementEvent e)
        {
            //New process is being added (along with a participant/pool)
            if (e.Element.Type == "bpmn:Participant")
            {
                _processModelManager.AddNewProcess(e.Element.ProcessId, e.Element.Id);

                if (_processModelManager.TryGetProcess(e.Element.ProcessId, out var process))
                {
                    if (_editElementService.EditElement == process)
                        _editElementService.EditedElementModified();
                    else
                        _editElementService.EditElement = process;
                }
            }
            //Process element is being added
            else
            {
                OnElementAdded(e);
            }
        }

        private void OnElementAdded(BpmnElementEvent e)
        {
            var element = _processModelManager.AddElement(e.Element.Type, e.Element.Id, e.Element.ProcessId);
            if (element != null)
            {
                _editElementService.EditElement = element;
            }
        }

        private void OnShapeRemoved(object sender, BpmnElementEvent e)
        {
            if (e.Element.Type == "label" || e.Element.Type == "bpmn:TextAnnotation")
                return;

            //Process is being removed
            if (e.Element.Type == "bpmn:Participant")
            {
                var processId = _processModelManager.GetProcessIdFromParticipantId(e.Element.Id);
                _processModelManager.RemoveProcess(processId);
            }
            //Process element is being removed
            else
            {
                _processModelManager.RemoveElement(e.Element?.Id);
            }
            //Close the sidebar if the deleted element is currently selected
            if (_editElementService.EditElement?.Id == e.Element.Id || _editElementService.EditElement?.Id == e.Element.ProcessId)
                _editElementService.EditElement = null;
        }

        private void OnElementIdUpdated(object sender, BpmnElementEvent e)
        {
            _processModelManager.UpdateId(e.Element.Id, e.NewId, e.Element.ProcessId);
            if (_processModelManager.TryRetrieveElementById(e.NewId, e.Element.ProcessId, out var element))
            {
                if (_editElementService.EditElement == element)
                    _editElementService.EditedElementModified();
            }
        }

        private void OnElementChanged(object sender, BpmnElementEvent e)
        {
            //No process id is defined, or the process does not exist -- the element is in the phase of deletion
            if (string.IsNullOrEmpty(e.Element.ProcessId) || !_processModelManager.ProcessExists(e.Element.ProcessId))
                return;

            IContractElement contractElement = null;

            if (e.Element.Type == "bpmn:Participant")
            {
                _processModelManager.TryGetProcess(e.Element.ProcessId, out var process);
                process.Name = e.Element.Name;
                contractElement = process;
            }

            if (_processModelManager.TryRetrieveIElementById(e.Element.Id, out var element))
            {
                //Parse element name
                element.Name = e.Element.Name;
                //Parse element loop type
                if (element is Abstraction.Processes.Tasks.Task)
                {
                    var taskElement = element as Abstraction.Processes.Tasks.Task;
                    if (e.Element.LoopType == "bpmn:MultiInstanceLoopCharacteristics")
                    {
                        if (e.Element.IsSequential)
                            taskElement.InstanceType = Abstraction.Processes.Tasks.InstanceType.Sequential;
                        else
                            taskElement.InstanceType = Abstraction.Processes.Tasks.InstanceType.Parallel;
                    }
                    else
                    {
                        taskElement.InstanceType = Abstraction.Processes.Tasks.InstanceType.Single;
                    }
                }
                if (element is BoundaryEvent)
                {
                    var boundaryEvent = element as BoundaryEvent;
                    boundaryEvent.AttachedTo = e.Element.AttachedTo;
                }

                //Parse incoming and outgoing if processElement
                if (element is ProcessElement)
                {
                    var processElement = element as ProcessElement;
                    processElement.Incoming = new List<string>(e.Element.Incoming);
                    processElement.Outgoing = new List<string>(e.Element.Outgoing);
                }

                //Parse sequence flow information
                if (element is SequenceFlow)
                {
                    var sequenceFlow = element as SequenceFlow;
                    _processModelManager.UpdateSequenceFlowSourceAndTarget(sequenceFlow, e.Element.Source, e.Element.Target, e.Element.ProcessId);
                }

                //Check if parent process of the element has changed
                if (_processModelManager.TryRetrieveProcessOfElement(element.Id, out var process))
                {
                    if (process.Id != e.Element.ProcessId)
                    {
                        _processModelManager.ChangeProcessOfElement(element, process.Id, e.Element.ProcessId);
                    }
                }
                contractElement = element;
            }
            //Notify about element modification if currently selected
            if (_editElementService.EditElement == contractElement)
                _editElementService.EditedElementModified();
        }

        private void OnElementClicked(object sender, BpmnElementEvent e)
        {
            if (e.Element.Type == "bpmn:Collaboration" || e.Element.Type == "bpmn:Association" || e.Element.Type == "bpmn:TextAnnotation")
                return;

            string elementId;
            //Extract the represented element id if the element is of type label 
            if (e.Element?.Type == "label")
                elementId = e.Element.Id.Substring(0, e.Element.Id.Length - "_label".Length);
            else
                elementId = e.Element.Id;

            if (e.Element.Type == "bpmn:Process")
            {
                _processModelManager.TryGetProcess(e.Element.Id, out var process);
                _editElementService.EditElement = process;
            }
            else if (e.Element.Type == "bpmn:Participant")
            {
                _processModelManager.TryGetProcess(e.Element.ProcessId, out var process);
                _editElementService.EditElement = process;
            }
            else if (_processModelManager.TryRetrieveIElementById(elementId, e.Element.ProcessId, out var element))
            {
                _editElementService.EditElement = element;
            }
            else
            {
                _editElementService.EditElement = null;
            }
        }

        private void OnConnectionAdded(object sender, BpmnElementEvent e)
        {
            if (e.Element.Type == "bpmn:SequenceFlow")
            {
                _processModelManager.AddSequenceFlow(e.Element.Id, e.Element.Target, e.Element.Source, e.Element.ProcessId);
            }
        }

        private void OnConnectionRemoved(object sender, BpmnElementEvent e)
        {
            if (e.Element.Type == "bpmn:SequenceFlow")
            {
                if (_editElementService.EditElement?.Id == e.Element.Id)
                    _editElementService.EditElement = null;
                _processModelManager.RemoveSequenceFlow(e.Element.Id);
            }
        }
    }
}
