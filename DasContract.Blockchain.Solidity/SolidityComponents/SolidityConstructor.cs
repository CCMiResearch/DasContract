using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Abstraction.Data;
using Liquid.NET;
using Liquid.NET.Constants;

namespace DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityConstructor : SolidityAbstractMethod
    {
        List<string> parentCalls = new List<string>();

        static readonly LiquidTemplate template = LiquidTemplate.Create("{{indent}}constructor" +
            "({{parameters}}) " +
            "{{parentCalls}}" +
            "public payable" +
            "{\n" +
            "{{body}}" +
            "{{indent}}}\n").LiquidTemplate;

        public void AddParentCall(string parentCallName, List<string> propertyNames)
        {
            var joinedPropertyNames = "";
            if(propertyNames.Count > 0)
                joinedPropertyNames = string.Join(", ", propertyNames);
            parentCalls.Add($"{parentCallName}({joinedPropertyNames}) ");
        }

        LiquidCollection ParentCallsToLiquid()
        {
            var liquidCollection = new LiquidCollection();
            foreach (var parentCall in parentCalls)
            {
                liquidCollection.Add(LiquidString.Create(parentCall));
            }
            return liquidCollection;
        }

        public override LiquidString ToLiquidString(int indent)
        {
            return LiquidString.Create(ToString(indent));
        }

        public override string ToString(int indent = 0)
        {
            ITemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("indent", CreateIndent(indent)).
                DefineLocalVariable("body", BodyToLiquid(indent)).
                DefineLocalVariable("parameters", ParametersToLiquid()).
                DefineLocalVariable("parentCalls", ParentCallsToLiquid());
            return template.Render(ctx).Result;
        }
    }
}
