using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CValueInput.ValueInputHelpers
{
    public partial class ValueInputDescription: RootComponent
    {

        /// <summary>
        /// Description content
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
