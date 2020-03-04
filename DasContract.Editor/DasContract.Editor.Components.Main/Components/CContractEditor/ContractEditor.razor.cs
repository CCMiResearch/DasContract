using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Editor.Components.Main.Components;
using DasContract.Editor.Entities;
using DasContract.Editor.Migrator.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Components.Main.Components.CContractEditor
{
    public partial class ContractEditor : LoadableComponent
    {
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
                    contract.GetMigrator().OnMigrationsChange -= OnMigrationsChange;
                contract = value;
                
                //Setup the new contract
                if (contract != null)
                {
                    contract.StartTracingSteps();
                    contract.GetMigrator().OnMigrationsChange += OnMigrationsChange;
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

        public void StepBackward()
        {
            Contract.GetMigrator().StepBackward();
            StateHasChanged();
        }

        public bool HasStepBackward() => Contract.GetMigrator().HasStepBackward();

        public void StepForward()
        {
            Contract.GetMigrator().StepForward();
            StateHasChanged();
        }

        public bool HasStepForward() => Contract.GetMigrator().HasStepForward();


        private void OnMigrationsChange(IMigrator caller, IMigratorArgs args)
        {
            StateHasChanged();
        }

    }
}
