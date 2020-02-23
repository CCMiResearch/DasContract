using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CProgressBar
{
    public enum ProgressBarThickness
    {
        Thin,
        Normal,
        Thick
    }

    public static class ProgressBarThicknessHelper
    {
        public static string ToClass(ProgressBarThickness scheme)
        {
            if (scheme == ProgressBarThickness.Normal)
                return "";
            else if (scheme == ProgressBarThickness.Thick)
                return "progress-bar-thick";
            else if (scheme == ProgressBarThickness.Thin)
                return "progress-bar-thin";

            throw new Exception("Unknown progress bar scheme");
        }
    }
}
