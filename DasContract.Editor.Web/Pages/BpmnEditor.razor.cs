using DasContract.Editor.Web.Services;
using DasContract.Editor.Web.Services.BpmnEvents;
using DasContract.Editor.Web.Services.EditElement;
using DasContract.Editor.Web.Services.Processes;
using DasContract.Editor.Web.Shared;
using DasContract.JSON;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
        }

        protected IList<ToolBarItem> CreateToolBarItems()
        {
            var downloadItem = new ToolBarItem { IconPath = "icons/download.svg", Tooltip = "Save as .dascontract" };
            downloadItem.OnClick += HandleSaveContractClicked;
            return new List<ToolBarItem>
            {
               downloadItem
            };
        }

        protected async void HandleSaveContractClicked(object sender, MouseEventArgs args)
        {
            await JSRunTime.InvokeVoidAsync("fileSaverLib.saveFile", "contract.dascontract", await ContractManager.SerializeContract());
        }

        private void HandleEditElementChanged(object sender, EditElementEventArgs args)
        {
            ShowDetailBar = args.processElement != null;

            StateHasChanged();
        }

        async void InitializeBpmnEditor()
        {
            var bpmnEditorDiagram = ContractManager.Contract.ProcessDiagram;
            await CamundaEventHandler.InitializeHandler();
            await JSRunTime.InvokeVoidAsync("modellerLib.createModeler", bpmnEditorDiagram ?? "");   
        }

        async void InitializeSplitGutter()
        {
            await JSRunTime.InvokeVoidAsync("splitterLib.createSplit", ".gutter-col-1");
            await ResizeHandler.InitializeHandler();
        }

    }
}
