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

        SolidityContract(string name)
        {
            this.name = name;

        }

        public void AddComponent(SolidityComponent component)
        {
            components.Add(component);
        }

        public void AddComponents(IList<SolidityComponent> components)
        {
            this.components.AddRange(components);
        }

        public override LiquidString ToLiquidString()
        {
            var ctx = new TemplateContext();
            ctx.DefineLocalVariable("components", functionsToLiquid());

            return LiquidString.Create(template.Render(ctx).Result);
        }

        LiquidCollection functionsToLiquid()
        {
            var col = new LiquidCollection();
            foreach (var f in components)
                col.Add(f.ToLiquidString());
            return col;
        }
    }
}
