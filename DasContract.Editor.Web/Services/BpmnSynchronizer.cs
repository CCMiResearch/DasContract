using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.CamundaEvents;
using DasContract.Editor.Web.Services.EditElement;
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
        }

        public void Dispose()
        {
            _camundaEventHandler.ShapeAdded -= ShapeAdded;
            _camundaEventHandler.ShapeRemoved -= ShapeRemoved;
            _camundaEventHandler.ElementIdUpdated -= ElementIdUpdated;
            _camundaEventHandler.ElementClick -= ElementClicked;
        }

        private void ShapeAdded(object sender, BpmnInternalEvent e)
        {
            _processManager.AddElement(e.Element.Id, e.Element.Type);
        }

        private void ShapeRemoved(object sender, BpmnInternalEvent e)
        {
            _processManager.RemoveElement(e.Element.Id);
        }

        private void ElementIdUpdated(object sender, BpmnInternalEvent e)
        {
            _processManager.UpdateId(e.Element.Id, e.NewId);
        }

        private void ElementClicked(object sender, BpmnInternalEvent e)
        {
            if(_processManager.TryRetrieveElementById<ProcessElement>(e.Element?.Id, out var element))
            {
                _editElementService.EditElement = element;
            }
        }
    }
}
