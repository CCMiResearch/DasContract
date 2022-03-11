using DasContract.Abstraction.UserInterface.FormFields;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.UserForms.Fields
{
    public partial class DField: ComponentBase
    {
        [Parameter]
        public Field Field { get; set; }
    }
}
