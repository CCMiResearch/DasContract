using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CValueInput.PrimitiveValueInputs
{
    public partial class EnumValueInput<TEnum>: ValueInput<TEnum>
    {
        protected override TEnum Parse(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var result = (TEnum)Enum.Parse(Value.GetType(), value.ToString());
            return result;
            //return (TEnum)int.Parse((string)inputValue);
        }
    }
}
