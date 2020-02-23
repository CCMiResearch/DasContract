using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bonsai.Utils.String;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CDialogWindow
{
    public partial class DialogWindow : RootComponent
    {
        [Inject]
        IJSRuntime JSRuntime { get; set; }

        /// <summary>
        /// Id of this dialog window
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString().ToIdFriendly();

        /// <summary>
        /// Property for getting or setting the open status
        /// </summary>
        public bool IsOpen { get; private set; } = false;

        /// <summary>
        /// Opens the dialog window
        /// </summary>
        public async Task OpenAsync()
        {
            await JSRuntime.InvokeAsync<object>("MaterialBootstrapRazorComponents.DialogWindow.Show", Id);

            if (IsOpen)
                return;
            IsOpen = true;
            await OnOpen.InvokeAsync(false);
        }

        /// <summary>
        /// Closes the dialog window
        /// </summary>
        public async Task CloseAsync()
        {
            await JSRuntime.InvokeAsync<object>("MaterialBootstrapRazorComponents.DialogWindow.Hide", Id);

            if (!IsOpen)
                return;
            IsOpen = false;
            await OnClose.InvokeAsync(false);
        }

        /// <summary>
        /// Heading right after the Title for more title options
        /// </summary>
        [Parameter]
        public RenderFragment Header { get; set; }

        /// <summary>
        /// Content of the dialog window
        /// </summary>
        [Parameter]
        public RenderFragment Body { get; set; }

        /// <summary>
        /// Dialogs footer, mostly for buttons
        /// </summary>
        [Parameter]
        public RenderFragment Footer { get; set; }

        /// <summary>
        /// Title of the dialog window
        /// </summary>
        [Parameter]
        public string Title { get; set; } = "";

        /// <summary>
        /// Automatically show and sets up close button
        /// </summary>
        [Parameter]
        public bool ShowCloseButton { get; set; } = true;

        /// <summary>
        /// Calls callback on dialog close
        /// </summary>
        [Parameter]
        public EventCallback OnClose { get; set; }

        /// <summary>
        /// Calls callback on dialog open
        /// </summary>
        [Parameter]
        public EventCallback OnOpen { get; set; }

        /// <summary>
        /// Dialog size
        /// </summary>
        [Parameter]
        public DialogWindowSize Size { get; set; } = DialogWindowSize.Default;
    }
}
