using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.UserInput
{
    public class UserInputHandler
    {
        private IJSRuntime _jsRuntime;

        public event EventHandler<KeyEvent> KeyDown;

        public UserInputHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            InitializeHandler();
        }

        public async Task InitializeHandler()
        {
            await _jsRuntime.InvokeVoidAsync("keyCaptureLib.setEventHandlerInstance", DotNetObjectReference.Create(this));
        }

        [JSInvokable]
        public void HandleKeyInputEvent(KeyEvent e)
        {
            Console.WriteLine("key event");
            
            if(e.Type == "keydown")
            {
                Console.WriteLine("key down");
                KeyDown?.Invoke(this, e);
            }
        }
    }
}
