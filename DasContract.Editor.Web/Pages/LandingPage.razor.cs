using DasContract.Editor.Web.Services.ExamplesLoader;
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
        private IExampleLoader ExampleLoader { get; set; }

        [Inject]
        protected IContractStorage ContractStorage { get; set; }



        protected IList<StoredContractLink> ContractLinks { get; set; } = new List<StoredContractLink>();
        protected IList<ExampleContract> ExampleContracts { get; set; } = new List<ExampleContract>();

        protected override async Task OnParametersSetAsync()
        {
            ContractLinks = await ContractStorage.GetAllContractLinks();
            ExampleContracts = await ExampleLoader.ReadManifest();
            await base.OnParametersSetAsync();
        }

        protected async void RemoveStoredContract(string contractId)
        {
            if (await SaveGuardJsCommunicator.DisplayAndCollectConfirmation("This will permanently remove the contract. Are you sure?"))
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

        protected async Task OpenExampleContract(string exampleName)
        {
            var contract = await ExampleLoader.ReadContract(exampleName);
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
