using BpmnToSolidity.SolidityConverter;
using Liquid.NET;
using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Solidity
{
    class SolidityContract : SolidityComponent
    {
        string name;

        List<SolidityFunction> functions;

        LiquidTemplate template = LiquidTemplate.Create(
            "contract {{name}} { \n" +
            "{{functions}} " +
            "}").LiquidTemplate;

        SolidityContract(string name)
        {
            this.name = name;

        }

        public void AddFunction(SolidityFunction function)
        {
            functions.Add(function);
        }

        public override LiquidString ToLiquidString()
        {
            var ctx = new TemplateContext();
            ctx.DefineLocalVariable("functions", functionsToLiquid());

            return LiquidString.Create(template.Render(ctx).Result);
        }

        LiquidCollection functionsToLiquid()
        {
            var col = new LiquidCollection();
            foreach (var f in functions)
                col.Add(f.ToLiquidString());
            return col;
        }
    }
}
