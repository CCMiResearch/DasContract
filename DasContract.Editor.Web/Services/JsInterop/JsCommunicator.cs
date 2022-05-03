using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.JsInterop
{
    public abstract class JsCommunicator
    {
        protected IJSRuntime JSRuntime { get; set; }
        public JsCommunicator(IJSRuntime jSRuntime)
        {
            JSRuntime = jSRuntime;
        }
    }
}
