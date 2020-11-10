using Liquid.NET;
using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityFor: SolidityComponent
    {
        static readonly LiquidTemplate template = LiquidTemplate.Create("{{indent}}for(int {{varName}} = 0; {{varName}} < {{loopCount}}; {{varName}}++){ \n" +
            "{{body}}" +
            "{{indent}}}\n").LiquidTemplate;

        string loopVariableName;
        string loopCountVariable;

        SolidityStatement body;

        public SolidityFor(string loopVariableName, string loopCountVariable)
        {
            body = new SolidityStatement();
            this.loopVariableName = loopVariableName;
            this.loopCountVariable = loopCountVariable;
        }

        public void AddToBody(SolidityStatement statement)
        {
            body.Add(statement);
        }

        public override LiquidString ToLiquidString(int indent)
        {
            return LiquidString.Create(ToString(indent));
        }

        public override string ToString(int indent = 0)
        {
            ITemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("indent", CreateIndent(indent)).
                DefineLocalVariable("varName", LiquidString.Create(loopVariableName)).
                DefineLocalVariable("loopCount", LiquidString.Create(loopCountVariable)).
                DefineLocalVariable("body", body.ToLiquidString(indent + 1));
            return template.Render(ctx).Result;
        }

    }
}

