using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CButtonInput
{
    public enum ButtonInputType
    {
        Flat,
        Raised
    }

    public static class ButtonInputTypeHelper
    {
        /// <summary>
        /// Returns associated class for input button scheme
        /// </summary>
        /// <param name="type">Button scheme</param>
        /// <returns>Associated class for input button scheme, else exception</returns>
        public static string ToClass(ButtonInputType type)
        {
            if (type == ButtonInputType.Flat)
                return "";
            else if (type == ButtonInputType.Raised)
                return "btn-raised";

            throw new Exception("Unknown button type");
        }
    }
}
