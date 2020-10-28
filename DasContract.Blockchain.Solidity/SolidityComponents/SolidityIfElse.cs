using Liquid.NET;
using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.SolidityComponents
{

    public class SolidityIfElse : SolidityComponent
    {

        IList<IfBlock> conditionBlocks = new List<IfBlock>();

        LiquidTemplate template = LiquidTemplate.Create("{{conditionBlocks}}").LiquidTemplate;

        public SolidityIfElse AddConditionBlock(string condition, SolidityComponent blockBody)
        {
            var conditionBlock = new IfBlock(condition, blockBody, conditionBlocks.Count > 0);
            conditionBlocks.Add(conditionBlock);
            return this;
        }
        
        public override LiquidString ToLiquidString(int indent)
        {
            return LiquidString.Create(ToString(indent));
        }

        public override string ToString(int indent = 0)
        {
            ITemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("conditionBlocks", ConditionBlocksToLiquid(indent));
            return template.Render(ctx).Result;
        }

        LiquidCollection ConditionBlocksToLiquid(int indent)
        {
            var col = new LiquidCollection();
            foreach (var c in conditionBlocks)
                col.Add(c.ToLiquidString(indent));
            return col;
        }
    }

    class IfBlock : SolidityComponent
    {
        string condition;
        SolidityComponent blockBody;
        bool isElse;

        static readonly LiquidTemplate template = LiquidTemplate.Create("{{indent}}{% if isElse == true%}else {% endif%}" +
            "if({{condition}}){\n" +
            "{{blockBody}}"+
            "{{indent}}}\n").LiquidTemplate;

        public IfBlock(string condition, SolidityComponent blockBody, bool isElse)
        {
            this.condition = condition;
            this.blockBody = blockBody;
            this.isElse = isElse;
        }
        public override LiquidString ToLiquidString(int indent)
        {
            return LiquidString.Create(ToString(indent));
        }

        public override string ToString(int indent = 0)
        {
            ITemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("indent", CreateIndent(indent)).
                DefineLocalVariable("condition", LiquidString.Create(condition)).
                DefineLocalVariable("isElse", new LiquidBoolean(isElse)).
                DefineLocalVariable("blockBody", blockBody.ToLiquidString(indent + 1));
            return template.Render(ctx).Result;
        }
    }
}
