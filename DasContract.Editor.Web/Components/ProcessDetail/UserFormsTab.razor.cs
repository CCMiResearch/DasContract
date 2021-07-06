using DasContract.Abstraction.UserInterface;
using DasContract.Editor.Web.Services.UserForm;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.ProcessDetail
{
    public partial class UserFormsTab: ComponentBase
    {
        [Inject]
        public UserFormService UserFormService { get; set; }

        [Parameter]
        public UserForm UserForm { get; set; }
    }
}
