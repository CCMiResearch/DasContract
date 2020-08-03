using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CValueInput;
using DasContract.Editor.Entities.DataModels;
using DasContract.Editor.Entities.Forms;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.FormEditor.BindingInput
{
    public partial class ContractPropertyBindingInput: ValueInput<ContractPropertyBinding>
    {
        [Parameter]
        public ContractDataModel DataModel { get; set; }

        protected override ContractPropertyBinding Parse(object value)
        {
            var valueInString = value.ToString();

            if (valueInString == "null")
                return null;

            foreach(var entity in DataModel.Entities)
            {
                foreach (var property in entity.Properties)
                    if (property.Id == valueInString)
                        return ContractPropertyBinding.With(property);
            }

            return null;
        }
    }
}
