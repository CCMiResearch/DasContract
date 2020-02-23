using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CDialogWindow
{
    public enum DialogWindowSize
    {
        Small,
        Default,
        Large,
        ExtraLarge
    }

    public static class DialogWindowSizeHelper
    {
        /// <summary>
        /// Returns associated class for input button scheme
        /// </summary>
        /// <param name="scheme">Button scheme</param>
        /// <returns>Associated class for input button scheme, else exception</returns>
        public static string ToClass(DialogWindowSize size)
        {
            if (size == DialogWindowSize.Default)
                return "";
            else if (size == DialogWindowSize.Small)
                return "modal-sm";
            else if (size == DialogWindowSize.Large)
                return "modal-lg";
            else if (size == DialogWindowSize.ExtraLarge)
                return "modal-xl";

            throw new Exception("Unknown dialog size");
        }
    }
}
