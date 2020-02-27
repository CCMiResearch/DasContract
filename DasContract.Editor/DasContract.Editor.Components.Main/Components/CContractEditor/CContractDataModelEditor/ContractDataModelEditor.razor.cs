using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CAlert;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CDialogWindow;
using DasContract.Editor.Entities;
using DasContract.Editor.Entities.DataModels;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.CContractDataModelEditor
{
    public partial class ContractDataModelEditor: ContractEditorSectionBase
    {
        [Parameter]
        public EditorContract Contract { get; set; }

        AlertController alertController;

        //--------------------------------------------------
        //                    DELETE
        //--------------------------------------------------
        public async Task DeleteEntityAsync(int index)
        {
            throw new NotImplementedException();
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
            Contract.DataModel.Entities.Add(newEntityModel.ToContractEntity());

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
