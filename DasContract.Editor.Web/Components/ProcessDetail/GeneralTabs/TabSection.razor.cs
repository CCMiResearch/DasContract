using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.ProcessDetail.GeneralTabs
{
    public partial class TabSection: ComponentBase
    {
        [Parameter]
        public string SectionName { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

    }
}
