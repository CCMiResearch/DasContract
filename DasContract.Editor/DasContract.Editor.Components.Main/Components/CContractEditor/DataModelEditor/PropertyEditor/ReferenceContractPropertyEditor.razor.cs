using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CAlert;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Primitive;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Reference;
using DasContract.Editor.Entities.Integrity.Contract.DataModel;
using DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities;
using DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities.Properties.Reference;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.DataModelEditor.PropertyEditor
{
    public partial class ReferenceContractPropertyEditor : ContractPropertyEditorBase
    {

        //--------------------------------------------------
        //                    DELETE
        //--------------------------------------------------
        protected ReferenceContractProperty propertyToDelete = null;
        public override async Task DeletePropertyAsync(int index)
        {
            propertyToDelete = Entity.ReferenceProperties[index];
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
            var newProperty = newPropertyModel.ToReferenceContractProperty();
            newProperty.Entity = Entity;
            //Entity.ReferenceProperties.Add(newProperty);
            Contract.AddSafely(Entity, newProperty);

            await createDialogWindow.CloseAsync();
            await addSnackbarSuccess.ShowAsync();
            ResetNewPropertyModel();
        }
    }
}
