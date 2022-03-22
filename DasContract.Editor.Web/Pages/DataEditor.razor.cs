using DasContract.Abstraction.Data;
using DasContract.Editor.Web.Components.Common;
using DasContract.Editor.Web.Services;
using DasContract.Editor.Web.Services.Processes;
using DasContract.Editor.Web.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Scriban;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DasContract.Editor.Web.Pages
{
    public partial class DataEditor : ComponentBase, IDisposable
    {
        [Inject]
        protected ResizeHandler ResizeHandler { get; set; }

        [Inject]
        private SaveManager SaveManager { get; set; }

        [Inject]
        private IJSRuntime JSRunTime { get; set; }

        [Inject]
        private IContractManager ContractManager { get; set; }

        protected Refresh Refresh { get; set; }

        [CascadingParameter]
        protected MainLayout Layout { get; set; }

        protected string MermaidScript { get; set; }

        //  protected string MermaidScript { get; set; } = @"classDiagram
        //Animal <|-- Duck
        //Animal <|-- Fish
        //Animal <|-- Zebra
        //Animal: +int age
        //Animal: +String gender
        //Animal: +isMammal()
        //Animal: +mate()
        //class Duck{
        //    +String beakColor
        //    +swim()
        //    +quack()
        //}
        //  class Fish{
        //    -int sizeInFeet
        //    -canEat()
        //  }
        //  class Zebra{
        //    +bool is_wild
        //    +run()
        //  }";

        private Template _mermaidTemplate = Template.Parse(@"classDiagram
{{for enum in enums}}
class {{enum.name}} { 
<<enum>>
{{for value in enum.values}}
{{value}}
{{end}}
}
{{end}}

{{for entity in entities}}
class {{entity.name}} {
<<entity>>
{{for property in entity.properties}}
{{property.data_type}}{{if property.property_type == 'Collection'}}[]{{else if property.property_type == 'Dictionary'}}<{{property.key_type}}>{{end}} {{property.name}}
{{end}}
}
{{end}}

{{for token in tokens}}
class {{token.name}} {
<<token>>
{{for property in token.properties}}
{{property.data_type}}{{if property.property_type == 'Collection'}}[]{{else if property.property_type == 'Dictionary'}}<{{property.key_type}}>{{end}} {{property.name}}
{{end}}
}
{{end}}

{{for relationship in relationships}}
{{relationship.item1}} -- {{relationship.item2}}
{{end}}
");



        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await InitializeSplitGutter();
                await RefreshDiagram();
                CreateToolbarItems();
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

        }

        async Task InitializeSplitGutter()
        {
            await JSRunTime.InvokeVoidAsync("splitterLib.createSplit", ".gutter-col-1");
            await ResizeHandler.InitializeHandler();
        }

        protected async Task DataModelXmlChanged(string script)
        {
            ContractManager.SetDataModelXml(script);
            if (Refresh.AutoRefresh && !string.IsNullOrEmpty(script))
            {
                await RefreshDiagram();
            }
        }

        private void CreateToolbarItems()
        {
            var saveDiagramSvgItem = new ToolBarItem
            {
                Name = "Diagram as svg",
                IconName = "filetype-svg",
                Id = "download-svg"
            };
            saveDiagramSvgItem.OnClick += HandleSaveToSvgClicked;
            Layout.AddToolbarItem(saveDiagramSvgItem);
            var saveDiagramPngItem = new ToolBarItem
            {
                Name = "Diagram as png",
                IconName = "file-earmark-image",
                Id = "download-png"
            };
            saveDiagramPngItem.OnClick += HandleSaveToPngClicked;
            Layout.AddToolbarItem(saveDiagramPngItem);
        }

        private IList<Tuple<string, string>> GetModelRelationships(IDictionary<string, DataType> dataTypes)
        {
            var entities = dataTypes.Values.OfType<Entity>();
            var relationships = new List<Tuple<string, string>>();
            foreach (var entity in entities)
            {
                foreach (var property in entity.Properties)
                {
                    if (property.DataType == PropertyDataType.Reference && property.ReferencedDataType != null
                        && dataTypes.TryGetValue(property.ReferencedDataType, out var referenced))
                    {
                        relationships.Add(new Tuple<string, string>(entity.Name, referenced.Name));
                    }
                }
            }
            return relationships;
        }

        private async void HandleSaveToSvgClicked(object sender, MouseEventArgs args)
        {
            await JSRunTime.InvokeVoidAsync("mermaidLib.downloadSVG", ContractManager.GetContractName());
        }

        private async void HandleSaveToPngClicked(object sender, MouseEventArgs args)
        {
            await JSRunTime.InvokeVoidAsync("mermaidLib.downloadPNG", ContractManager.GetContractName());
        }

        private async Task RefreshDiagram()
        {
            try
            {
                var dataTypes = ContractManager.GetDataTypes();
                MermaidScript = _mermaidTemplate.Render(new
                {
                    Relationships = GetModelRelationships(dataTypes),
                    Enums = dataTypes.Values.OfType<Abstraction.Data.Enum>(),
                    Entities = dataTypes.Values.OfType<Entity>().Where(e => e.GetType() == typeof(Entity)),
                    Tokens = dataTypes.Values.OfType<Token>().Where(e => e.GetType() == typeof(Token))
                });

                await JSRunTime.InvokeVoidAsync("mermaidLib.renderMermaidDiagram", "mermaid-canvas", MermaidScript);
            }
            catch (JSException)
            {
                Console.WriteLine("Could not render mermaid diagram");
            }
        }

        public void Dispose()
        {
            Layout.RemoveToolbarItem("download-svg");
            Layout.RemoveToolbarItem("download-png");
        }
    }
}
