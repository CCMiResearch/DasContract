using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.Processes;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.ProcessDetail.GeneralTabs
{
    public partial class ProcessGeneralTab: ComponentBase
    {
        [Parameter]
        public Process Process{ get; set; }
    }
}
