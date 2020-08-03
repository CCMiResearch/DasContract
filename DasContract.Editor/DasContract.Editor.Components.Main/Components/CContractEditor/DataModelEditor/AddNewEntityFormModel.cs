using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DasContract.Editor.Entities.DataModels.Entities;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.DataModelEditor
{
    class AddNewEntityFormModel
    {
        [Required(ErrorMessage = "Name is required")]
        [DisplayName("Entity name")]
        [Description("Name of the new entity")]
        public string Name { get; set; } = "";

        public ContractEntity ToContractEntity()
        {
            return new ContractEntity()
            {
                Name = Name
            };
        }

        public static AddNewEntityFormModel Empty()
        {
            return new AddNewEntityFormModel();
        }
    }
}
