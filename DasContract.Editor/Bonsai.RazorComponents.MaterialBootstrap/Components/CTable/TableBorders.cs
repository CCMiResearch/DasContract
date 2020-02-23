using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CTable
{
    public enum TableBorder
    {
        None,
        Rows,
        Cells
    }

    public static class TableBordersHelper
    {
        /// <summary>
        /// Returns associated class for table borders
        /// </summary>
        /// <param name="borders">Table borders</param>
        /// <returns>Associated class for table borders, else exception</returns>
        public static string ToClass(TableBorder borders)
        {
            if (borders == TableBorder.None)
                return "table-borderless";
            else if (borders == TableBorder.Rows)
                return "";
            else if (borders == TableBorder.Cells)
                return "table-bordered";

            throw new Exception("Unknown table borders");
        }
    }
}
