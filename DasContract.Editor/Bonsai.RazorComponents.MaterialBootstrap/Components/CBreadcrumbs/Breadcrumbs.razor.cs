using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CBreadcrumbs
{
    public partial class Breadcrumbs: RootComponent
    {
        /// <summary>
        /// Breadcrumbs renderer for creating breadcrumbs
        /// </summary>
        public BreadcrumbsRenderer BreadcrumbsRender { get; set; } = new BreadcrumbsRenderer();

        /// <summary>
        /// Content that the cascading renderer will be passed to 
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// List of current breadrumbs
        /// </summary>
        List<Breadcrumb> Trail { get; set; } = new List<Breadcrumb>();

        /// <summary>
        /// Name of the cascading parameter
        /// </summary>
        [Parameter]
        public string CascadingParameterName { get; set; } = "GlobalBreadcrumbs";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            BreadcrumbsRender.OnNewBreadcrumbs += (caller, args) =>
            {
                Trail = args.Trail;
                StateHasChanged();
            };
        }
    }
}
