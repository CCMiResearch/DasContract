using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CValueInput.ValueInputHelpers
{
    public partial class EditContextCascator: RootComponent
    {
        /// <summary>
        /// Caught EditContext cascading parameter
        /// </summary>
        [CascadingParameter]
        public EditContext EditContext { get; set; }

        /// <summary>
        /// Context where new recascaded parameter is set
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
