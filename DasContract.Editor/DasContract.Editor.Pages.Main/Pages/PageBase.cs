using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CBreadcrumbs;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Pages.Main.Pages
{
    public class PageBase : ComponentBase
    {
        [CascadingParameter(Name = "GlobalBreadcrumbs")]
        protected BreadcrumbsRenderer Breadcrumbs { get; set; }

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
