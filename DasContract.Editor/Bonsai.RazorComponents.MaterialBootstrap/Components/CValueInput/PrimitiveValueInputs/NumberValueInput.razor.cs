using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CValueInput.PrimitiveValueInputs
{
    public partial class NumberValueInput: ValueInput<int>
    {
        protected override int Parse(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));


            string valueString = value.ToString();

            //Test for empty string
            if (string.IsNullOrWhiteSpace(valueString))
                return 0;

            //Try parse
            var success = int.TryParse(valueString, out var res);

            //Return
            if (success)
                return res;
            return 0;

        }
    }
}
