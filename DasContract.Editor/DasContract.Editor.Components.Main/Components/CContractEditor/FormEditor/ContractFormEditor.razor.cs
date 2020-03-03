using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CAlert;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CDialogWindow;
using DasContract.Editor.Entities;
using DasContract.Editor.Entities.DataModels.Entities;
using DasContract.Editor.Entities.Forms;
using DasContract.Editor.Entities.Integrity.Analysis;
using DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities;
using DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities.Properties.Primitive;
using DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities.Properties.Reference;
using DasContract.Editor.Entities.Integrity.Contract.Forms;
using DasContract.Editor.Entities.Integrity.Contract.Processes.Process.Activities;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.FormEditor
{
    public partial class ContractFormEditor: LoadableComponent
    {
        [Parameter]
        public EditorContract Contract { get; set; }

        [Parameter]
        public ContractForm Form { get; set; }

        AlertController alertController;

        //--------------------------------------------------
        //                    DELETE
        //--------------------------------------------------
        DialogWindow deleteDialogWindow;
        ContractIntegrityAnalysisResult deleteFieldAnalysis;
        ContractFormField fieldToDelete = null;
        public async Task DeleteFieldAsync(int index)
        {
            fieldToDelete = Form.Fields[index];
            deleteFieldAnalysis = Contract.AnalyzeIntegrityOf(Form, fieldToDelete);
            await deleteDialogWindow.OpenAsync();
        }

        async Task ConfirmDeleteAsync()
        {
            try
            {
                Contract.RemoveSafely(Form, fieldToDelete);
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
        public async Task AddFieldAsync()
        {
            await createDialogWindow.OpenAsync();
        }

        AddNewFieldFormModel newFieldModel = AddNewFieldFormModel.Empty();
        protected async Task ConfirmAddFieldAsync()
        {
            Contract.AddSafely(Form, newFieldModel.ToFormField());
            //Contract.DataModel.Entities.Add(newEntityModel.ToContractEntity());

            await createDialogWindow.CloseAsync();
            alertController.AddAlert("New field added successfuly", AlertScheme.Success);
            ResetNewFieldModel();
        }

        protected void ResetNewFieldModel()
        {
            newFieldModel = AddNewFieldFormModel.Empty();
        }
    }
}
