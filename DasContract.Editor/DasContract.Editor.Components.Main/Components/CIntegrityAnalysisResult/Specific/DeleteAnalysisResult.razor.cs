using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using DasContract.Editor.Entities.Integrity.Analysis;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Components.Main.Components.CIntegrityAnalysisResult.Specific
{
    public partial class DeleteAnalysisResult : RootComponent
    {
        [Parameter]
        public ContractIntegrityAnalysisResult AnalysisResult { get; set; }

        [Parameter]
        public int Depth { get; set; } = 0;

        //protected string MarginLeftValue => (Depth * 1.5).ToString(CultureInfo.InvariantCulture) + "rem";

        protected string MarginLeftValue => Depth == 0 ? "0" : "1.5rem";
    }
}
