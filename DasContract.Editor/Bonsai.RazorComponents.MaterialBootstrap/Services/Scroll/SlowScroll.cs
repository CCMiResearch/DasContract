using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Bonsai.Services.Interfaces;

namespace Bonsai.RazorComponents.MaterialBootstrap.Services.Scroll
{
    public class SlowScroll : IScroll
    {
        protected IJSRuntime JSRuntime { get; set; }

        public SlowScroll(IJSRuntime jsRuntime)
        {
            JSRuntime = jsRuntime;
        }

        public async Task AnchorScrollAsync(string className)
        {
            await JSRuntime.InvokeVoidAsync("MaterialBootstrapRazorComponents.SlowScroll.AnchorScroll", className);
        }

        public async Task ToAsync(uint top)
        {
            await JSRuntime.InvokeVoidAsync("MaterialBootstrapRazorComponents.SlowScroll.ToPx", top);
        }

        public async Task ToAsync(string selector)
        {
            await JSRuntime.InvokeVoidAsync("MaterialBootstrapRazorComponents.SlowScroll.ToFirst", selector);
        }

        public async Task ToTopAsync()
        {
            await JSRuntime.InvokeVoidAsync("MaterialBootstrapRazorComponents.SlowScroll.ToTop");
        }
    }
}
