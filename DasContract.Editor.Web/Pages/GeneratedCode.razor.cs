using DasContract.Editor.Web.Services.Converter;
using DasContract.Editor.Web.Services.Processes;
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
    public partial class GeneratedCode: ComponentBase, IDisposable
    {
        [Inject]
        IContractManager ContractManager { get; set; }

        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [CascadingParameter]
        protected MainLayout Layout { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            CreateToolbarItems();
            ContractManager.ConvertContract(new SolidityConverterService());
        }

        private void CreateToolbarItems()
        {
            var saveCodeToolbarItem = new ToolBarItem { 
                Name = "Download code", 
                IconPath = "icons/file-text.svg",
                Id = "download-code"
                };
            saveCodeToolbarItem.OnClick += HandleSaveCodeClicked;
            Layout.AddToolbarItem(saveCodeToolbarItem);
        }

        private async void HandleSaveCodeClicked(object sender, MouseEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("fileSaverLib.saveFile", "generated.sol", ContractManager.GeneratedContract);
        }

        public void Dispose()
        {
            Layout.RemoveToolbarItem("download-code");
        }
    }
}
