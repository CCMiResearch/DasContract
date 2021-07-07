using DasContract.Abstraction.UserInterface.FormFields;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.UserForms.Fields
{
    public partial class DAddressField: ComponentBase
    {
        [Parameter]
        public AddressField Field { get; set; }
    }
}
