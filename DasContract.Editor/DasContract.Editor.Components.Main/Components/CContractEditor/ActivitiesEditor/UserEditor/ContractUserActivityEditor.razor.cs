using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Entities;
using DasContract.Editor.Entities.Processes.Process.Activities;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.ActivitiesEditor.UserEditor
{
    public partial class ContractUserActivityEditor: LoadableComponent
    {
        [Parameter]
        public EditorContract Contract { get; set; }

        [Parameter]
        public ContractUserActivity UserActivity { get; set; }
    }
}
