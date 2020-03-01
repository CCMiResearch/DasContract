using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CValueInput.PrimitiveValueInputs
{
    public partial class TextValueInput: ValueInput<string>
    {
        [Parameter]
        public TextValueInputType Type { get; set; } = TextValueInputType.Text;
    }
}
