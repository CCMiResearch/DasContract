using Blazored.LocalStorage;
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

        [Inject]
        private ILocalStorageService LocalStorage { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ToolBarItems = CreateSharedToolbarItems();
        }

        public void AddToolbarItem(ToolBarItem toolbarItem)
        {
            ToolBarItems.Add(toolbarItem);
            StateHasChanged();
        }

        public void RemoveToolbarItem(string ItemName)
        {
            ToolBarItems = ToolBarItems.Where(i => i.Name != ItemName).ToList();
            StateHasChanged();
        }

        //Creates toolbar items that are shared across all editors
        protected IList<ToolBarItem> CreateSharedToolbarItems()
        {
            var downloadItem = new ToolBarItem { IconPath = "icons/download.svg", Description = "Save as .dascontract", Name = "Save contract" };
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
            var serializedContract = ContractManager.SerializeContract();
            await LocalStorage.SetItemAsync("contract", serializedContract);
            await JSRunTime.InvokeVoidAsync("fileSaverLib.saveFile", "contract.dascontract", serializedContract);
        }
    }
}
