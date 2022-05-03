using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Web.Components.Buttons
{
    public partial class RefreshButton: ComponentBase
    {
        public bool AutoRefresh { get; protected set; } = true;

        [Parameter]
        public EventCallback RefreshRequested { get; set; }
    }
}
