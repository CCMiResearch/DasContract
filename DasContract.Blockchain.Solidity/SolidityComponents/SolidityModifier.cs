using Liquid.NET;
using Liquid.NET.Constants;
using System.Collections.Generic;

namespace DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityModifier : SolidityComponent
    {
        LiquidString modifierName;
        IList<SolidityComponent> body;

        static readonly LiquidTemplate template = LiquidTemplate.Create("{{indent}}modifier {{name}}" +
            "{\n" +
            "{{body}}" +
            "{{indent}}\t_;\n" +
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

        public override LiquidString ToLiquidString(int indent)
        {
            return LiquidString.Create(ToString(indent));
        }

        public override string ToString(int indent = 0)
        {
            ITemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("indent", CreateIndent(indent)).
                DefineLocalVariable("name", modifierName).
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
