using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CButtonInput
{
    public partial class ButtonInput : LoadableComponent
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Buttons text
        /// </summary>
        [Parameter]
        public string Text { get; set; } = "";

        /// <summary>
        /// If this parameter is defined, the button will serve as link
        /// </summary>
        [Parameter]
        public string Link { get; set; } = null;

        /// <summary>
        /// Buttons loading text
        /// </summary>
        [Parameter]
        public string LoadingText { get; set; } = "Načítá se";

        /// <summary>
        /// Block or unblock the button
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; } = false;

        /// <summary>
        /// Tells is the button is type="submit" or just regular button
        /// </summary>
        [Parameter]
        public bool Submit { get; set; } = false;

        /// <summary>
        /// Button color scheme
        /// </summary>
        [Parameter]
        public ButtonInputScheme Scheme { get; set; } = ButtonInputScheme.Primary;

        /// <summary>
        /// Button design type
        /// </summary>
        [Parameter]
        public ButtonInputType Type { get; set; } = ButtonInputType.Flat;

        /// <summary>
        /// Callback on (unblocked) button click
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        /// <summary>
        /// On button click internal method
        /// </summary>
        /// <param name="test"></param>
        async Task OnButtonClick(MouseEventArgs args)
        {
            if (!string.IsNullOrEmpty(Link))
                NavigationManager.NavigateTo(Link);

            await OnClick.InvokeAsync(args);
        }

        /// <summary>
        /// Buttons content
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Returns scheme class
        /// </summary>
        protected string SchemeClass
        {
            get
            {
                return ButtonInputSchemeHelper.ToClass(Scheme);
            }
        }

        /// <summary>
        /// Returns type class
        /// </summary>
        protected string TypeClass
        {
            get
            {
                return ButtonInputTypeHelper.ToClass(Type);
            }
        }
    }
}
