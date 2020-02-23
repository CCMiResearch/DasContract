using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CTable
{
    public enum TableStripe
    {
        None,
        Even
    }

    public static class TableStripesHelper
    {
        /// <summary>
        /// Returns associated class for table stripes
        /// </summary>
        /// <param name="stripes">Table stripes</param>
        /// <returns>Associated class for table stripes, else exception</returns>
        public static string ToClass(TableStripe stripes)
        {
            if (stripes == TableStripe.None)
                return "";
            else if (stripes == TableStripe.Even)
                return "table-striped";

            throw new Exception("Unknown table stripes");
        }
    }
}
