using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.CContractProcessEditor
{
    public class ContractProcessEditorMediator
    {
        public event ContractProcessEditorMediatorHandler OnDiagramChange;

        readonly IJSRuntime jsRuntime;

        public ContractProcessEditorMediator(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task InitBPMN(string id, string xml = "")
        {
            await jsRuntime.InvokeVoidAsync("DasContractComponents.ContractEditor.ContractProcessEditor.InitBPMN", id, xml, 
                DotNetObjectReference.Create(this));
        }

        public async Task<string> GetDiagramXML(string id)
        {
            return await jsRuntime.InvokeAsync<string>("DasContractComponents.ContractEditor.ContractProcessEditor.GetDiagramXML", id);
        }

        public async Task SetDiagramXML(string id, string diagramXML)
        {
            await jsRuntime.InvokeVoidAsync("DasContractComponents.ContractEditor.ContractProcessEditor.SetDiagramXML", id, diagramXML);
        }

        [JSInvokable]
        public void DiagramChangeCallback()
        {
            OnDiagramChange?.Invoke(this, new ContractProcessEditorMediatorArgs());
        }
    }

    public delegate void ContractProcessEditorMediatorHandler(ContractProcessEditorMediator caller, ContractProcessEditorMediatorArgs args);

    public class ContractProcessEditorMediatorArgs
    {

    }
}
