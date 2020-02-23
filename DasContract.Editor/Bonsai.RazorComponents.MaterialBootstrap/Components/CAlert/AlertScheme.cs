using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CAlert
{
    public enum AlertScheme
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

    public static class AlertSchemeHelper
    {
        /// <summary>
        /// Returns associated class for input button scheme
        /// </summary>
        /// <param name="scheme">Button scheme</param>
        /// <returns>Associated class for input button scheme, else exception</returns>
        public static string ToClass(AlertScheme scheme)
        {
            if (scheme == AlertScheme.Primary)
                return "alert-primary";
            else if (scheme == AlertScheme.Secondary)
                return "alert-secondary";
            else if (scheme == AlertScheme.Success)
                return "alert-success";
            else if (scheme == AlertScheme.Danger)
                return "alert-danger";
            else if (scheme == AlertScheme.Warning)
                return "alert-warning";
            else if (scheme == AlertScheme.Info)
                return "alert-info";
            else if (scheme == AlertScheme.Light)
                return "alert-light";
            else if (scheme == AlertScheme.Dark)
                return "alert-dark";

            throw new Exception("Unknown alert scheme");
        }
    }
}
