using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.ProcessDetail
{
    public partial class ScriptTab: ComponentBase
    {
        private string _script;

        [Parameter]
        public string Script { 
            get => _script;
            set
            {
                if (_script == value)
                    return;

                _script = value;
                ScriptChanged.InvokeAsync(value);
            }
        }

        [Parameter]
        public EventCallback<string> ScriptChanged { get; set; }
    }
}
