using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.RazorComponents.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CHeading
{
    public partial class Heading: RootComponent
    {
        [Parameter]
        public HeadingLevel Level { get; set; } = HeadingLevel.H1;

        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
