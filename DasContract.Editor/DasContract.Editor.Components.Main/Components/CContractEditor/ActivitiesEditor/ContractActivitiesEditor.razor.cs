using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DasContract.Editor.Components.Main.Components.CEditableItemsList;
using DasContract.Editor.Entities;
using DasContract.Editor.Entities.Processes.Process;
using DasContract.Editor.Entities.Processes.Process.Activities;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.ActivitiesEditor
{
    public partial class ContractActivitiesEditor: LoadableComponent
    {
        [Parameter]
        public EditorContract Contract { get; set; }

        public ContractProcess Process => Contract.Processes.Main;

        public EditableItemsList<ContractUserActivity> UserActivitiesList { get; set; }

        public EditableItemsList<ContractBusinessRuleActivity> BusinessRuleActivitiesList { get; set; }

        public EditableItemsList<ContractScriptActivity> ScriptActivitiesList { get; set; }
    }
}
