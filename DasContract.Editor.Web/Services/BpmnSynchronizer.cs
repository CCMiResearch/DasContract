using DasContract.Abstraction.Processes;
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
        private EditElementService _editElementService;

        public BpmnSynchronizer(IBpmnEventHandler camundaEventHandler, IProcessManager processManager, EditElementService editElementService)
        {
            _bpmnEventHandler = camundaEventHandler;
            _processManager = processManager;
            _editElementService = editElementService;
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
        }

        private void ShapeAdded(object sender, BpmnInternalEvent e)
        {
            ProcessElement element;
            try
            {
                element = ProcessElementFactory.CreateElementFromType(e.Element.Type);
            }
            catch (InvalidCamundaElementTypeException _)
            {
                return;
            }

            element.Id = e.Element.Id;
            _processManager.AddElement(element);
            if (_processManager.TryRetrieveElementById(e.Element.Id, out element))
            {
                _editElementService.EditElement = element;
            }
        }

        private void ShapeRemoved(object sender, BpmnInternalEvent e)
        {
            if (e.Element.Type == "label")
                return;

            if (_processManager.TryRetrieveElementById<ProcessElement>(e.Element?.Id, out var element))
            {
                if (_editElementService.EditElement == element)
                    _editElementService.EditElement = null;
            }
            _processManager.RemoveElement(e.Element.Id);
        }

        private void ElementIdUpdated(object sender, BpmnInternalEvent e)
        {
            _processManager.UpdateId(e.Element.Id, e.NewId);
            if (_processManager.TryRetrieveElementById<ProcessElement>(e.NewId, out var element))
            {
                if (_editElementService.EditElement == element)
                    _editElementService.EditedElementModified();
            }

        }

        private void ElementChanged(object sender, BpmnInternalEvent e)
        {
            if (_processManager.TryRetrieveIElementById(e.Element?.Id, out var element))
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
                //Notify about element modification if currently selected
                if (_editElementService.EditElement == element)
                    _editElementService.EditedElementModified();
            }
        }

        private void ElementClicked(object sender, BpmnInternalEvent e)
        {
            string elementId;
            //Extract the represented element id if the element is of type label 
            if (e.Element?.Type == "label")
                elementId = e.Element.Id.Substring(0, e.Element.Id.Length - "_label".Length);
            else
                elementId = e.Element.Id;

            if(_processManager.TryRetrieveIElementById(elementId, out var element))
            {
                _editElementService.EditElement = element;
            }
            else
            {
                _editElementService.EditElement = null;
            }
        }

        private void ConnectionAdded(object sender, BpmnInternalEvent e)
        {
            var sequenceFlow = new SequenceFlow { Id = e.Element.Id };
            _processManager.AddSequenceFlow(sequenceFlow);
        }

        private void ConnectionRemoved(object sender, BpmnInternalEvent e)
        {
            if (_processManager.TryRetrieveSequenceFlowById(e.Element?.Id, out var sequenceFlow))
            {
                if (_editElementService.EditElement == sequenceFlow)
                    _editElementService.EditElement = null;
            }
            _processManager.RemoveSequenceFlow(e.Element.Id);
        }
    }
}
