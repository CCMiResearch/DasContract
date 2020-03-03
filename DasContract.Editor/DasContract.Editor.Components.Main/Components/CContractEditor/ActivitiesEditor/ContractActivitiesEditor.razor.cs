using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Entities;
using DasContract.Editor.Entities.Processes.Process;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Components.Main.Components.CContractEditor.ActivitiesEditor
{
    public partial class ContractActivitiesEditor: LoadableComponent
    {
        [Parameter]
        public EditorContract Contract { get; set; }

        public ContractProcess Process => Contract.Processes.Main;


    }
}
