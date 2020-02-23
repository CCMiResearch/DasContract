using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.RazorComponents.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CCard.Texts
{
    public partial class CardText: RootComponent
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
