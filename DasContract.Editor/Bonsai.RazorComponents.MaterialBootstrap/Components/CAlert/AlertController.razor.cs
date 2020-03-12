using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Utils.String;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CAlert
{
    public partial class AlertController: RootComponent
    {
        /// <summary>
        /// List of render fragments identified by an id
        /// </summary>
        protected List<KeyValuePair<string, RenderFragment>> Alerts { get; set; } = new List<KeyValuePair<string, RenderFragment>>();

        public void AddAlert(string text, AlertScheme scheme)
        {
            var id = Guid.NewGuid().ToString().ToIdFriendly();
            RenderFragment fragment = builder =>
            {
                builder.OpenComponent(0, typeof(Alert));
                builder.AddAttribute(1, "Closable", true);
                builder.AddAttribute(2, "Id", id);
                builder.AddAttribute(3, "Scheme", scheme);
                builder.AddAttribute(4, "Content", text);
                builder.CloseComponent();
            };
            Alerts.Add(new KeyValuePair<string, RenderFragment>(id, fragment));
            StateHasChanged();
        }
    }
}
