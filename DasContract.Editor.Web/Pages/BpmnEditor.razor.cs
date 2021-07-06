using DasContract.Editor.Web.Services;
using DasContract.Editor.Web.Services.BpmnEvents;
using DasContract.Editor.Web.Services.EditElement;
using DasContract.Editor.Web.Services.Processes;
using DasContract.Editor.Web.Services.UserForm;
using DasContract.Editor.Web.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Pages
{
    public partial class BpmnEditor: ComponentBase, IAsyncDisposable
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
        protected UserFormService UserFormService { get; set; }

        [Inject]
        private SaveManager SaveManager { get; set; }

        [Inject]
        private IJSRuntime JSRunTime { get; set; }

        [CascadingParameter]
        protected MainLayout Layout { get; set; }

        private bool ShowDetailBar { get; set; } = false;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if(!ContractManager.IsContractInitialized())
                    ContractManager.InitializeNewContract();
                InitializeBpmnEditor();
            }

            if (ShowDetailBar)
                InitializeSplitGutter();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            EditElementService.EditElementChanged += HandleEditElementChanged;
            UserFormService.IsPreviewOpenChanged += () => StateHasChanged();
        }

        private void HandleEditElementChanged(object sender, EditElementEventArgs args)
        {
            ShowDetailBar = args.processElement != null;
            StateHasChanged();
        }

        async void InitializeBpmnEditor()
        {
            var bpmnEditorDiagram = ContractManager.GetProcessDiagram();
            await CamundaEventHandler.InitializeHandler();
            await JSRunTime.InvokeVoidAsync("modellerLib.createModeler", bpmnEditorDiagram ?? "");
            SaveManager.SaveRequested += SaveDiagramXml;
        }

        async void InitializeSplitGutter()
        {
            await JSRunTime.InvokeVoidAsync("splitterLib.createSplit", ".gutter-col-1");
            await ResizeHandler.InitializeHandler();
        }

        private async Task SaveDiagramXml(object sender, EventArgs e)
        {
            var diagramXml = await JSRunTime.InvokeAsync<string>("modellerLib.getDiagramXML");
            ContractManager.SetProcessDiagram(diagramXml);
        }

        public async ValueTask DisposeAsync()
        {
            await SaveManager.RequestSave();
            SaveManager.SaveRequested -= SaveDiagramXml;
            UserFormService.IsPreviewOpenChanged -= () => StateHasChanged();
        }
    }
}
