using DasContract.Editor.Web.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.Common
{
    public partial class ToolBarButton : ComponentBase
    {
        [Parameter]
        public ToolBarItem ToolBarItem { get; set; }
    }
}
