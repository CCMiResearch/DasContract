using BpmnToSolidity.SolidityConverter;
using Liquid.NET;
using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace BpmnToSolidity.Solidity
{
    public class SolidityModifier : SolidityComponent
    {
        LiquidString modifierName;
        IList<SolidityComponent> body;

        LiquidTemplate template = LiquidTemplate.Create("{{indent}}modifier {{name}}(" +
            "{\n" +
            "{{body}}" +
            "_;\n" +
            "{{indent}}}\n").LiquidTemplate;

        public SolidityModifier(string modifierName)
        {
            this.modifierName = LiquidString.Create(modifierName);

            body = new List<SolidityComponent>();
        }

        public void AddToBody(SolidityComponent component)
        {
            body.Add(component);
        }

        public override LiquidString ToLiquidString()
        {
            ITemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("indent", CreateIndent(indent)).
                DefineLocalVariable("name", modifierName).
                DefineLocalVariable("body", bodyToLiquid());
            return LiquidString.Create(template.Render(ctx).Result);
        }

        LiquidCollection bodyToLiquid()
        {
            var col = new LiquidCollection();
            foreach (var b in body)
                col.Add(b.ToLiquidString());
            return col;
        }
    }
}
