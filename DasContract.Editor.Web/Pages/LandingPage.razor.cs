using DasContract.Editor.Web.Services.Processes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Pages
{
    public partial class LandingPage: ComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IContractManager ContractManager { get; set; }

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

        protected void OnCreateNewClicked()
        {
            ContractManager.InitializeNewContract();
            NavigationManager.NavigateTo("/process");
        }
    }
}
