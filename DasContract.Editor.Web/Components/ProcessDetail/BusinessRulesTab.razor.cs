﻿using DasContract.Abstraction.Processes.Dmn;
using DasContract.Editor.Web.Services.Save;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DasContract.Editor.Web.Components.ProcessDetail
{
    public partial class BusinessRulesTab: ComponentBase, IDisposable
    {
        [Inject]
        private SaveManager SaveManager { get; set; }

        [Inject]
        private IJSRuntime JSRunTime { get; set; }

        [Parameter]
        public Abstraction.Processes.Tasks.BusinessRuleTask BusinessRuleTask { get; set; }

        protected async override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                await JSRunTime.InvokeVoidAsync("dmnModellerLib.createModeler", BusinessRuleTask.BusinessRuleDefinitionXml ?? "");
                SaveManager.StateSaveRequested += HandleSaveRequested;
            }
            
        }

        public async void Dispose()
        {
            SaveManager.StateSaveRequested -= HandleSaveRequested;
            await SaveModel();
        }

        private async Task HandleSaveRequested(object sender, EventArgs e)
        {
            await SaveModel();
        }

        private async Task SaveModel()
        {
            var dmnPlainXml = await JSRunTime.InvokeAsync<string>("dmnModellerLib.saveXml");
            BusinessRuleTask.BusinessRuleDefinitionXml = dmnPlainXml;
            BusinessRuleTask.BusinessRule = Definitions.DeserializePlainDefinition(dmnPlainXml);
        }
    }
}
