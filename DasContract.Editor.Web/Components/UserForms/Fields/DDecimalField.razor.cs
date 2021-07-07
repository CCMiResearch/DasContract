using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Abstraction.UserInterface.FormFields;

namespace DasContract.Editor.Web.Components.UserForms.Fields
{
    public partial class DDecimalField: ComponentBase
    {
        [Parameter]
        public DecimalField Field { get; set; }
    }
}
