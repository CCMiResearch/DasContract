using DasContract.Editor.Web.Services;
using DasContract.Editor.Web.Services.CamundaEvents;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Pages
{
    public partial class BpmnEditor: ComponentBase
    {
        [Inject]
        private IBpmnEventHandler CamundaEventHandler { get; set; }

        [Inject]
        private IBpmnSynchronizer CamundaSynchronizer { get; set; }

        [Inject]
        private IContractManager ContractManager { get; set; }

        [Inject]
        private IJSRuntime JSRunTime { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                ContractManager.InitializeNewContract();
                CreateEditor();
                InitializeSplitGutter();
            }

        }

        async void CreateEditor()
        {
            await CamundaEventHandler.InitializeHandler();
            await JSRunTime.InvokeVoidAsync("modellerLib.createModeler");   
        }

        async void InitializeSplitGutter()
        {
            await JSRunTime.InvokeVoidAsync("splitterLib.createSplit", ".gutter-col-1");
        }

        async void ExportXML()
        {
            var xml = await JSRunTime.InvokeAsync<string>("MyLib.getDiagramXML");
            Console.WriteLine(xml);
        }
    }
}
