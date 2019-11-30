using BpmnToSolidity.SolidityConverter;
using Liquid.NET;
using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Solidity
{
    class SolidityContract : SolidityComponent
    {
        string name;

        List<SolidityComponent> components;

        LiquidTemplate template = LiquidTemplate.Create(
            "contract {{name}} { \n" +
            "{{components}} " +
            "}").LiquidTemplate;

        public SolidityContract(string name)
        {
            this.name = name;
            components = new List<SolidityComponent>();
        }

        public void AddComponent(SolidityComponent component)
        {
            components.Add(component);
        }

        public void AddComponents(IList<SolidityComponent> components)
        {
            this.components.AddRange(components);
        }

        public override LiquidString ToLiquidString(int indent)
        {
            return LiquidString.Create(ToString(indent));
        }

        public override string ToString(int indent = 0)
        {
            var ctx = new TemplateContext();
            ctx.DefineLocalVariable("components", FunctionsToLiquid(indent)).
                DefineLocalVariable("name",LiquidString.Create(name));

            return template.Render(ctx).Result;
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
