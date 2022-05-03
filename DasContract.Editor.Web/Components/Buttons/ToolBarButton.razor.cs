using DasContract.Editor.Web.Shared;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Web.Components.Buttons
{
    public partial class ToolBarButton : ComponentBase
    {
        [Parameter]
        public ToolBarItem ToolBarItem { get; set; }
    }
}
