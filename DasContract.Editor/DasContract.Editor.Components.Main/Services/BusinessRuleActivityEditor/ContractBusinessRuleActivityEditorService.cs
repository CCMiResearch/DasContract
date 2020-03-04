using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace DasContract.Editor.Components.Main.Services.BusinessRuleActivityEditor
{
    public class ContractBusinessRuleActivityEditorService
    {
        readonly IJSRuntime jsRuntime;

        public ContractBusinessRuleActivityEditorService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task RedrawActiveEditorsAsync()
        {
            await jsRuntime.InvokeVoidAsync("DasContractComponents.ContractEditor.ActivitiesEditor.BusinessRuleEditor.RequestEditorsRedraw");
        }
    }
}
