using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.JsInterop
{
    public class SaveGuardJsCommunicator : JsCommunicator, ISaveGuardJsCommunicator
    {
        public SaveGuardJsCommunicator(IJSRuntime jSRuntime) : base(jSRuntime)
        {
        }

        public async Task<bool> DisplayAndCollectConfirmation(string message = null)
        {
            if (message is null)
            {
                return await JSRuntime.InvokeAsync<bool>("exitGuardLib.confirmDialog");
            }
            else
            {
                return await JSRuntime.InvokeAsync<bool>("exitGuardLib.confirmDialog", message);
            }
        }
    }
}
