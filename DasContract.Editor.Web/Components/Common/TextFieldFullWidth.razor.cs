using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.Common
{
    public partial class TextFieldFullWidth<TValue>: ComponentBase
    {
        [Parameter]
        public EventCallback<FocusEventArgs> OnFocusOut { get; set; }

        private TValue _value;

        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        [Parameter]
        public TValue Value {
            get => _value;
            set
            {
                if (Equals(_value, value))
                    return;

                _value = value;
                ValueChanged.InvokeAsync(value);
            }
        }

        protected async void OnFocusOutHandler(FocusEventArgs args)
        {
            await OnFocusOut.InvokeAsync(args);
        }

        [Parameter]
        public string Label { get; set; }


    }
}
