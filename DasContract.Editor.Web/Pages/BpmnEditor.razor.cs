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
    public partial class BpmnEditor : ComponentBase, IAsyncDisposable
    {
        [Inject]
        private IBpmnEventHandler BpmnEventHandler { get; set; }

        [Inject]
        private IBpmnSynchronizer BpmnSynchronizer { get; set; }

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

        private bool _restoreBpmnElement = false;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                BpmnSynchronizer.InitializeOrRestoreBpmnEditor("canvas");
                CreateSaveDiagramToolbarButton();
            }

            if (_restoreBpmnElement)
            {
                BpmnSynchronizer.InitializeOrRestoreBpmnEditor("canvas");
                _restoreBpmnElement = false;
            }

            if (ShowDetailBar)
            {
                InitializeSplitGutter();
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (EditElementService.EditElement != null)
            {
                ShowDetailBar = true;
            }

            EditElementService.EditElementChanged += HandleEditElementChanged;
            SaveManager.StateSaveRequested += SaveDiagramXml;
            UserFormService.RefreshRequested += HandleUserFormPreviewChanged;
        }

        private void CreateSaveDiagramToolbarButton()
        {
            var saveDiagramAsSvgButton = new ToolBarItem
            {
                IconName = "filetype-svg",
                Id = "bpmn-download-svg",
                Name = "Diagram as svg"
            };
            saveDiagramAsSvgButton.OnClick += HandleSaveDiagramAsSvg;
            Layout.AddToolbarItem(saveDiagramAsSvgButton);

            var saveDiagramAsPngButton = new ToolBarItem
            {
                IconName = "file-earmark-image",
                Id = "bpmn-download-png",
                Name = "Diagram as png"
            };
            saveDiagramAsPngButton.OnClick += HandleSaveDiagramAsPng;
            Layout.AddToolbarItem(saveDiagramAsPngButton);
        }

        private async void HandleSaveDiagramAsSvg(object sender, MouseEventArgs args)
        {
            await JSRunTime.InvokeVoidAsync("modellerLib.saveAsSvg", ContractManager.GetContractName());
        }

        private async void HandleSaveDiagramAsPng(object sender, MouseEventArgs args)
        {
            await JSRunTime.InvokeVoidAsync("modellerLib.saveAsPng", ContractManager.GetContractName());
        }

        private void HandleEditElementChanged(object sender, EditElementEventArgs args)
        {
            ShowDetailBar = args.processElement != null;
            StateHasChanged();
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

        private void HandleUserFormPreviewChanged()
        {
            if (!UserFormService.IsPreviewOpen)
                _restoreBpmnElement = true;
            StateHasChanged();
        }

        public async ValueTask DisposeAsync()
        {
            await SaveManager.RequestStateSave();
            SaveManager.StateSaveRequested -= SaveDiagramXml;
            UserFormService.RefreshRequested -= HandleUserFormPreviewChanged;
            Layout.RemoveToolbarItem("bpmn-download-png");
            Layout.RemoveToolbarItem("bpmn-download-svg");
        }
    }
}
