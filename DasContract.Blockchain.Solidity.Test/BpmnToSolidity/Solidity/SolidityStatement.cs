using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace BpmnToSolidity.SolidityConverter
{
    class SolidityStatement : SolidityComponent
    {
        string statement;
        public SolidityStatement(string statement)
        {
            this.statement = statement;
        }
        public override LiquidString ToLiquidString()
        {
            return LiquidString.Create(CreateIndent(indent) + statement + ";" + "\n");
        }
    }
}
