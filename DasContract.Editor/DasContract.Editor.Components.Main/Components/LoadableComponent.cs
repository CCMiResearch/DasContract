using Microsoft.AspNetCore.Components;
using Bonsai.RazorComponents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Components.Main.Components
{
    public class LoadableComponent : RootComponent, ILoadableComponent
    {
        /// <summary>
        /// Cascading parameter catching loading value
        /// </summary>
        [CascadingParameter(Name = "Loading")]
        private bool LoadingCascade { get; set; } = false;

        /// <summary>
        /// Property getting and setting loading status
        /// </summary>
        [Parameter]
        public bool Loading
        {
            get => LoadingCascade || loading;
            set
            {
                loading = value;
                StateHasChanged();
            }
        }
        private bool loading = false;
    }
}
