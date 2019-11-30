using Liquid.NET;
using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace BpmnToSolidity.SolidityConverter
{
    public class SolidityFunction : SolidityComponent
    {
        LiquidString visibility;
        LiquidString functionName;
        LiquidString returns;
        bool view;
        IList<SolidityParameter> parameters;
        IList<SolidityComponent> body;
        IList<string> modifiers;

        LiquidTemplate template = LiquidTemplate.Create("{{indent}}function {{name}}" +
            //" {{ parameters | join: ', '}}) " +
            " {{modifiers}}" +
            "{{visibility}} " +
            "{% unless returns == ''%}returns {{returns}}{% endunless %}" +
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
        }

        public SolidityFunction addParameter(SolidityParameter parameter)
        {
            parameters.Add(parameter);
            return this;
        }

        public SolidityFunction AddModifier(string modifier)
        {
            modifiers.Add(modifier);
            return this;
        }

        public SolidityFunction addToBody(SolidityComponent component)
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
                DefineLocalVariable("parameters", parametersToLiquid()).
                DefineLocalVariable("visibility", visibility).
                DefineLocalVariable("body", bodyToLiquid(indent)).
                DefineLocalVariable("modifiers", modifiersToLiquid()).
                DefineLocalVariable("returns", returns);
            return template.Render(ctx).Result;
        }

        LiquidCollection modifiersToLiquid()
        {
            var col = new LiquidCollection();
            foreach (var mod in modifiers)
                col.Add(LiquidString.Create(mod + " "));
            return col;
        }

        LiquidCollection parametersToLiquid()
        {
            var col = new LiquidCollection();
            foreach (var par in parameters)
                col.Add(par.ToLiquidString());
            return col;
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
