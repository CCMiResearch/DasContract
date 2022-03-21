using DasContract.Editor.Web.Services.JsInterop;
using DasContract.Editor.Web.Services.LocalStorage;
using DasContract.Editor.Web.Services.Processes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Pages
{
    public partial class LandingPage: ComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IContractManager ContractManager { get; set; }

        [Inject]
        protected ISaveGuardJsCommunicator SaveGuardJsCommunicator { get; set; }

        [Inject]
        protected IContractStorage ContractStorage { get; set; }

        [Inject]
        private HttpClient HttpClient { get; set; }

        protected IList<StoredContractLink> ContractLinks { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ContractLinks = await ContractStorage.GetAllContractLinks();
            StateHasChanged();
            await base.OnInitializedAsync();
        }

        protected async void RemoveStoredContract(string contractId)
        {
            if (await SaveGuardJsCommunicator.DisplayAndCollectConfirmation("Are you sure?"))
            {
                await ContractStorage.RemoveContract(contractId);
                ContractLinks = await ContractStorage.GetAllContractLinks();
                StateHasChanged();
            }
        }

        protected async void OnInputFileProvided(InputFileChangeEventArgs args)
        {
            string content;
            using (StreamReader reader = new StreamReader(args.File.OpenReadStream()))
            {
                content = await reader.ReadToEndAsync();
            }
            ContractManager.RestoreContract(content);
            NavigationManager.NavigateTo("/process");
        }

        protected async Task OnOpenRecentClicked(string id)
        {
            var contractXml = await ContractStorage.GetContractXml(id);
            ContractManager.RestoreContract(contractXml);
            NavigationManager.NavigateTo("/process");
        }

        protected async Task OpenExampleContract(string contractAddress)
        {
            var contract = await HttpClient.GetStringAsync(contractAddress);
            ContractManager.RestoreContract(contract);
            NavigationManager.NavigateTo("/process");
        }

        protected void OnCreateNewClicked()
        {
            ContractManager.InitializeNewContract();
            NavigationManager.NavigateTo("/process");
        }
    }
}
