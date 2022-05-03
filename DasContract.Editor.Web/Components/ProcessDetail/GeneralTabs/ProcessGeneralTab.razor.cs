using DasContract.Abstraction.Processes;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Web.Components.ProcessDetail.GeneralTabs
{
    public partial class ProcessGeneralTab: ComponentBase
    {
        [Parameter]
        public Process Process {get; set; }
    }
}
