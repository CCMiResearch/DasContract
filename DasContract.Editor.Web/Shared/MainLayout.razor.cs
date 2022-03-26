using Blazored.LocalStorage;
using DasContract.Editor.Web.Services.ContractManagement;
using DasContract.Editor.Web.Services.Save;
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
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private SaveManager SaveManager { get; set; }

        [Inject]
        private IJSRuntime JSRunTime { get; set; }

        [Inject]
        private ILocalStorageService LocalStorage { get; set; }

        protected IList<string> Alerts { get; set; } = new List<string>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Console.WriteLine("Initializing");
            if (!ContractManager.IsContractInitialized())
                NavigationManager.NavigateTo("");

            ToolBarItems = CreateSharedToolbarItems();
        }

        public void AddToolbarItem(ToolBarItem toolbarItem)
        {
            ToolBarItems.Add(toolbarItem);
            StateHasChanged();
        }

        public void RemoveToolbarItem(string ItemId)
        {
            ToolBarItems = ToolBarItems.Where(i => i.Id != ItemId).ToList();
            StateHasChanged();
        }

        public async Task CreateAlert(string message, int lifespanMs = 1500)
        {
            Alerts.Add(message);
            StateHasChanged();
            await Task.Delay(lifespanMs);
            Alerts.Remove(message);
            StateHasChanged();
        }

        //Creates toolbar items that are shared across all editors
        protected IList<ToolBarItem> CreateSharedToolbarItems()
        {
            var downloadItem = new ToolBarItem { 
                IconName = "file-earmark-medical", 
                Description = "Save as .dascontract", 
                Name = "Contract",
                Id = "download-contract"};
            downloadItem.OnClick += HandleDownloadContractClicked;
            return new List<ToolBarItem>
            {
               downloadItem
            };
        }

        protected async void HandleDownloadContractClicked(object sender, MouseEventArgs args)
        {
            //Request a force save
            await SaveManager.RequestStateSave();
            var serializedContract = ContractManager.SerializeContract();
            var contractName = string.IsNullOrEmpty(ContractManager.GetContractName()) ? "contract" : ContractManager.GetContractName();
            await JSRunTime.InvokeVoidAsync("fileSaverLib.saveContract", $"{contractName}.dascontract", serializedContract);
        }
    }
}
