using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using DasContract.Editor.Web.Services.ContractManagement;
using DasContract.Abstraction.Processes;

namespace DasContract.Editor.Web.Services.BpmnEvents
{
    public class BpmnEventListener : IBpmnEventListener
    {
        IJSRuntime _jsRuntime;
        IProcessModelManager _processModelManager;

        public event EventHandler<BpmnElementEvent> ElementClick;
        public event EventHandler<BpmnElementEvent> ElementChanged;
        public event EventHandler<BpmnElementEvent> ElementIdUpdated;
        public event EventHandler<BpmnElementEvent> ShapeAdded;
        public event EventHandler<BpmnElementEvent> ShapeRemoved;
        public event EventHandler<BpmnElementEvent> ConnectionAdded;
        public event EventHandler<BpmnElementEvent> ConnectionRemoved;
        public event EventHandler<BpmnElementEvent> RootAdded;
        public event EventHandler<BpmnElementEvent> RootRemoved;

        public BpmnEventListener(IJSRuntime jsRuntime, IProcessModelManager processModelManager)
        {
            _jsRuntime = jsRuntime;
            _processModelManager = processModelManager;
        }

        public async Task InitializeHandler()
        {
            await _jsRuntime.InvokeVoidAsync("modellerLib.setEventHandlerInstance", DotNetObjectReference.Create(this));
        }

        [JSInvokable]
        public void HandleBpmnElementEvent(BpmnElementEvent e)
        {
            TranslateProcessId(e);
            switch(e.Type)
            {
                case BpmnConstants.BPMN_EVENT_CLICK:
                    ElementClick?.Invoke(this, e);
                    break;
                case BpmnConstants.BPMN_EVENT_ELEMENT_CHANGED:
                    ElementChanged?.Invoke(this, e);
                    break;
                case BpmnConstants.BPMN_EVENT_SHAPE_ADDED:
                    ShapeAdded?.Invoke(this, e);
                    break;
                case BpmnConstants.BPMN_EVENT_SHAPE_REMOVED:
                    ShapeRemoved?.Invoke(this, e);
                    break;
                case BpmnConstants.BPMN_EVENT_UPDATE_ID:
                    ElementIdUpdated?.Invoke(this, e);
                    break;
                case BpmnConstants.BPMN_EVENT_CONNECTION_REMOVED:
                    ConnectionRemoved?.Invoke(this, e);
                    break;
                case BpmnConstants.BPMN_EVENT_CONNECTION_ADDED:
                    ConnectionAdded?.Invoke(this, e);
                    break;
                case BpmnConstants.BPMN_EVENT_ROOT_ADDED:
                    RootAdded?.Invoke(this, e);
                    break;
                case BpmnConstants.BPMN_EVENT_ROOT_REMOVED:
                    RootRemoved?.Invoke(this, e);   
                    break;
            }
        }

        private void TranslateProcessId(BpmnElementEvent e)
        {
            string procId;
            if (e.Element.Type == BpmnConstants.BPMN_ELEMENT_PROCESS)
            {
                procId = _processModelManager.TranslateBpmnProcessId(e.Element.Id);
                if (procId != null)
                {
                    e.Element.Id = procId;
                }
            }

            if (string.IsNullOrEmpty(e.Element.ProcessId))
                return;

            procId = _processModelManager.TranslateBpmnProcessId(e.Element.ProcessId);
            if (procId != null)
            {
                e.Element.ProcessId = procId;
            }
        }
    }
}
