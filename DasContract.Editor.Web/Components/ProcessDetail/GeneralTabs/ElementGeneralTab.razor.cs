using DasContract.Abstraction.Processes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.ProcessDetail.GeneralTabs
{
    public partial class ElementGeneralTab: ComponentBase
    {
        [Parameter]
        public IProcessElement ProcessElement { get; set; }

        [Inject]
        private IJSRuntime JSRunTime { get; set; }

        protected async void NameInput(FocusEventArgs args)
        {
            await JSRunTime.InvokeVoidAsync("modellerLib.updateElementName", ProcessElement.Id, ProcessElement.Name);
        }
    }
}
