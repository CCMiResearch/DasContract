using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CTable
{
    public enum TableHeadScheme
    {
        Normal,
        Dark,
        Light
    }

    public static class TableHeadSchemeHelper
    {
        /// <summary>
        /// Returns associated class for table schemes head
        /// </summary>
        /// <param name="scheme">Table head scheme</param>
        /// <returns>Associated class for table head scheme, else exception</returns>
        public static string ToClass(TableHeadScheme scheme)
        {
            if (scheme == TableHeadScheme.Normal)
                return "";
            else if (scheme == TableHeadScheme.Dark)
                return "thead-dark";
            else if (scheme == TableHeadScheme.Light)
                return "thead-light";

            throw new Exception("Unknown table head scheme");
        }
    }
}
