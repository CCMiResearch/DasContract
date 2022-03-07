using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace DasContract.Editor.Web.Services.BpmnEvents
{
    public class BpmnEventHandler : IBpmnEventHandler
    {
        IJSRuntime _jsRuntime;

        public event EventHandler<BpmnElementEvent> ElementClick;
        public event EventHandler<BpmnElementEvent> ElementChanged;
        public event EventHandler<BpmnElementEvent> ElementIdUpdated;
        public event EventHandler<BpmnElementEvent> ShapeAdded;
        public event EventHandler<BpmnElementEvent> ShapeRemoved;
        public event EventHandler<BpmnElementEvent> ConnectionAdded;
        public event EventHandler<BpmnElementEvent> ConnectionRemoved;
        public event EventHandler<BpmnElementEvent> RootAdded;
        public event EventHandler<BpmnElementEvent> RootRemoved;

        public BpmnEventHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeHandler()
        {
            await _jsRuntime.InvokeVoidAsync("modellerLib.setEventHandlerInstance", DotNetObjectReference.Create(this));
        }

        [JSInvokable]
        public void HandleBpmnElementEvent(BpmnElementEvent e)
        {
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
    }
}
