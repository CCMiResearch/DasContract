using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CAlert;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CDialogWindow;
using DasContract.Editor.Components.Main.Components.CContractEditor.DataModelEditor.PropertyEditor;
using DasContract.Editor.Entities;
using DasContract.Editor.Entities.DataModels.Entities;
using DasContract.Editor.Entities.Integrity.Analysis;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.DataModelEditor.PropertyEditor
{
    public abstract class ContractPropertyEditorBase : LoadableComponent
    {
        [Parameter]
        public EditorContract Contract { get; set; }

        [Parameter]
        public ContractEntity Entity { get; set; }

        protected AlertController alertController;
        //--------------------------------------------------
        //                    DELETE
        //--------------------------------------------------
        protected DialogWindow deleteDialogWindow;
        protected ContractIntegrityAnalysisResult deletePropertyAnalysis;
        public abstract Task DeletePropertyAsync(int index);

        public abstract Task ConfirmDeleteAsync();

        //--------------------------------------------------
        //                     ADD
        //--------------------------------------------------

        protected DialogWindow createDialogWindow;
        public abstract Task AddPropertyAsync();

        protected AddNewPropertyFormModel newPropertyModel = AddNewPropertyFormModel.Empty();
        protected abstract Task ConfirmAddPropertyAsync();

        protected void ResetNewPropertyModel()
        {
            newPropertyModel = AddNewPropertyFormModel.Empty();
        }
    }
}
