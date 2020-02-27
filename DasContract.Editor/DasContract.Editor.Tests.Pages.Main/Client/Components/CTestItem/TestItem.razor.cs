using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Tests.Components.Main.Client.Components.CTestItem
{
    public partial class TestItem
    {
        [Parameter]
        public string UITestLink { get; set; } = "";

        [Parameter]
        public string Heading { get; set; }

        [Parameter]
        public string Text { get; set; } = "";
    }
}
