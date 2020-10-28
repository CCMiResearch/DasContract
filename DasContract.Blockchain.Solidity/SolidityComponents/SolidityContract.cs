using Liquid.NET;
using Liquid.NET.Constants;
using System.Collections.Generic;
using System.Linq;

namespace DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityContract : SolidityComponent
    {
        string name;

        List<string> inheritance;
        List<SolidityComponent> components;

        static readonly LiquidTemplate template = LiquidTemplate.Create(
            "contract {{name}} " +
            "{% unless inherits == '' %}is {{inherits}}{%endunless%}{\n" +
            "{{components}}" +
            "}").LiquidTemplate;

        public SolidityContract(string name)
        {
            this.name = name;
            components = new List<SolidityComponent>();
            inheritance = new List<string>();
        }

        public void AddInheritance(string inheritance)
        {
            this.inheritance.Add(inheritance);
        }

        public void AddComponent(SolidityComponent component)
        {
            if (component is SolidityStatement)
                components.Insert(0, component);
            else
                components.Add(component);
        }

        public void AddComponents(IList<SolidityComponent> components)
        {
            foreach (var component in components)
                AddComponent(component);
        }

        public override LiquidString ToLiquidString(int indent)
        {
            return LiquidString.Create(ToString(indent));
        }

        public override string ToString(int indent = 0)
        {
            var ctx = new TemplateContext();
            ctx.DefineLocalVariable("components", FunctionsToLiquid(indent)).
                DefineLocalVariable("name",LiquidString.Create(name)).
                DefineLocalVariable("inherits", InheritanceToLiquid());

            return template.Render(ctx).Result;
        }

        LiquidString InheritanceToLiquid()
        {
            if (inheritance.Count == 0)
                return LiquidString.Create("");
            return LiquidString.Create(string.Join(", ", inheritance));
        }

        LiquidCollection FunctionsToLiquid(int indent)
        {
            var col = new LiquidCollection();
            foreach (var f in components)
                col.Add(LiquidString.Create(f.ToString(indent + 1) + "\n"));
            return col;
        }
    }
}
