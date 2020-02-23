using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bonsai.Utils.String;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CSnackbar
{
    public partial class Snackbar : RootComponent
    {
        [Inject]
        IJSRuntime JSRuntime { get; set; }

        /// <summary>
        /// Snackbars Id
        /// </summary>
        [Parameter]
        public string Id { get; set; } = Guid.NewGuid().ToString().ToIdFriendly();

        /// <summary>
        /// Snackbar type
        /// </summary>
        [Parameter]
        public SnackbarType Type { get; set; }

        /// <summary>
        /// Snackbars content
        /// </summary>
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// If true, the snackbar will automatically hide after timeout
        /// </summary>
        [Parameter]
        public bool AutoHide { get; set; } = true;

        /// <summary>
        /// AutoHide timout [ms]
        /// </summary>
        [Parameter]
        public int HideTimeout { get; set; } = 5000;

        /// <summary>
        /// Tells if the snackbar is showed
        /// </summary>
        public bool Opened { get; private set; } = false;

        /// <summary>
        /// Shows the snackbar
        /// </summary>
        /// <returns></returns>
        public async Task ShowAsync()
        {
            if (Opened || timeoutInProgress || animationInProgress)
                return;

            Opened = true;

            animationInProgress = true;
            await JSRuntime.InvokeAsync<object>("MaterialBootstrapRazorComponents.Snackbar.Show", Id);
            animationInProgress = false;

            await OnOpen.InvokeAsync(false);

            if (AutoHide)
            {
                timeoutInProgress = true;
                await Task.Delay(HideTimeout);
                timeoutInProgress = false;
                await HideAsync();
            }
        }
        private bool timeoutInProgress = false;
        private bool animationInProgress = false;

        /// <summary>
        /// Hides the snackbar
        /// </summary>
        /// <returns></returns>
        public async Task HideAsync()
        {
            if (!Opened || timeoutInProgress || animationInProgress)
                return;

            Opened = false;

            animationInProgress = true;
            await JSRuntime.InvokeAsync<object>("MaterialBootstrapRazorComponents.Snackbar.Hide", Id);
            animationInProgress = false;

            await OnClose.InvokeAsync(false);
        }

        /// <summary>
        /// If the snackbar is opened, it closes.
        /// If the snackbar is close, it opens
        /// </summary>
        /// <returns></returns>
        public async Task ToggleAsync()
        {
            if (Opened)
                await HideAsync();
            else
                await ShowAsync();
        }

        /// <summary>
        /// Open event callback
        /// </summary>
        [Parameter]
        public EventCallback OnOpen { get; set; }

        /// <summary>
        /// Close event callback
        /// </summary>
        [Parameter]
        public EventCallback OnClose { get; set; }

        /// <summary>
        /// Returns class for the snackbar type
        /// </summary>
        protected string TypeClass
        {
            get
            {
                return SnackbarTypeHelper.ToClass(Type);
            }
        }

    }
}
