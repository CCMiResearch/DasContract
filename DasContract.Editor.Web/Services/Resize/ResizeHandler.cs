using BlazorPro.BlazorSize;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.Resize
{
    public class ResizeHandler
    {
        IJSRuntime _jsRuntime;
        ResizeListener _resizeListener;

        public event EventHandler OnMainGutterResize;
        public event EventHandler<BrowserWindowSize> OnBodyResize;

        public ResizeHandler(IJSRuntime jsRuntime, ResizeListener resizeListener)
        {
            _jsRuntime = jsRuntime;
            _resizeListener = resizeListener;
            _resizeListener.OnResized += BodyResized;
        }

        public async Task InitializeHandler()
        {
            await _jsRuntime.InvokeVoidAsync("splitterLib.setResizeHandlerInstance", DotNetObjectReference.Create(this));
        }

        [JSInvokable]
        public void MainGutterResized()
        {
            OnMainGutterResize?.Invoke(this, EventArgs.Empty);
        }

        public void BodyResized(object sender, BrowserWindowSize args)
        {
            OnBodyResize?.Invoke(this, args);
        }
    }
}
