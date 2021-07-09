using DasContract.Editor.Web.Components.Common;
using DasContract.Editor.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Pages
{
    public partial class DataEditor: ComponentBase
    {
        [Inject]
        protected ResizeHandler ResizeHandler { get; set; }

        [Inject]
        private SaveManager SaveManager { get; set; }

        [Inject]
        private IJSRuntime JSRunTime { get; set; }

        protected Refresh Refresh { get; set; }

        protected string MermaidScript { get; set; } = @"classDiagram
      Animal <|-- Duck
      Animal <|-- Fish
      Animal <|-- Zebra
      Animal: +int age
      Animal: +String gender
      Animal: +isMammal()
      Animal: +mate()
      class Duck{
          +String beakColor
          +swim()
          +quack()
      }
        class Fish{
          -int sizeInFeet
          -canEat()
        }
        class Zebra{
          +bool is_wild
          +run()
        }";


        protected override void OnAfterRender(bool firstRender)
        {
            if(firstRender)
            {
                InitializeSplitGutter();
                MermaidScriptChanged(MermaidScript);
            }
        }

        async void InitializeSplitGutter()
        {
            await JSRunTime.InvokeVoidAsync("splitterLib.createSplit", ".gutter-col-1");
            await ResizeHandler.InitializeHandler();
        }

        protected void MermaidScriptChanged(string script)
        {
            MermaidScript = script;
            if (Refresh.AutoRefresh)
                RefreshDiagram();
        }

        private async void RefreshDiagram()
        {
            try
            {
                await JSRunTime.InvokeVoidAsync("mermaidLib.renderMermaidDiagram", "mermaid-canvas", MermaidScript);
            }
            catch (JSException)
            {
                Console.WriteLine("Could not render mermaid diagram");
            }
        }
    }
}
