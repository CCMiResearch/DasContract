using DasContract.Abstraction.UserInterface;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.UserForms
{
    public partial class DForm: ComponentBase
    {
        [Parameter]
        public UserForm UserForm { get; set; }
    }
}
