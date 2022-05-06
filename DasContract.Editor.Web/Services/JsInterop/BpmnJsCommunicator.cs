using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.JsInterop
{
    public class BpmnJsCommunicator : JsCommunicator, IBpmnJsCommunicator
    {
        public BpmnJsCommunicator(IJSRuntime jSRuntime) : base(jSRuntime)
        {
        }

        public async Task UpdateElementId(string oldElementId, string newElementId)
        {
            await JSRuntime.InvokeVoidAsync("modellerLib.updateElementId", oldElementId, newElementId);
        }

        public async Task UpdateElementName(string elementId, string newElementName)
        {
            await JSRuntime.InvokeVoidAsync("modellerLib.updateElementName", elementId, newElementName);
        }
    }
}
