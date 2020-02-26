using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Editor.DataPersistence.Entities;
using DasContract.Editor.Pages.Main.Services.Entities;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Pages.Main.Pages.ContractFileSessionPages
{
    public partial class ContractFileSessionEditor : PageBase
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        protected ContractFileSessionService ContractFileSessionService { get; set; }

        public ContractFileSession ContractFileSession { get; set; }

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
            }
            catch (Exception e)
            {
                initialException = e;
            }

            Loading = false;
        }
    }
}
