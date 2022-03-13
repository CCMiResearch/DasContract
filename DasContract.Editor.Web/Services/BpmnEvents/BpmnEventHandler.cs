using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using DasContract.Editor.Web.Services.Processes;
using DasContract.Abstraction.Processes;

namespace DasContract.Editor.Web.Services.BpmnEvents
{
    public class BpmnEventHandler : IBpmnEventHandler
    {
        IJSRuntime _jsRuntime;
        IContractManager _contractManager;

        public event EventHandler<BpmnElementEvent> ElementClick;
        public event EventHandler<BpmnElementEvent> ElementChanged;
        public event EventHandler<BpmnElementEvent> ElementIdUpdated;
        public event EventHandler<BpmnElementEvent> ShapeAdded;
        public event EventHandler<BpmnElementEvent> ShapeRemoved;
        public event EventHandler<BpmnElementEvent> ConnectionAdded;
        public event EventHandler<BpmnElementEvent> ConnectionRemoved;
        public event EventHandler<BpmnElementEvent> RootAdded;
        public event EventHandler<BpmnElementEvent> RootRemoved;

        public BpmnEventHandler(IJSRuntime jsRuntime, IContractManager contractManager)
        {
            _jsRuntime = jsRuntime;
            _contractManager = contractManager;
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
                case "element.click":
                    ElementClick?.Invoke(this, e);
                    break;
                case "element.changed":
                    ElementChanged?.Invoke(this, e);
                    break;
                case "shape.added":
                    ShapeAdded?.Invoke(this, e);
                    break;
                case "shape.removed":
                    ShapeRemoved?.Invoke(this, e);
                    break;
                case "element.updateId":
                    ElementIdUpdated?.Invoke(this, e);
                    break;
                case "connection.removed":
                    ConnectionRemoved?.Invoke(this, e);
                    break;
                case "connection.added":
                    ConnectionAdded?.Invoke(this, e);
                    break;
                case "root.added":
                    RootAdded?.Invoke(this, e);
                    break;
                case "root.removed":
                    RootRemoved?.Invoke(this, e);   
                    break;
            }
        }

        private void TranslateProcessId(BpmnElementEvent e)
        {
            string procId;
            if (e.Element.Type == BpmnConstants.BPMN_ELEMENT_PROCESS)
            {
                procId = _contractManager.TranslateBpmnProcessId(e.Element.Id);
                if (procId != null)
                {
                    e.Element.Id = procId;
                }
            }

            if (string.IsNullOrEmpty(e.Element.ProcessId))
                return;

            procId = _contractManager.TranslateBpmnProcessId(e.Element.ProcessId);
            if (procId != null)
            {
                e.Element.ProcessId = procId;
            }
        }
    }
}
