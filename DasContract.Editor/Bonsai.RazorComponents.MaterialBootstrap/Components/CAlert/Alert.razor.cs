using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bonsai.Utils.String;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CAlert
{
    public partial class Alert: RootComponent
    {
        [Inject]
        IJSRuntime JSRuntime { get; set; }

        /// <summary>
        /// Closes the alert
        /// </summary>
        public async Task CloseAsync()
        {
            if (closing)
                return;
            closing = true;
            StateHasChanged();

            //await JSRuntime.InvokeAsync<object>("MaterialBootstrapRazorComponents.Alert.Close", Id);
            await Task.Delay(300);

            await OnClose.InvokeAsync(false);
            Closed = true;
            closing = false;
            StateHasChanged();
        }
        private bool closing = false;

        /// <summary>
        /// Alerts Id
        /// </summary>
        [Parameter]
        public string Id { get; set; } = Guid.NewGuid().ToString().ToIdFriendly();

        /// <summary>
        /// Tells if the alert can be closed with cross
        /// </summary>
        [Parameter]
        public bool Closable { get; set; } = false;

        /// <summary>
        /// Tells if this alert is closed
        /// </summary>
        public bool Closed { get; protected set; }

        /// <summary>
        /// Alert close callback
        /// </summary>
        [Parameter]
        public EventCallback OnClose { get; set; }

        /// <summary>
        /// Alerts content
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Alerts text alternative to child content
        /// </summary>
        [Parameter]
        public string Content { get; set; } = "";

        /// <summary>
        /// Alert color scheme
        /// </summary>
        [Parameter]
        public AlertScheme Scheme { get; set; } = AlertScheme.Info;

        /// <summary>
        /// Returns class based on current alert scheme
        /// </summary>
        protected string AlertSchemeClass
        {
            get
            {
                return AlertSchemeHelper.ToClass(Scheme);
            }
        }
    }
}
