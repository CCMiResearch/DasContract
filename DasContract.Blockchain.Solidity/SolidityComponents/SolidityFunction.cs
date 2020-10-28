using Liquid.NET;
using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityFunction : SolidityComponent
    {
        LiquidString visibility;
        LiquidString functionName;
        LiquidString returns;
        IList<SolidityParameter> parameters;
        IList<SolidityComponent> body;
        IList<string> modifiers;

        static readonly LiquidTemplate template = LiquidTemplate.Create("{{indent}}function {{name}}(" +
            "{{parameters}}) " +
            "{{modifiers}}" +
            "{{visibility}} " +
            "{% unless returns == ''%}returns({{returns}}){% endunless %}" +
            "{\n" +
            "{{body}}" +
            "{{indent}}}\n").LiquidTemplate;

        public SolidityFunction(string functionName, SolidityVisibility visibility, string returns = "") 
        {
            this.functionName = LiquidString.Create(functionName);
            this.visibility = LiquidString.Create(visibility.ToString().ToLower());
            this.returns = LiquidString.Create(returns);

            parameters = new List<SolidityParameter>();
            body = new List<SolidityComponent>();
            modifiers = new List<string>();

            // Has to be payable and has to be public (for now, solidity v0.6.x)
            // TODO: Payable from DAS contract field instead of function name
            if (functionName.ToLower().EndsWith("payable"))
            {
                modifiers.Add("payable");
                if (visibility == SolidityVisibility.Internal)
                    this.visibility = LiquidString.Create("public");
            }
        }

        public SolidityFunction AddParameter(SolidityParameter parameter)
        {
            parameters.Add(parameter);
            return this;
        }

        public SolidityFunction AddModifier(string modifier)
        {
            modifiers.Add(modifier);
            return this;
        }

        public SolidityFunction AddToBody(SolidityComponent component)
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
                DefineLocalVariable("name", functionName).
                DefineLocalVariable("parameters", ParametersToLiquid()).
                DefineLocalVariable("visibility", visibility).
                DefineLocalVariable("body", BodyToLiquid(indent)).
                DefineLocalVariable("modifiers", ModifiersToLiquid()).
                DefineLocalVariable("returns", returns);
            return template.Render(ctx).Result;
        }

        LiquidCollection ModifiersToLiquid()
        {
            var col = new LiquidCollection();
            foreach (var mod in modifiers)
                col.Add(LiquidString.Create(mod + " "));
            return col;
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

        LiquidCollection BodyToLiquid(int indent)
        {
            var col = new LiquidCollection();
            foreach (var b in body)
                col.Add(b.ToLiquidString(indent + 1));
            return col;
        }
    }
}
