using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace DasContract.Editor.Web.Services.CamundaEvents
{
    public class BpmnEventHandler : IBpmnEventHandler
    {

        IJSRuntime _jsRuntime;

        public event EventHandler<BpmnInternalEvent> ElementClick;
        public event EventHandler<BpmnInternalEvent> ElementChanged;
        public event EventHandler<BpmnInternalEvent> ShapeAdded;
        public event EventHandler<BpmnInternalEvent> ShapeRemoved;
        public event EventHandler<BpmnInternalEvent> ElementIdUpdated;

        public BpmnEventHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeHandler()
        {
            await _jsRuntime.InvokeVoidAsync("modellerLib.setEventHandlerInstance", DotNetObjectReference.Create(this));
        }

        [JSInvokable]
        public void HandleCamundaEvent(BpmnInternalEvent e)
        {
            switch(e.Type)
            {
                case "element.click":
                    ElementClick?.Invoke(this, e);
                    break;
                case "shape.changed":
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
            }
        }
    }
}
