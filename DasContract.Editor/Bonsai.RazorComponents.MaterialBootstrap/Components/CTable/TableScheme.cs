using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CTable
{
    public enum TableScheme
    {
        Normal,
        Dark
    }

    public static class TableSchemeHelper
    {
        /// <summary>
        /// Returns associated class for table scheme
        /// </summary>
        /// <param name="scheme">Table scheme</param>
        /// <returns>Associated class for table scheme, else exception</returns>
        public static string ToClass(TableScheme scheme)
        {
            if (scheme == TableScheme.Normal)
                return "";
            else if (scheme == TableScheme.Dark)
                return "table-dark";

            throw new Exception("Unknown table scheme");
        }
    }
}
