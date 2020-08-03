using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DasContract.Editor.Components.Main.Components.CContractEditor.DataModelEditor;
using DasContract.Editor.Entities.DataModels.Entities;
using DasContract.Editor.Entities.Forms;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.FormEditor
{
    class AddNewFieldFormModel
    {
        [Required(ErrorMessage = "Name is required")]
        [DisplayName("New field name")]
        [Description("Name of the new field")]
        public string Name { get; set; } = "";

        public ContractFormField ToFormField()
        {
            return new ContractFormField()
            {
                Name = Name
            };
        }

        public static AddNewFieldFormModel Empty()
        {
            return new AddNewFieldFormModel();
        }
    }
}
