using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Primitive;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Reference;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.CContractDataModelEditor.CContractPropertyEditor
{
    public class AddNewPropertyFormModel
    {
        [Required(ErrorMessage = "Name is required")]
        [DisplayName("Property name")]
        [Description("Name of the new property")]
        public string Name { get; set; } = "";

        public PrimitiveContractProperty ToPrimitiveContractProperty()
        {
            return new PrimitiveContractProperty()
            {
                Name = Name
            };
        }

        public ReferenceContractProperty ToReferenceContractProperty()
        {
            return new ReferenceContractProperty()
            {
                Name = Name
            };
        }

        public static AddNewPropertyFormModel Empty()
        {
            return new AddNewPropertyFormModel();
        }
    }
}
