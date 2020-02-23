using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CProgressBar
{
    public enum ProgressBarScheme
    {
        Primary,
        Secondary,
        Success,
        Danger,
        Warning,
        Info,
        Light,
        Dark
    }

    public static class ProgressBarSchemeHelper
    {
        /// <summary>
        /// Returns associated class for progress bar scheme
        /// </summary>
        /// <param name="scheme">Progress bar scheme</param>
        /// <returns>Associated class for progress bar scheme, else exception</returns>
        public static string ToClass(ProgressBarScheme scheme)
        {
            if (scheme == ProgressBarScheme.Primary)
                return "bg-primary";
            else if (scheme == ProgressBarScheme.Secondary)
                return "bg-secondary";
            else if (scheme == ProgressBarScheme.Success)
                return "bg-success";
            else if (scheme == ProgressBarScheme.Danger)
                return "bg-danger";
            else if (scheme == ProgressBarScheme.Warning)
                return "bg-warning";
            else if (scheme == ProgressBarScheme.Info)
                return "bg-info";
            else if (scheme == ProgressBarScheme.Light)
                return "bg-light";
            else if (scheme == ProgressBarScheme.Dark)
                return "bg-dark";

            throw new Exception("Unknown progress bar scheme");
        }
    }
}
