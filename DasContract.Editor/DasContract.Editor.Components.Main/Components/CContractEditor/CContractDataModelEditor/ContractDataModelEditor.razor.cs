using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CAlert;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CDialogWindow;
using DasContract.Editor.Entities;
using DasContract.Editor.Entities.DataModels;
using DasContract.Editor.Entities.DataModels.Entities;
using DasContract.Editor.Entities.Integrity.Analysis;
using DasContract.Editor.Entities.Integrity.Contract.DataModel;
using DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.CContractDataModelEditor
{
    public partial class ContractDataModelEditor: ContractEditorSectionBase
    {
        [Parameter]
        public EditorContract Contract { get; set; }

        public ContractDataModel DataModel => Contract.DataModel;


        AlertController alertController;

        //--------------------------------------------------
        //                    DELETE
        //--------------------------------------------------
        DialogWindow deleteDialogWindow;
        ContractIntegrityAnalysisResult deleteEntityAnalysis;
        ContractEntity entityToDelete = null;
        public async Task DeleteEntityAsync(int index)
        {
            entityToDelete = Contract.DataModel.Entities[index];
            deleteEntityAnalysis = Contract.AnalyzeIntegrityOf(entityToDelete);
            await deleteDialogWindow.OpenAsync();
        }

        async Task ConfirmDeleteAsync()
        {
            try
            {
                Contract.RemoveSafely(entityToDelete);
                alertController.AddAlert("Delete successful", AlertScheme.Success);
                StateHasChanged();
            }
            catch (Exception)
            {
                alertController.AddAlert("Something went wrong :(", AlertScheme.Danger);
            }

            await deleteDialogWindow.CloseAsync();
        }

        //--------------------------------------------------
        //                     ADD
        //--------------------------------------------------

        DialogWindow createDialogWindow;
        public async Task AddEntityAsync()
        {
            await createDialogWindow.OpenAsync();
        }

        AddNewEntityFormModel newEntityModel = AddNewEntityFormModel.Empty();
        protected async Task ConfirmAddEntityAsync()
        {
            Contract.AddSafely(newEntityModel.ToContractEntity());
            //Contract.DataModel.Entities.Add(newEntityModel.ToContractEntity());

            await createDialogWindow.CloseAsync();
            alertController.AddAlert("New entity added successfuly", AlertScheme.Success);
            ResetNewEntityModel();
        }

        protected void ResetNewEntityModel()
        {
            newEntityModel = AddNewEntityFormModel.Empty();
        }
    }
}
