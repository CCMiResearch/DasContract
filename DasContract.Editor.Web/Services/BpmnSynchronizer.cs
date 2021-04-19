using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.BpmnEvents;
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

        private IBpmnEventHandler _camundaEventHandler;
        private IProcessManager _processManager;
        private EditElementService _editElementService;

        public BpmnSynchronizer(IBpmnEventHandler camundaEventHandler, IProcessManager processManager, EditElementService editElementService)
        {
            _camundaEventHandler = camundaEventHandler;
            _processManager = processManager;
            _editElementService = editElementService;
        }

        public void Initiliaze()
        {
            _camundaEventHandler.ShapeAdded += ShapeAdded;
            _camundaEventHandler.ShapeRemoved += ShapeRemoved;
            _camundaEventHandler.ElementIdUpdated += ElementIdUpdated;
            _camundaEventHandler.ElementClick += ElementClicked;
            _camundaEventHandler.ElementChanged += ElementChanged;
        }

        public void Dispose()
        {
            _camundaEventHandler.ShapeAdded -= ShapeAdded;
            _camundaEventHandler.ShapeRemoved -= ShapeRemoved;
            _camundaEventHandler.ElementIdUpdated -= ElementIdUpdated;
            _camundaEventHandler.ElementClick -= ElementClicked;
            _camundaEventHandler.ElementChanged -= ElementChanged;
        }

        private void ShapeAdded(object sender, BpmnInternalEvent e)
        {
            var element = ProcessElementFactory.CreateElementFromType(e.Element.Type);
            element.Id = e.Element.Id;
            _processManager.AddElement(element);
            if (_processManager.TryRetrieveElementById(e.Element.Id, out element))
            {
                _editElementService.EditElement = element;
            }
        }

        private void ShapeRemoved(object sender, BpmnInternalEvent e)
        {
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
            if (_processManager.TryRetrieveElementById<ProcessElement>(e.Element?.Id, out var element))
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
            if(_processManager.TryRetrieveElementById<ProcessElement>(e.Element?.Id, out var element))
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
            
        }
    }
}
