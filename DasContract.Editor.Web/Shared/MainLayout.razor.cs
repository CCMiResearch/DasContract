using DasContract.Editor.Web.Services;
using DasContract.Editor.Web.Services.Processes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        public IList<ToolBarItem> ToolBarItems { get; set; } = new List<ToolBarItem>();

        [Inject]
        private IContractManager ContractManager { get; set; }

        [Inject]
        private SaveManager SaveManager { get; set; }

        [Inject]
        private IJSRuntime JSRunTime { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ToolBarItems = CreateSharedToolbarItems();
        }

        //Creates toolbar items that are shared across all editors
        protected IList<ToolBarItem> CreateSharedToolbarItems()
        {
            var downloadItem = new ToolBarItem { IconPath = "icons/download.svg", Description = "Save as .dascontract", Name = "Save as" };
            downloadItem.OnClick += HandleSaveContractClicked;
            return new List<ToolBarItem>
            {
               downloadItem
            };
        }

        protected async void HandleSaveContractClicked(object sender, MouseEventArgs args)
        {
            //Request a force save
            await SaveManager.RequestSave();
            await JSRunTime.InvokeVoidAsync("fileSaverLib.saveFile", "contract.dascontract", ContractManager.SerializeContract());
        }
    }
}
