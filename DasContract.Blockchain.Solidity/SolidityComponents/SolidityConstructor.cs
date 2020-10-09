using System;
using System.Collections.Generic;
using System.Text;
using Liquid.NET;
using Liquid.NET.Constants;

namespace DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityConstructor : SolidityComponent
    {
        List<SolidityComponent> body;
        LiquidTemplate template = LiquidTemplate.Create("{{indent}}constructor () public payable" +
            "{\n" +
            "{{body}}" +
            "{{indent}}}\n").LiquidTemplate;
        public SolidityConstructor()
        {
            body = new List<SolidityComponent>();
        }

        public SolidityConstructor AddToBody(SolidityComponent component)
        {
            body.Add(component);
            return this;
        }

        public override LiquidString ToLiquidString(int indent)
        {
            return LiquidString.Create(ToString(indent));
        }

        public override string ToString(int indent = 0)
        {
            ITemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("indent", CreateIndent(indent)).
                DefineLocalVariable("body", bodyToLiquid(indent));
            return template.Render(ctx).Result;
        }

        LiquidCollection bodyToLiquid(int indent)
        {
            var col = new LiquidCollection();
            foreach (var b in body)
                col.Add(b.ToLiquidString(indent + 1));
            return col;
        }
    }
}
