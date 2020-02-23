using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CSnackbar
{
    public enum SnackbarType
    {
        Snackbar,
        Toast
    }

    public static class SnackbarTypeHelper
    {
        /// <summary>
        /// Returns associated class for snackbar type
        /// </summary>
        /// <param name="scheme">Snackbar type</param>
        /// <returns>Associated class for snackbar type, else exception</returns>
        public static string ToClass(SnackbarType scheme)
        {
            if (scheme == SnackbarType.Snackbar)
                return "";
            else if (scheme == SnackbarType.Toast)
                return "toast";

            throw new Exception("Unknown snackbar type");
        }
    }
}
