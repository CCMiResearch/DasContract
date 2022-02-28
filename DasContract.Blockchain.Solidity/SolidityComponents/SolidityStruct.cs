using Liquid.NET;
using Liquid.NET.Constants;
using System.Linq;
using System.Collections.Generic;
using DasContract.Abstraction.Data;

namespace DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityStruct : SolidityComponent
    {
        string structName;
        IList<SolidityComponent> body;

        static readonly LiquidTemplate template = LiquidTemplate.Create(
            "{{indent}}struct {{name}}" +
            "{\n" +
            "{{body}}" +
            "{{indent}}}\n").LiquidTemplate;

        public SolidityStruct(string name)
        {
            structName = name;
            body = new List<SolidityComponent>();
        }

        public SolidityStruct AddToBody(SolidityStatement statement)
        {
            body.Add(statement);
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
                DefineLocalVariable("name", LiquidString.Create(structName)).
                DefineLocalVariable("body", BodyToLiquid(indent));
            return template.Render(ctx).Result;
        }

        LiquidCollection BodyToLiquid(int indent)
        {
            var col = new LiquidCollection();
            foreach (var b in body)
                col.Add(b.ToLiquidString(indent + 1));
            return col;
        }
    }
}