using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CCard
{
    public enum CardScheme
    {
        Primary,
        Secondary,
        Success,
        Danger,
        Warning,
        Info,
        Light,
        Dark,
        None
    }

    public static class CardSchemeHelper
    {
        public static string ToClass(CardScheme theme)
        {
            if (theme == CardScheme.None)
                return "";
            if (theme == CardScheme.Primary)
                return "text-white bg-primary";
            if (theme == CardScheme.Secondary)
                return "text-white bg-secondary";
            if (theme == CardScheme.Success)
                return "text-white bg-success";
            if (theme == CardScheme.Danger)
                return "text-white bg-danger";
            if (theme == CardScheme.Warning)
                return "text-white bg-warning";
            if (theme == CardScheme.Info)
                return "text-white bg-info";
            if (theme == CardScheme.Light)
                return "bg-light";
            if (theme == CardScheme.Dark)
                return "text-white bg-dark";

            throw new ArgumentException("Unknown theme value");
        }
    }
}
