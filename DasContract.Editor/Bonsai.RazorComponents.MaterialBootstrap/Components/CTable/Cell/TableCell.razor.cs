using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CProgressBar;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CTable.Cell
{
    public partial class TableCell: RootComponent
    {
        /// <summary>
        /// Cell scheme
        /// </summary>
        [Parameter]
        public TableCellScheme Scheme { get; set; } = TableCellScheme.Default;

        /// <summary>
        /// Cell scheme class
        /// </summary>
        protected string SchemeClass => TableCellSchemeHelper.ToClass(Scheme);

        /// <summary>
        /// Content of the cell
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
