using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CButtonInput
{
    public enum ButtonInputScheme
    {
        Primary,
        Secondary,
        Success,
        Danger,
        Warning,
        Info,
        Light,
        Dark,
        Link
    }

    public static class ButtonInputSchemeHelper
    {
        /// <summary>
        /// Returns associated class for input button scheme
        /// </summary>
        /// <param name="scheme">Button scheme</param>
        /// <returns>Associated class for input button scheme, else exception</returns>
        public static string ToClass(ButtonInputScheme scheme)
        {
            if (scheme == ButtonInputScheme.Primary)
                return "btn-primary";
            else if (scheme == ButtonInputScheme.Secondary)
                return "btn-secondary";
            else if (scheme == ButtonInputScheme.Success)
                return "btn-success";
            else if (scheme == ButtonInputScheme.Danger)
                return "btn-danger";
            else if (scheme == ButtonInputScheme.Warning)
                return "btn-warning";
            else if (scheme == ButtonInputScheme.Info)
                return "btn-info";
            else if (scheme == ButtonInputScheme.Light)
                return "btn-light";
            else if (scheme == ButtonInputScheme.Dark)
                return "btn-dark";
            else if (scheme == ButtonInputScheme.Link)
                return "btn-link";

            throw new Exception("Unknown button scheme");
        }
    }
}
