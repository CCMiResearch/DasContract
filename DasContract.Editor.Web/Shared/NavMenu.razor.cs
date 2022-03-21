using DasContract.Blockchain.Solidity.Converters;
using DasContract.Editor.Web.Services;
using DasContract.Editor.Web.Services.Converter;
using DasContract.Editor.Web.Services.JsInterop;
using DasContract.Editor.Web.Services.Processes;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Shared
{
    public partial class NavMenu : ComponentBase
    {
        [Inject]
        protected IContractManager ContractManager { get; set; }

        [Inject]
        protected SaveManager SaveManager { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        protected ISaveGuardJsCommunicator SaveGuardJsCommunicator { get; set; }

        [Inject]
        protected IConverterService ConverterService { get; set; }

        protected ElementReference NameInputReference { get; set; }

        protected string ContractName { get { return ContractManager.GetContractName(); } set { ContractManager.SetContractName(value); } }

        [CascadingParameter]
        protected MainLayout Layout { get; set; }

        protected bool IsNameBeingEdited { get; set; }

        protected void SetConversionTargetAndNavigate(ConversionTarget conversionTarget)
        {
            ConverterService.SetConversionTarget(conversionTarget);
            NavigationManager.NavigateTo("generated");
            StateHasChanged();
        }

        protected async Task NavigateToLandingPage()
        {
            if (!ContractManager.CanSafelyExit() && !await SaveGuardJsCommunicator.DisplayAndCollectConfirmation())
            {
                return;
            }
            NavigationManager.NavigateTo("");
        }

        protected async Task SaveContract()
        {
            try
            {
                await SaveManager.RequestSave();
                await Layout.CreateAlert("Contract saved");
            }
            catch (Exception)
            {
                await Layout.CreateAlert("Could not save contract");
            }
        }

        protected string BaseRelativePath()
        {
            return NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        }
    }
}
