using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.RazorComponents.Interfaces;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CCard
{
    public static class CardContentAlignHelper
    {
        public static string ToClass(ContentAlign align)
        {
            if (align == ContentAlign.Center)
                return "text-center";
            else if (align == ContentAlign.Left)
                return "text-left";
            else if (align == ContentAlign.Right)
                return "text-right";

            throw new ArgumentException("Unknown content align value");
        }
    }
}
