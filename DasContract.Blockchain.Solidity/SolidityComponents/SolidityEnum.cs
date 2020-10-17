using Liquid.NET;
using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityEnum : SolidityComponent
    {
        LiquidString enumName;
        List<string> values;

        LiquidTemplate template = LiquidTemplate.Create(
            "{{indent}}enum {{name}}{ {{values}} }").LiquidTemplate;

        public SolidityEnum(string enumName)
        {
            this.enumName = LiquidString.Create(enumName);
            values = new List<string>();
        }

        public void Add(string value)
        {
            values.Add(value);
        }

        public void Add(IList<string> values)
        {
            this.values.AddRange(values);
        }

        public override LiquidString ToLiquidString(int indent)
        {
            return LiquidString.Create(ToString(indent));
        }

        LiquidCollection ValuesToLiquid()
        {
            //TODO add exceptions
            var liquidCollection = new LiquidCollection();
            foreach(var value in values.Take(values.Count() - 1))
            {
                liquidCollection.Add(LiquidString.Create($"{value}, "));
            }
            liquidCollection.Add(LiquidString.Create(values.Last()));
            return liquidCollection;
        }

        public override string ToString(int indent = 0)
        {
            ITemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("indent", CreateIndent(indent)).
                DefineLocalVariable("enumName", enumName).
                DefineLocalVariable("values", ValuesToLiquid());
            return template.Render(ctx).Result;
        }
    }
}
