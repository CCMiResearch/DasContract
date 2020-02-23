using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CValueInput.ValueInputHelpers
{
    public partial class ValueInputValidationMessage<TProperty>: RootComponent
    {
        /// <summary>
        /// Target property expression
        /// </summary>
        [Parameter]
        public Expression<Func<TProperty>> For { get; set; }

        /// <summary>
        /// More custom messages
        /// </summary>
        [Parameter]
        public IEnumerable<string> Messages { get; set; } = new List<string>();
    }
}
