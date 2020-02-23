using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CTable.Row
{
    public partial class TableRow: RootComponent
    {
        /// <summary>
        /// Row scheme
        /// </summary>
        [Parameter]
        public TableRowScheme Scheme { get; set; } = TableRowScheme.Default;

        /// <summary>
        /// Row scheme class
        /// </summary>
        protected string SchemeClass => TableRowSchemeHelper.ToClass(Scheme);

        /// <summary>
        /// Content of the row
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
