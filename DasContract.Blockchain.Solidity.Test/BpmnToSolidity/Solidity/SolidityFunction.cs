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

        LiquidTemplate template = LiquidTemplate.Create("{{indent}}function {{name}}(" +
            " {{ parameters | join: ', '}}) " +
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
        }

        public SolidityFunction addParameter(SolidityParameter parameter)
        {
            parameters.Add(parameter);
            return this;
        }

        public SolidityFunction addToBody(SolidityComponent component)
        {
            component.setIndent(indent + 1);
            body.Add(component);
            return this;
        }

        public override LiquidString ToLiquidString()
        {
            ITemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("indent", CreateIndent(indent)).
                DefineLocalVariable("name", functionName).
                DefineLocalVariable("parameters", parametersToLiquid()).
                DefineLocalVariable("visibility", visibility).
                DefineLocalVariable("body", bodyToLiquid()).
                DefineLocalVariable("returns", returns);
            return LiquidString.Create(template.Render(ctx).Result);
        }

        LiquidCollection parametersToLiquid()
        {
            var col = new LiquidCollection();
            foreach (var par in parameters)
                col.Add(par.ToLiquidString());
            return col;
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
