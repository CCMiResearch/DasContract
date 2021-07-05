using DasContract.Abstraction.UserInterface;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.ProcessDetail
{
    public partial class UserFormsTab: ComponentBase
    {
        [Parameter]
        public UserForm UserForm { get; set; }
    }
}
