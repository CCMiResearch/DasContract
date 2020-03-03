using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CAlert;
using DasContract.Editor.DataPersistence.Entities;
using DasContract.Editor.Entities;
using DasContract.Editor.Entities.Serialization.XML;
using DasContract.Editor.Pages.Main.Services.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DasContract.Editor.Pages.Main.Pages.ContractFileSessionPages
{
    public partial class ContractFileSessionEditor : PageBase
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        protected ContractFileSessionService ContractFileSessionService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        public ContractFileSession ContractFileSession { get; set; }

        public EditorContract Contract { get; set; }
        AlertController contractAlertController;


        protected override void OnInitialized()
        {
            base.OnInitialized();
            Breadcrumbs
                .AddHome("DasContract")
                .AddCrumb(ContractFileSessionIndex.Breadcrumb)
                .AddLastCrumb("Contract session " + Id);
        }

        Exception initialException = null;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadCurrentContractAsync();
        }

        async Task LoadCurrentContractAsync()
        {
            Loading = true;

            try
            {
                ContractFileSession = await ContractFileSessionService.GetAsync(Id);
                Contract = EditorContractXML.From(ContractFileSession.SerializedContract);
            }
            catch (Exception e)
            {
                initialException = e;
            }

            Loading = false;
        }

        async Task DownloadAsync()
        {
            //await JSRuntime.InvokeVoidAsync("open", ContractFileSessionService.DownloadUrl(Id), "_blank");
            NavigationManager.NavigateTo(ContractFileSessionService.DownloadUrl(Id));
        }

        async Task SaveAsync()
        {
            ContractFileSession.SerializedContract = EditorContractXML.To(Contract);
            try
            {
                await ContractFileSessionService.UpdateAsync(ContractFileSession);
                contractAlertController.AddAlert("Contract saved successfuly", AlertScheme.Success);
            }
            catch(Exception)
            {
                contractAlertController.AddAlert("Error while saving contract occured", AlertScheme.Danger);
            }

            //await LoadCurrentContractAsync();
        }
    }
}
