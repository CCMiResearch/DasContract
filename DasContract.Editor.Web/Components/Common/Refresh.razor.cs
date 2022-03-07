using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.Common
{
    public partial class Refresh: ComponentBase
    {
        public bool AutoRefresh { get; protected set; } = true;

        [Parameter]
        public EventCallback RefreshRequested { get; set; }
    }
}
