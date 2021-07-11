using DasContract.Abstraction.Processes.Events;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.ProcessDetail.GeneralTabs
{
    public partial class TimerBoundaryGeneralTab: ComponentBase
    {
        [Parameter]
        public TimerBoundaryEvent TimerBoundaryEvent { get; set; }

    }
}
