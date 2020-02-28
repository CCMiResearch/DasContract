using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CAlert;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CDialogWindow;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CModelForm
{
    public partial class ModelForm<TModel>: LoadableComponent
    {
        /// <summary>
        /// Current model to edit
        /// </summary>
        [Parameter]
        public TModel Model { get; set; }

        /// <summary>
        /// Tells if this form should be read only
        /// </summary>
        [Parameter]
        public bool ReadOnly { get; set; } = false;

        /// <summary>
        /// Render fragmen for buttons on the bottom of the form
        /// </summary>
        [Parameter]
        public RenderFragment Buttons { get; set; }

        /// <summary>
        /// Render fragment for all form inputs
        /// </summary>
        [Parameter]
        public RenderFragment Inputs { get; set; }

        /// <summary>
        /// Callback triggered on form valid submit
        /// </summary>
        [Parameter]
        public EventCallback OnValidSubmit
        {
            get => onValidSubmit.GetValueOrDefault();
            set => onValidSubmit = value;
        }
        EventCallback? onValidSubmit = null;

        protected async Task HandleValidSubmitAsync()
        {
            if (onValidSubmit != null)
                await onValidSubmit.Value.InvokeAsync(null);
        }

        /// <summary>
        /// Callback triggered when reset is confirmed
        /// </summary>
        [Parameter]
        public EventCallback OnReset 
        {
            get => onReset.GetValueOrDefault();
            set => onReset = value;
        }
        EventCallback? onReset = null;

        /// <summary>
        /// Opens dialog window for reset
        /// </summary>
        /// <returns>Task</returns>
        protected async Task ResetAsync()
        {
            await ResetDialogWindow.OpenAsync();
        }

        /// <summary>
        /// Called by dialog window to confirm reset.
        /// Invokes OnReset callback
        /// </summary>
        /// <returns>Task</returns>
        protected async Task ConfirmReset()
        {
            if (onReset != null)
                await OnReset.InvokeAsync(null);
            await ResetDialogWindow.CloseAsync();
        }

        /// <summary>
        /// Dialog window for reset
        /// </summary>
        protected DialogWindow ResetDialogWindow { get; set; }

        /// <summary>
        /// Alert controller
        /// </summary>
        public AlertController AlertController { get; set; }

        /// <summary>
        /// Used edit form
        /// </summary>
        public EditForm EditForm { get; set; }

    }


}
