using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CValueInput.PrimitiveValueInputs
{
    public partial class DateValueInput: ValueInput<DateTime>
    {
        [Parameter]
        public string DateFormat { get; set; } = "d. M. yyyy";

        protected bool LastDateParseError { get; set; } = false;

        protected List<string> parseErrorMessages = new List<string>() { "Date must be in format dd. mm. yyyy" };

        protected override DateTime Parse(object inputValue)
        {
            string dateString = inputValue.ToString().Trim();
            if (DateTime.TryParseExact(dateString, DateFormat, new CultureInfo("cs"), DateTimeStyles.None, out var date))
            {
                LastDateParseError = false;
                StateHasChanged();
                return date;
            }
            LastDateParseError = true;
            return Value;
        }
    }
}
