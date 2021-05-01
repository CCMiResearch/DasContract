using DasContract.Editor.Web.Services.Processes;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.ProcessDetail.GeneralTabs
{
    public partial class ParticipantGeneralTab: ComponentBase
    {
        [Parameter]
        public ProcessParticipant Participant { get; set; }
    }
}
