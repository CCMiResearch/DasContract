using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace BpmnToSolidity.SolidityConverter
{
    public class SolidityStatement : SolidityComponent
    {
        string statement;
        public SolidityStatement(string statement)
        {
            this.statement = statement;
        }
        public override LiquidString ToLiquidString(int indent)
        {
            return LiquidString.Create(ToString(indent));
        }

        public override string ToString(int indent = 0)
        {
            return CreateIndent(indent) + statement + ";" + "\n";
        }
    }
}
