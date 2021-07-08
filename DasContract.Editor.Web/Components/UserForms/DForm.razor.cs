using DasContract.Abstraction.UserInterface;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if(firstRender)
            {
                await JSRuntime.InvokeVoidAsync("tooltipsLib.initializeAll");
            }
        }
    }
}
