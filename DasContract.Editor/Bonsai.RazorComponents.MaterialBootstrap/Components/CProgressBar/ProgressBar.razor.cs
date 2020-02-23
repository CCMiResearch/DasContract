using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CProgressBar
{
    public partial class ProgressBar : RootComponent
    {
        /// <summary>
        /// The minimum value, usually 0
        /// </summary>
        [Parameter]
        public double Minimum { get; set; } = 0;

        /// <summary>
        /// Maximum value of the progres bar
        /// </summary>
        [Parameter]
        public double Maximum { get; set; } = 100;

        /// <summary>
        /// Current value
        /// </summary>
        [Parameter]
        public double Current { get; set; } = 0;

        /// <summary>
        /// Progress represented in percentage
        /// </summary>
        [Parameter]
        public double Percentage 
        {
            get
            {
                return (Current / Maximum) * 100;
            }
            set
            {
                Current = (Maximum / 100) * value;
            }
        }

        protected string BarWidth
        {
            get 
            {
                var format = new NumberFormatInfo
                {
                    NumberDecimalSeparator = "."
                };
                return Percentage.ToString(format) + "%";
            }
        }

        /// <summary>
        /// Scheme of this progress bar
        /// </summary>
        [Parameter]
        public ProgressBarScheme Scheme { get; set; } = ProgressBarScheme.Primary;

        /// <summary>
        /// Returns class for current scheme
        /// </summary>
        protected string SchemeClass => ProgressBarSchemeHelper.ToClass(Scheme);

        /// <summary>
        /// Thickness of this progress bar
        /// </summary>
        [Parameter]
        public ProgressBarThickness Thickness { get; set; } = ProgressBarThickness.Normal;

        /// <summary>
        /// Returns class for current thickness
        /// </summary>
        protected string ThicknessClass => ProgressBarThicknessHelper.ToClass(Thickness);
    }
}
