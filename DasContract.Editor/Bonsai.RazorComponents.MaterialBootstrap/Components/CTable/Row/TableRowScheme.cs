using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CTable.Row
{
    public enum TableRowScheme
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

    public static class TableRowSchemeHelper
    {
        /// <summary>
        /// Returns associated class for table row scheme
        /// </summary>
        /// <param name="scheme">Table row scheme</param>
        /// <returns>Associated class for table row scheme, else exception</returns>
        public static string ToClass(TableRowScheme scheme)
        {
            if (scheme == TableRowScheme.Default)
                return "";
            else if (scheme == TableRowScheme.Primary)
                return "table-primary";
            else if (scheme == TableRowScheme.Secondary)
                return "table-secondary";
            else if (scheme == TableRowScheme.Success)
                return "table-success";
            else if (scheme == TableRowScheme.Danger)
                return "table-danger";
            else if (scheme == TableRowScheme.Warning)
                return "table-warning";
            else if (scheme == TableRowScheme.Info)
                return "table-info";
            else if (scheme == TableRowScheme.Light)
                return "table-light";
            else if (scheme == TableRowScheme.Dark)
                return "table-dark";
            else if (scheme == TableRowScheme.Active)
                return "table-active";

            throw new Exception("Unknown table row scheme");
        }
    }
}
