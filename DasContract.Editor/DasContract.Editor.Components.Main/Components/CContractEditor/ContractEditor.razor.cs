using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Editor.Components.Main.Components;
using DasContract.Editor.Entities;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Components.Main.Components.CContractEditor
{
    public partial class ContractEditor : LoadableComponent
    {
        [Parameter]
        public EditorContract Contract { get; set; }


    }
}
