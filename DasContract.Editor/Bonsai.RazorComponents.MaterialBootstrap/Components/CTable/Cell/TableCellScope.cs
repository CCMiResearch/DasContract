using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CTable.Cell
{
    public enum TableCellScope
    {
        None,
        Column,
        Row,
        ColumnGroup,
        RowGroup
    }

    public static class TableCellScopeHelper
    {
        /// <summary>
        /// Returns associated keyword for table cell scope
        /// </summary>
        /// <param name="scope">Table cell scope</param>
        /// <returns>Associated keyword for table cell scope, else exception</returns>
        public static string ToKeyword(TableCellScope scope)
        {
            if (scope == TableCellScope.None)
                return "";
            else if (scope == TableCellScope.Column)
                return "col";
            else if (scope == TableCellScope.Row)
                return "row";
            else if (scope == TableCellScope.ColumnGroup)
                return "colgroup";
            else if (scope == TableCellScope.RowGroup)
                return "rowgroup";

            throw new Exception("Unknown table cell scope");
        }
    }
}
