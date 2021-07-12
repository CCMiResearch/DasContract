using BlazorMonaco;
using DasContract.Editor.Web.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.ProcessDetail
{
    public partial class ScriptTab: ComponentBase, IDisposable
    {
        [Inject]
        private ResizeHandler ResizeHandler { get; set; }

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

        [Parameter]
        public string Language { get; set; }

        protected MonacoEditor MonacoEditor { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ResizeHandler.OnMainGutterResize += ResizeLayout;
            ResizeHandler.OnBodyResize += ResizeLayout;
        }

        public void Dispose()
        {
            Console.WriteLine("Disposing");
            ResizeHandler.OnMainGutterResize -= ResizeLayout;
            ResizeHandler.OnBodyResize -= ResizeLayout;
        }

        protected StandaloneEditorConstructionOptions EditorConstructionOptions(MonacoEditor editor)
        {
            return new StandaloneEditorConstructionOptions
            {
                Language = Language,
                Value = _script,
                ScrollBeyondLastLine = false,
                Minimap = new EditorMinimapOptions { Enabled = false}
            };
        }

        protected async void OnEditorInput()
        {
            Script = await MonacoEditor.GetValue();
        }

        protected void ResizeLayout(object sender, EventArgs args)
        {
            Console.WriteLine("Resizing layout");
            MonacoEditor.Layout();
        }

    }
}
