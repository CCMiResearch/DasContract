using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CLoadingScreen
{
    public partial class LoadingScreenAnimation: LoadableComponent
    {
        [Parameter]
        public bool Fullscreen { get; set; } = false;
    }
}
