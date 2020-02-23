using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CTable
{
    public enum TableSize
    {
        Normal,
        Small
    }

    public static class TableSizeHelper
    {
        /// <summary>
        /// Returns associated class for table size
        /// </summary>
        /// <param name="size">Table size</param>
        /// <returns>Associated class for table size, else exception</returns>
        public static string ToClass(TableSize size)
        {
            if (size == TableSize.Normal)
                return "";
            else if (size == TableSize.Small)
                return "table-sm";

            throw new Exception("Unknown table size");
        }
    }
}
