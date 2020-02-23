using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CValueInput.PrimitiveValueInputs
{
    public partial class BooleanValueInput: ValueInput<bool>
    {
        /// <summary>
        /// Boolean input type
        /// </summary>
        [Parameter]
        public BooleanInputType Type { get; set; } = BooleanInputType.Checkbox;

        /// <summary>
        /// React method for checkbox change event
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected async Task ToggleCheckbox()
        {
            await ChangeValueAsync(!Value);
            StateHasChanged();
        }
    }

    public enum BooleanInputType
    {
        Checkbox
    }
}
