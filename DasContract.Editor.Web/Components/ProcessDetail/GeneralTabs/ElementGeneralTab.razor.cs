using DasContract.Abstraction;
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
        public IContractElement ContractElement { get; set; }

        [Inject]
        private IJSRuntime JSRunTime { get; set; }

        protected async void NameInput(FocusEventArgs args)
        {
            string elementId;
            if (ContractElement is Process)
            {
                var process = ContractElement as Process;
                elementId = process.ParticipantId;
            }
            else
                elementId = ContractElement.Id;

            if(elementId != null)
                await JSRunTime.InvokeVoidAsync("modellerLib.updateElementName", elementId, ContractElement.Name);
        }
    }
}
