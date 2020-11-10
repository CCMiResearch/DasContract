using Liquid.NET;
using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityFunction : SolidityAbstractMethod
    {
        LiquidString functionName;
        LiquidString visibility;
        LiquidString returns;
        bool isView;
        
        List<string> modifiers;

        static readonly LiquidTemplate template = LiquidTemplate.Create("{{indent}}function {{name}}(" +
            "{{parameters}}) " +
            "{{modifiers}}" +
            "{{visibility}} " +
            "{% unless isView == false %}view {% endunless %}" +
            "{% unless returns == ''%}returns({{returns}}){% endunless %}" +
            "{\n" +
            "{{body}}" +
            "{{indent}}}\n").LiquidTemplate;

        public SolidityFunction(string functionName, SolidityVisibility visibility, string returns = "", bool isView = false) 
        {
            this.functionName = LiquidString.Create(functionName);
            this.visibility = LiquidString.Create(visibility.ToString().ToLower());
            this.returns = LiquidString.Create(returns);
            this.isView = isView;

            
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

        public void AddModifier(string modifier)
        {
            modifiers.Add(modifier);
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
                DefineLocalVariable("isView", new LiquidBoolean(isView)).
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

        
    }
}
