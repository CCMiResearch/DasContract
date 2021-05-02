using DasContract.Editor.Web.Services;
using DasContract.Editor.Web.Services.BpmnEvents;
using DasContract.Editor.Web.Services.EditElement;
using DasContract.Editor.Web.Services.Processes;
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
        private EditElementService EditElementService { get; set; }

        [Inject]
        protected ResizeHandler ResizeHandler { get; set; }

        [Inject]
        private IJSRuntime JSRunTime { get; set; }

        private bool ShowDetailBar { get; set; } = false;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                ContractManager.InitializeNewContract();
                
                CreateEditor();
            }

            if (ShowDetailBar)
                InitializeSplitGutter();
        }

        protected override async void OnInitialized()
        {
            base.OnInitialized();
            EditElementService.EditElementChanged += HandleEditElementChanged;
        }

        private void HandleEditElementChanged(object sender, EditElementEventArgs args)
        {
            ShowDetailBar = args.processElement != null;

            StateHasChanged();
        }

        async void CreateEditor()
        {
            await CamundaEventHandler.InitializeHandler();
            await JSRunTime.InvokeVoidAsync("modellerLib.createModeler");   
        }

        async void InitializeSplitGutter()
        {
            await JSRunTime.InvokeVoidAsync("splitterLib.createSplit", ".gutter-col-1");
            await ResizeHandler.InitializeHandler();
        }

        async void ExportXML()
        {
            var xml = await JSRunTime.InvokeAsync<string>("MyLib.getDiagramXML");
            Console.WriteLine(xml);
        }
    }
}
