using DasContract.Editor.Web.Services.CamundaEvents;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services
{
    public class BpmnSynchronizer : IBpmnSynchronizer
    {

        private IBpmnEventHandler _camundaEventHandler;
        private IProcessManager _processManager;

        public BpmnSynchronizer(IBpmnEventHandler camundaEventHandler, IProcessManager processManager)
        {
            _camundaEventHandler = camundaEventHandler;
            _processManager = processManager;
        }

        public void Initiliaze()
        {
            _camundaEventHandler.ShapeAdded += ShapeAdded;
            _camundaEventHandler.ShapeRemoved += ShapeRemoved;
            _camundaEventHandler.ElementIdUpdated += ElementIdUpdated;
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
    }
}
