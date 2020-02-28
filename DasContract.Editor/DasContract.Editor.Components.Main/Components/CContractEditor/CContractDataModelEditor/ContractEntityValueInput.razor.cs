using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CValueInput;
using DasContract.Editor.Entities.DataModels.Entities;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.CContractDataModelEditor
{
    public partial class ContractEntityValueInput: ValueInput<ContractEntity>
    {
        [Parameter]
        public List<ContractEntity> Entities { get; set; }

        protected override ContractEntity Parse(object value)
        {
            var id = value.ToString();
            return Entities.Find(e => e.Id == id);
        }
    }
}
