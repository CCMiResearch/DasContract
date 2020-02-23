using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CTable.Cell
{
    public enum TableCellScheme
    {
        Primary,
        Secondary,
        Success,
        Danger,
        Warning,
        Info,
        Light,
        Dark,
        Active,
        Default
    }

    public static class TableCellSchemeHelper
    {
        /// <summary>
        /// Returns associated class for table cell scheme
        /// </summary>
        /// <param name="scheme">Table cell scheme</param>
        /// <returns>Associated class for table cell scheme, else exception</returns>
        public static string ToClass(TableCellScheme scheme)
        {
            if (scheme == TableCellScheme.Default)
                return "";
            else if (scheme == TableCellScheme.Primary)
                return "table-primary";
            else if (scheme == TableCellScheme.Secondary)
                return "table-secondary";
            else if (scheme == TableCellScheme.Success)
                return "table-success";
            else if (scheme == TableCellScheme.Danger)
                return "table-danger";
            else if (scheme == TableCellScheme.Warning)
                return "table-warning";
            else if (scheme == TableCellScheme.Info)
                return "table-info";
            else if (scheme == TableCellScheme.Light)
                return "table-light";
            else if (scheme == TableCellScheme.Dark)
                return "table-dark";
            else if (scheme == TableCellScheme.Active)
                return "table-active";

            throw new Exception("Unknown table cell scheme");
        }
    }
}
