using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.ActivitiesEditor.BusinessRuleEditor
{
    public class ContractBusinessRuleActivityEditorMediator
    {
        //public event ContractBusinessRuleActivityEditorMediatorHandler OnDiagramChange;

        public event ContractBusinessRuleActivityEditorMediatorHandler OnRedrawRequest;

        readonly IJSRuntime jsRuntime;

        public ContractBusinessRuleActivityEditorMediator(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task InitBPMN(string id, string xml = "")
        {
            await jsRuntime.InvokeVoidAsync("DasContractComponents.ContractEditor.ActivitiesEditor.BusinessRuleEditor.InitBPMN", id, xml,
                DotNetObjectReference.Create(this));
        }

        public async Task<string> GetDiagramXML(string id)
        {
            return await jsRuntime.InvokeAsync<string>("DasContractComponents.ContractEditor.ActivitiesEditor.BusinessRuleEditor.GetDiagramXML", id);
        }

        public async Task SetDiagramXML(string id, string diagramXML)
        {
            await jsRuntime.InvokeVoidAsync("DasContractComponents.ContractEditor.ActivitiesEditor.BusinessRuleEditor.SetDiagramXML", id, diagramXML);
        }

        [JSInvokable]
        public void RequestEditorRedrawCallback()
        {
            OnRedrawRequest?.Invoke(this, new ContractBusinessRuleActivityEditorMediatorArgs());
        }

        //[JSInvokable]
        //public void DiagramChangeCallback()
        //{
        //    OnDiagramChange?.Invoke(this, new ContractBusinessRuleActivityEditorMediatorArgs());
        //}
    }

    public delegate void ContractBusinessRuleActivityEditorMediatorHandler(ContractBusinessRuleActivityEditorMediator caller, ContractBusinessRuleActivityEditorMediatorArgs args);

    public class ContractBusinessRuleActivityEditorMediatorArgs
    {

    }
}
