using DasContract.Abstraction.Data;
using DasContract.Editor.Web.Components.Buttons;
using DasContract.Editor.Web.Services.ContractManagement;
using DasContract.Editor.Web.Services.DataModel;
using DasContract.Editor.Web.Services.Resize;
using DasContract.Editor.Web.Services.Save;
using DasContract.Editor.Web.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Scriban;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Pages
{
    public partial class DataEditor : ComponentBase, IDisposable
    {
        [Inject]
        protected ResizeHandler ResizeHandler { get; set; }

        [Inject]
        private IJSRuntime JSRunTime { get; set; }

        [Inject]
        private IDataModelConverter DataModelConverter { get; set; }

        [Inject]
        private IContractManager ContractManager { get; set; }

        protected RefreshButton Refresh { get; set; }

        [CascadingParameter]
        protected MainLayout Layout { get; set; }

        protected string MermaidScript { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await InitializeSplitGutter();
                await RefreshDiagram();
                CreateToolbarItems();
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

        }

        async Task InitializeSplitGutter()
        {
            await JSRunTime.InvokeVoidAsync("splitterLib.createSplit", ".gutter-col-1");
            await ResizeHandler.InitializeHandler();
        }

        protected async Task DataModelXmlChanged(string script)
        {
            ContractManager.SetDataModelXml(script);
            if (Refresh.AutoRefresh && !string.IsNullOrEmpty(script))
            {
                await RefreshDiagram();
            }
        }

        private void CreateToolbarItems()
        {
            var saveDiagramSvgItem = new ToolBarItem
            {
                Name = "Diagram as svg",
                IconName = "filetype-svg",
                Id = "download-svg"
            };
            saveDiagramSvgItem.OnClick += HandleSaveToSvgClicked;
            Layout.AddToolbarItem(saveDiagramSvgItem);
            var saveDiagramPngItem = new ToolBarItem
            {
                Name = "Diagram as png",
                IconName = "file-earmark-image",
                Id = "download-png"
            };
            saveDiagramPngItem.OnClick += HandleSaveToPngClicked;
            Layout.AddToolbarItem(saveDiagramPngItem);
        }


        private async void HandleSaveToSvgClicked(object sender, MouseEventArgs args)
        {
            await JSRunTime.InvokeVoidAsync("mermaidLib.downloadSVG", ContractManager.GetContractName());
        }

        private async void HandleSaveToPngClicked(object sender, MouseEventArgs args)
        {
            await JSRunTime.InvokeVoidAsync("mermaidLib.downloadPNG", ContractManager.GetContractName());
        }

        private async Task RefreshDiagram()
        {
            try
            {
                var dataTypes = ContractManager.GetDataTypes();
                MermaidScript = DataModelConverter.ConvertToMermaid(dataTypes);

                await JSRunTime.InvokeVoidAsync("mermaidLib.renderMermaidDiagram", "mermaid-canvas", MermaidScript);
            }
            catch (JSException)
            {
                Console.WriteLine("Could not render mermaid diagram");
            }
            catch (DataModelConversionException)
            {
                Console.WriteLine("Error when converting the model");
            }
        }

        public void Dispose()
        {
            Layout.RemoveToolbarItem("download-svg");
            Layout.RemoveToolbarItem("download-png");
        }
    }
}
