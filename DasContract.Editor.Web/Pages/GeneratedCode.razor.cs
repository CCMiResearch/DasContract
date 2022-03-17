using DasContract.Editor.Web.Services.Converter;
using DasContract.Editor.Web.Services.Processes;
using DasContract.Editor.Web.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Pages
{
    public partial class GeneratedCode: ComponentBase, IDisposable
    {
        [Inject]
        IContractManager ContractManager { get; set; }

        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        IConverterService ConverterService { get; set; }

        [CascadingParameter]
        protected MainLayout Layout { get; set; }

        [Parameter]
        public string Platform { get; set; }

        private bool _conversionSuccessful;
        private string _conversionData;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            CreateToolbarItems();
            
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetPlatformAndConvert();
        }

        protected void SetPlatformAndConvert()
        {
            switch (Platform)
            {
                case "solidity":
                    ConverterService.SetConversionTarget(ConversionTarget.SOLIDITY);
                    break;
                case "plutus":
                    ConverterService.SetConversionTarget(ConversionTarget.PLUTUS);
                    break;
            }
            _conversionSuccessful = ContractManager.ConvertContract(out _conversionData);
        }


        private void CreateToolbarItems()
        {
            var saveCodeToolbarItem = new ToolBarItem { 
                Name = "Download code", 
                IconPath = "dist/icons/file-text.svg",
                Id = "download-code"
                };
            saveCodeToolbarItem.OnClick += HandleSaveCodeClicked;
            Layout.AddToolbarItem(saveCodeToolbarItem);
        }

        private async void HandleSaveCodeClicked(object sender, MouseEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("fileSaverLib.saveFile", "generated.sol", ContractManager.GeneratedContract);
        }

        public void Dispose()
        {
            Layout.RemoveToolbarItem("download-code");
        }
    }
}
