using Liquid.NET;
using Liquid.NET.Constants;
using System.Linq;
using System.Collections.Generic;
using DasContract.Abstraction.Data;

namespace DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityStruct : SolidityComponent
    {
        LiquidString structName;
        IList<SolidityComponent> body;

        LiquidTemplate template = LiquidTemplate.Create(
            "{{indent}}struct {{name}}" +
            "{\n" +
            "{{body}}" +
            "{{indent}}}\n"+
            "{{indent}}{{name}} {{varName}} = {{name}}({{parameters}});\n").LiquidTemplate;

        public Entity En { get; set; }

        public SolidityStruct(Entity entity)
        {
            En = entity;

            structName = LiquidString.Create(entity.Name);
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

        public LiquidString TypeName()
        {
            var lower = structName.ToString().ToLower();
            return LiquidString.Create(lower.First().ToString().ToUpper() + lower.Substring(1));
        }

        public LiquidString VariableName()
        {
            return LiquidString.Create(structName.ToString().ToLower());
        }

        public LiquidString GetParamteres()
        {
            string s = "{";
            foreach (var p in En.Properties)
            {
                //if(!p.IsCollection) TODO
                  //  s += (Helpers.GetPropertyVariableName(p.Name) + ": " + Helpers.GetDefaultValueString(p) + ", ");
            }
            return LiquidString.Create(s.Trim().Trim(',') + "}");
        }

        public override string ToString(int indent = 0)
        {
            ITemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("indent", CreateIndent(indent)).
                DefineLocalVariable("name", TypeName()).
                DefineLocalVariable("varName", VariableName()).
                DefineLocalVariable("parameters", GetParamteres()).
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