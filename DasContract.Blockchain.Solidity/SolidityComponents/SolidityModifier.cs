using Liquid.NET;
using Liquid.NET.Constants;
using System.Collections.Generic;

namespace DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityModifier : SolidityComponent
    {
        LiquidString modifierName;
        List<SolidityParameter> parameters = new List<SolidityParameter>();
        IList<SolidityComponent> body;

        static readonly LiquidTemplate template = LiquidTemplate.Create("{{indent}}modifier {{name}}" +
            "{% if parameters.size > 0 %}({{parameters}}){% endif %}" +
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

        public SolidityModifier AddParameter(SolidityParameter parameter)
        {
            parameters.Add(parameter);
            return this;
        }

        public SolidityModifier AddParameters(List<SolidityParameter> parameters)
        {
            this.parameters.AddRange(parameters);
            return this;
        }

        LiquidCollection ParametersToLiquid()
        {
            var col = new LiquidCollection();
            foreach (var par in parameters)
            {
                if (par != parameters[parameters.Count - 1])
                    par.Name = par.Name + ", ";
                col.Add(par.ToLiquidString());
            }
            return col;
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
                DefineLocalVariable("parameters", ParametersToLiquid()).
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
