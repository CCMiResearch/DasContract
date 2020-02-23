using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CTable.Cell
{
    public partial class TableHeadCell: TableCell
    {
        /// <summary>
        /// Cell scope
        /// </summary>
        [Parameter]
        public TableCellScope Scope { get; set; } = TableCellScope.None;

        /// <summary>
        /// Cell scope keyword
        /// </summary>
        protected string TableCellScopeKeyword => TableCellScopeHelper.ToKeyword(Scope);
    }
}
