using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CAlert;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CDialogWindow;
using Bonsai.Utils.String;
using DasContract.Editor.Entities;
using DasContract.Editor.Entities.Integrity.Analysis;
using DasContract.Editor.Entities.Integrity.Contract.Processes;
using DasContract.Editor.Entities.Processes;
using DasContract.Editor.Entities.Processes.Diagrams;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.CContractProcessEditor
{
    public partial class ContractProcessEditor : LoadableComponent
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public EditorContract Contract { get; set; }

        public ContractProcesses Processes => Contract.Processes;

        public string DiagramXML => Contract.Processes.Diagram.DiagramXML;

        public bool EditInProgress { get; protected set; } = false;

        protected ContractProcessEditorMediator Mediator
        {
            get
            {
                if (mediator == null)
                    mediator = new ContractProcessEditorMediator(JSRuntime);
                return mediator;
            }
        }
        ContractProcessEditorMediator mediator = null;

        [Parameter]
        public string Id { get; set; } = Guid.NewGuid().ToString().ToIdFriendly();

        AlertController alertController;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Mediator.InitBPMN(Id, DiagramXML);
                Mediator.OnDiagramChange += (caller, args) =>
                {
                    EditInProgress = true;
                    StateHasChanged();
                };
            }
        }

        public async Task RevertAsync()
        {
            if (!EditInProgress)
                return;

            await Mediator.SetDiagramXML(Id, DiagramXML);
            EditInProgress = false;
            StateHasChanged();
        }


        DialogWindow saveDialogWindow;
        ContractIntegrityAnalysisResult saveIntegrityResult;
        BPMNProcessDiagram diagramToSave;
        public async Task SaveAsync()
        {
            try
            {
                var xml = await Mediator.GetDiagramXML(Id);
                var newDiagram = BPMNProcessDiagram.FromXml(xml);
                diagramToSave = newDiagram;

                //Validate
                Contract.ValidatePotentialDiagram(newDiagram);
                saveIntegrityResult = Contract.AnalyzeIntegrityWhenReplacedWith(newDiagram);

                //Show dialog
                await saveDialogWindow.OpenAsync();
            }
            catch(Exception e)
            {
                alertController.AddAlert("Save unsuccessful: " + e.Message, AlertScheme.Danger); 
            }
        }

        protected async Task ConfirmSaveAsync()
        {
            try
            {
                Contract.ReplaceSafely(diagramToSave);
                alertController.AddAlert("Save successful", AlertScheme.Success);
                EditInProgress = false;
                StateHasChanged();
            }
            catch(Exception e)
            {
                alertController.AddAlert("Save confirm unsuccessful: " + e.Message, AlertScheme.Danger);
            }

            await saveDialogWindow.CloseAsync();
        }
    }
}
