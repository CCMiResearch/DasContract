using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CBreadcrumbs
{
    public class BreadcrumbsArgs
    {
        public List<Breadcrumb> Trail { get; set; }

        public BreadcrumbsArgs(List<Breadcrumb> trail)
        {
            Trail = trail;
        }
    }
}
