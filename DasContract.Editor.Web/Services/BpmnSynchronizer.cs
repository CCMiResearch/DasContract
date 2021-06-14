using DasContract.Abstraction;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Editor.Web.Services.BpmnEvents;
using DasContract.Editor.Web.Services.BpmnEvents.Exceptions;
using DasContract.Editor.Web.Services.EditElement;
using DasContract.Editor.Web.Services.Processes;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services
{
    public class BpmnSynchronizer : IBpmnSynchronizer, IDisposable
    {

        private IBpmnEventHandler _bpmnEventHandler;
        private IProcessManager _processManager;
        private IContractManager _contractManager;
        private EditElementService _editElementService;

        public BpmnSynchronizer(
            IBpmnEventHandler camundaEventHandler, 
            IProcessManager processManager,
            IContractManager contractManager, 
            EditElementService editElementService)
        {
            _bpmnEventHandler = camundaEventHandler;
            _processManager = processManager;
            _editElementService = editElementService;
            _contractManager = contractManager;
        }

        public void Initiliaze()
        {
            _bpmnEventHandler.ShapeAdded += ShapeAdded;
            _bpmnEventHandler.ShapeRemoved += ShapeRemoved;
            _bpmnEventHandler.ElementIdUpdated += ElementIdUpdated;
            _bpmnEventHandler.ElementClick += ElementClicked;
            _bpmnEventHandler.ElementChanged += ElementChanged;
            _bpmnEventHandler.ConnectionAdded += ConnectionAdded;
            _bpmnEventHandler.ConnectionRemoved += ConnectionRemoved;
            _bpmnEventHandler.RootAdded += RootAdded;
            _bpmnEventHandler.RootRemoved += RootRemoved;
        }

        public void Dispose()
        {
            _bpmnEventHandler.ShapeAdded -= ShapeAdded;
            _bpmnEventHandler.ShapeRemoved -= ShapeRemoved;
            _bpmnEventHandler.ElementIdUpdated -= ElementIdUpdated;
            _bpmnEventHandler.ElementClick -= ElementClicked;
            _bpmnEventHandler.ElementChanged -= ElementChanged;
            _bpmnEventHandler.ConnectionAdded -= ConnectionAdded;
            _bpmnEventHandler.ConnectionRemoved -= ConnectionRemoved;
            _bpmnEventHandler.RootAdded -= RootAdded;
            _bpmnEventHandler.RootRemoved -= RootRemoved;
        }

        /// <summary>
        /// Handles a bpmn root added event, by adding an appropriate process into the data model. 
        /// This event occurs whenever a root is added into the model.
        /// This occurs in only two scenarios - the first participant (pool) is added, or the last participant is removed.
        /// Only the case when the last participant is removed is handled in here, as the other case is handled 
        /// by the participant added event
        /// </summary>
        private void RootAdded(object sender, BpmnElementEvent e)
        {
            if (e.Element.Type == "bpmn:Process")
            {
                _contractManager.AddNewProcess(e.Element.Id);
            }
        }

        private void RootRemoved(object sender, BpmnElementEvent e)
        {
            /*
            if (e.Element.Type == "bpmn:Process")
            {
                _contractManager.RemoveProcess(e.Element.Id);
            }
            */
        }

        private void ShapeAdded(object sender, BpmnElementEvent e)
        {
            //New process is being added (along with a participant/pool)
            if (e.Element.Type == "bpmn:Participant")
            {
                _contractManager.AddNewProcess(e.Element.ProcessId, e.Element.Id);
                
                if (_contractManager.TryGetProcess(e.Element.ProcessId, out var process))
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
                ElementAdded(e);
            }
        }

        private void ElementAdded(BpmnElementEvent e)
        {
            ProcessElement element;
            try
            {
                element = ProcessElementFactory.CreateElementFromType(e.Element.Type);
            }
            catch (InvalidCamundaElementTypeException)
            {
                return;
            }

            element.Id = e.Element.Id;
            _processManager.AddElement(element, e.Element.ProcessId);
            if (_processManager.TryRetrieveElementById(e.Element.Id, e.Element.ProcessId, out element))
            {
                _editElementService.EditElement = element;
            }
        }

        private void ShapeRemoved(object sender, BpmnElementEvent e)
        {
            if (e.Element.Type == "label" || e.Element.Type == "bpmn:TextAnnotation")
                return;

            //Process is being removed
            if (e.Element.Type == "bpmn:Participant")
            {
                var processId = _contractManager.GetProcessIdFromParticipantId(e.Element.Id);
                _contractManager.RemoveProcess(processId);
            }
            //Process element is being removed
            else
            {
                _processManager.RemoveElement(e.Element?.Id);
            }
            //Close the sidebar if the deleted element is currently selected
            if (_editElementService.EditElement?.Id == e.Element.Id || _editElementService.EditElement?.Id == e.Element.ProcessId)
                _editElementService.EditElement = null;
        }

        private void ElementIdUpdated(object sender, BpmnElementEvent e)
        {
            _processManager.UpdateId(e.Element.Id, e.NewId, e.Element.ProcessId);
            if (_processManager.TryRetrieveElementById(e.NewId, e.Element.ProcessId, out var element))
            {
                if (_editElementService.EditElement == element)
                    _editElementService.EditedElementModified();
            }
        }

        private void ElementChanged(object sender, BpmnElementEvent e)
        {
            //No process id is defined, or the process does not exist -- the element is in the phase of deletion
            if (string.IsNullOrEmpty(e.Element.ProcessId) || !_processManager.ProcessExists(e.Element.ProcessId))
                return;

            IContractElement contractElement = null;

            if (e.Element.Type == "bpmn:Participant")
            {
                _contractManager.TryGetProcess(e.Element.ProcessId, out var process);
                process.Name = e.Element.Name;
                contractElement = process;
            }

            if (_processManager.TryRetrieveIElementById(e.Element.Id, out var element))
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
                    Console.WriteLine($"Attached to: {e.Element.AttachedTo}");
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
                    _processManager.UpdateSequenceFlowSourceAndTarget(sequenceFlow, e.Element.Source, e.Element.Target, e.Element.ProcessId);
                }    

                //Check if parent process of the element has changed
                if (_processManager.TryGetProcessOfElement(element.Id, out var process))
                {
                    if (process.Id != e.Element.ProcessId)
                    {
                        Console.WriteLine($"Process changed in element, prev process: {process.Id}, new Process: {e.Element.ProcessId}");
                        _processManager.ChangeProcessOfElement(element, process.Id, e.Element.ProcessId);
                    }
                }
                contractElement = element;
            }
            //Notify about element modification if currently selected
            if (_editElementService.EditElement == contractElement)
                _editElementService.EditedElementModified();
        }

        private void ElementClicked(object sender, BpmnElementEvent e)
        {
            if (e.Element.Type == "bpmn:Collaboration" || e.Element.Type == "bpmn:Association" || e.Element.Type == "bpmn:TextAnnotation")
                return;

            string elementId;
            //Extract the represented element id if the element is of type label 
            if (e.Element?.Type == "label")
                elementId = e.Element.Id.Substring(0, e.Element.Id.Length - "_label".Length);
            else
                elementId = e.Element.Id;

            if(e.Element.Type == "bpmn:Process")
            {
                _contractManager.TryGetProcess(e.Element.Id, out var process);
                _editElementService.EditElement = process;
            }
            else if(e.Element.Type == "bpmn:Participant")
            {
                _contractManager.TryGetProcess(e.Element.ProcessId, out var process);
                _editElementService.EditElement = process;
            }
            else if(_processManager.TryRetrieveIElementById(elementId, e.Element.ProcessId, out var element))
            {
                _editElementService.EditElement = element;
            }
            else
            {
                _editElementService.EditElement = null;
            }
        }

        private void ConnectionAdded(object sender, BpmnElementEvent e)
        {
            if (e.Element.Type == "bpmn:SequenceFlow")
            {
                var sequenceFlow = new SequenceFlow
                {
                    Id = e.Element.Id,
                    Name = e.Element.Name,
                    SourceId = e.Element.Source,
                    TargetId = e.Element.Target
                };
                _processManager.AddSequenceFlow(sequenceFlow, e.Element.ProcessId);
            }
        }

        private void ConnectionRemoved(object sender, BpmnElementEvent e)
        {
            if (e.Element.Type == "bpmn:SequenceFlow")
            {
                if (_editElementService.EditElement?.Id == e.Element.Id)
                    _editElementService.EditElement = null;
                _processManager.RemoveSequenceFlow(e.Element.Id);
            }
        }
    }
}
