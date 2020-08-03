using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Components.Main.Components.CContractEditor.CContractDataModelEditor;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Components.Main.Components.CContractEditor
{
    public class ContractEditorSectionBase: LoadableComponent
    {
        [CascadingParameter(Name = "ContractEditorContext")]
        public ContractEditorContext Context { get; set; }
    }
}
