using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CValueInput
{
    public enum ValueInputState
    {
        Invalid,
        Valid,
        NotModified
    }

    public static class ValueInputStateHelper
    {
        /// <summary>
        /// Translates value input state into a css class
        /// </summary>
        /// <param name="state">State to translate</param>
        /// <returns>Class representing the input state</returns>
        public static string ToClass(ValueInputState state)
        {
            if (state == ValueInputState.Valid)
                return "valid";
            else if (state == ValueInputState.Invalid)
                return "invalid";
            else if (state == ValueInputState.NotModified)
                return "";

            throw new Exception("Unknown value input state");

        }
    }
}
