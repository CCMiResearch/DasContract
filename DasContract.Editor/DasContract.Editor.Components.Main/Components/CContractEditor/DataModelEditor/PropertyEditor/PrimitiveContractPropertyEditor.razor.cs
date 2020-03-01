using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CAlert;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Primitive;
using DasContract.Editor.Entities.Integrity.Contract.DataModel;
using DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities;
using DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities.Properties.Primitive;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.DataModelEditor.PropertyEditor
{
    public partial class PrimitiveContractPropertyEditor : ContractPropertyEditorBase
    {

        //--------------------------------------------------
        //                    DELETE
        //--------------------------------------------------
        protected PrimitiveContractProperty propertyToDelete = null;
        public override async Task DeletePropertyAsync(int index)
        {
            propertyToDelete = Entity.PrimitiveProperties[index];
            deletePropertyAnalysis = Contract.AnalyzeIntegrityOf(propertyToDelete);
            await deleteDialogWindow.OpenAsync();
        }

        public override async Task ConfirmDeleteAsync()
        {
            try
            {
                Contract.RemoveSafely(propertyToDelete);
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
        public override async Task AddPropertyAsync()
        {
            await createDialogWindow.OpenAsync();
        }

        protected override async Task ConfirmAddPropertyAsync()
        {
            //Entity.PrimitiveProperties.Add(newPropertyModel.ToPrimitiveContractProperty());
            Contract.AddSafely(Entity, newPropertyModel.ToPrimitiveContractProperty());

            await createDialogWindow.CloseAsync();
            alertController.AddAlert("New primitive property added successfuly", AlertScheme.Success);
            ResetNewPropertyModel();
        }
    }
}
