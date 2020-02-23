using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CValueInput.ValueInputHelpers
{
    public partial class ValueInputLabel: RootComponent
    {
        /// <summary>
        /// Target input Id
        /// </summary>
        [Parameter]
        public string For { get; set; } = "";

        /// <summary>
        /// Label content/text
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Is target input required?
        /// </summary>
        [Parameter]
        public bool Required { get; set; } = false;

        /// <summary>
        /// Additional classes
        /// </summary>
        [Parameter]
        public string Class { get; set; } = "";
    }
}
