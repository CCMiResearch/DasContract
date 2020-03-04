using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Editor.Components.Main.Components;
using DasContract.Editor.Components.Main.Components.CContractEditor.ProcessEditor;
using DasContract.Editor.Components.Main.Services.BusinessRuleActivityEditor;
using DasContract.Editor.Entities;
using DasContract.Editor.Entities.Processes;
using DasContract.Editor.Entities.Processes.Diagrams;
using DasContract.Editor.Migrator.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Components.Main.Components.CContractEditor
{
    public partial class ContractEditor : LoadableComponent
    {
        [Inject]
        public ContractBusinessRuleActivityEditorService ContractBusinessRuleActivityEditorService { get; set; }

        [Parameter]
        public EditorContract Contract
        {
            get => contract;
            set
            {
                if (value == contract)
                    return;

                //Unbind the old contract
                if (contract != null)
                {
                    contract.GetMigrator().OnMigrationsChange -= OnMigrationsChange;
                    //contract.Processes.OnDiagramChange -= OnDiagramChange;
                }
                contract = value;
                
                //Setup the new contract
                if (contract != null)
                {
                    contract.StartTracingSteps();
                    contract.GetMigrator().OnMigrationsChange += OnMigrationsChange;
                    //contract.Processes.OnDiagramChange += OnDiagramChange;
                }
            }
        }


        EditorContract contract = null;

        public ContractEditorTab OpenedTab { get; protected set; } = ContractEditorTab.General;

        [Parameter]
        public EventCallback<EditorContract> OnSave { get; set; }

        [Parameter]
        public EventCallback<EditorContract> OnDownload { get; set; }

        public async Task SaveAndDownloadAsync()
        {
            await SaveAsync();
            await DownloadAsync();
        }

        public async Task SaveAsync()
        {
            await OnSave.InvokeAsync(Contract);
        }

        public async Task DownloadAsync()
        {
            await OnDownload.InvokeAsync(Contract);
        }

        public void Open(ContractEditorTab tab)
        {
            if (OpenedTab == tab)
                return;

            OpenedTab = tab;
        }

        protected async Task UpdateOpenedTab()
        {
            if (OpenedTab == ContractEditorTab.Process)
                await RedrawProcessDiagram();
            else if (OpenedTab == ContractEditorTab.Activities)
                await ContractBusinessRuleActivityEditorService.RedrawActiveEditorsAsync();
        }

        public async Task StepBackwardAsync()
        {
            Contract.GetMigrator().StepBackward();
            await UpdateOpenedTab();
            StateHasChanged();
        }

        public bool HasStepBackward() => Contract.GetMigrator().HasStepBackward();

        public async Task StepForwardAsync()
        {
            Contract.GetMigrator().StepForward();
            await UpdateOpenedTab();
            StateHasChanged();
        }

        public bool HasStepForward() => Contract.GetMigrator().HasStepForward();


        void OnMigrationsChange(IMigrator caller, IMigratorArgs args)
        {
            StateHasChanged();
        }


        ContractProcessEditor processEditor;
        async Task RedrawProcessDiagram()
        {
            if (processEditor == null)
                return;

            await processEditor.RedrawAsync();
        }
    }
}
