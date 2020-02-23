using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.RazorComponents.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CCard.Texts
{
    public partial class CardTitle: RootComponent
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public HeadingLevel HeadingLevel { get; set; } = HeadingLevel.H5;
    }
}
