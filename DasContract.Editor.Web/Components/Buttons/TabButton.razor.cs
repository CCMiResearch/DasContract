using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DasContract.Editor.Web.Components.Buttons
{
    public partial class TabButton: ComponentBase
    {
        [Parameter]
        public bool Active { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        protected async void OnClickHandler(MouseEventArgs args)
        {
            await OnClick.InvokeAsync(args);
        }
    }
}
