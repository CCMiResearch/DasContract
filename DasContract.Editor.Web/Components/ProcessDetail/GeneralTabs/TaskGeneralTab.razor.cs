using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.ProcessDetail.GeneralTabs
{
    public partial class TaskGeneralTab: ComponentBase
    {
        [Parameter]
        public Abstraction.Processes.Tasks.Task Task { get; set; }
    }
}
