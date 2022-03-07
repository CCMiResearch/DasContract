using DasContract.Abstraction.UserInterface;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.UserForms
{
    public partial class DFieldGroup: ComponentBase
    {
        [Parameter]
        public FieldGroup FieldGroup { get; set; }
    }
}
