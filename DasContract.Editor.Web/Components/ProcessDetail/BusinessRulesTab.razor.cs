using DasContract.Abstraction.Processes.Tasks;
using DasContract.Editor.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.ProcessDetail
{
    public partial class BusinessRulesTab: ComponentBase, IDisposable
    {
        [Inject]
        private SaveManager SaveManager { get; set; }

        [Inject]
        private IJSRuntime JSRunTime { get; set; }

        [Parameter]
        public BusinessRuleTask BusinessRuleTask { get; set; }

        protected async override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                await JSRunTime.InvokeVoidAsync("dmnModellerLib.createModeler", BusinessRuleTask.BusinessRuleDefinitionXml ?? "");
                SaveManager.SaveRequested += HandleSaveRequested;
            }
            
        }

        public async void Dispose()
        {
            SaveManager.SaveRequested -= HandleSaveRequested;
            await SaveModel();
        }

        private async void HandleSaveRequested(object sender, EventArgs e)
        {
            await SaveModel();
        }

        private async System.Threading.Tasks.Task SaveModel()
        {
            var dmnXml = await JSRunTime.InvokeAsync<string>("dmnModellerLib.saveXml");
            BusinessRuleTask.BusinessRuleDefinitionXml = dmnXml;
        }
    }
}
