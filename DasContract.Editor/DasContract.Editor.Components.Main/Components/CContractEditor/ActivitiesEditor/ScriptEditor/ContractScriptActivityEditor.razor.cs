using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Entities;
using DasContract.Editor.Entities.Processes.Process.Activities;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.ActivitiesEditor.ScriptEditor
{
    public partial class ContractScriptActivityEditor : LoadableComponent
    {
        [Parameter]
        public EditorContract Contract { get; set; }

        [Parameter]
        public ContractScriptActivity ScriptActivity { get; set; }
    }
}
