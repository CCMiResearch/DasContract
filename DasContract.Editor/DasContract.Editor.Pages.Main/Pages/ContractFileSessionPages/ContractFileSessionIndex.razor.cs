using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CAlert;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CBreadcrumbs;
using DasContract.Editor.DataPersistence.Entities;
using DasContract.Editor.Entities;
using DasContract.Editor.Entities.Serialization.XML;
using DasContract.Editor.Pages.Main.Services.Entities;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Pages.Main.Pages.ContractFileSessionPages
{
    public partial class ContractFileSessionIndex: PageBase
    {
        [Inject]
        protected ContractFileSessionService ContractFileSessionService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        string newSessionId { get; } = Guid.NewGuid().ToString();
        string newSessionUrl => "/api/ContractFileSession/InitiateWithFile/" + newSessionId;

        public static Breadcrumb Breadcrumb { get; } = new Breadcrumb("File session", "/ContractFileSession");

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Breadcrumbs
                .AddHome("DasContract")
                .AddLastCrumb(Breadcrumb);
        }


        //--------------------------------------------------
        //              CREATE NEW CONTRACT
        //--------------------------------------------------
        AlertController newContractAlertController;
        async Task CreateNewContractAsync()
        {
            var newContract = new EditorContract();
            var newSession = ContractFileSession.FromContract(newContract);
            newSession.Id = newSessionId;

            Loading = true;

            try
            {
                await ContractFileSessionService.InsertAsync(newSession);
                NavigationManager.NavigateTo("/ContractFileSession/" + newSession.Id);
            }
            catch (Exception)
            {
                newContractAlertController.AddAlert("Something went wrong :(", AlertScheme.Danger);
            }

            Loading = false;
        }

        //--------------------------------------------------
        //               CREATE NEW SESSION
        //--------------------------------------------------
        Task CreateNewSessionAsync()
        {
            Loading = true;

            NavigationManager.NavigateTo("/ContractFileSession/" + newSessionId);

            Loading = false;
            return Task.CompletedTask;
        }
    }
}
