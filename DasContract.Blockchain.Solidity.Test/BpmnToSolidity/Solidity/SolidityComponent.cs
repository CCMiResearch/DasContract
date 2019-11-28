using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace BpmnToSolidity.SolidityConverter
{
    abstract class SolidityComponent
    {
        protected int indent;
        protected SolidityComponent ()
        {
        }

        public abstract LiquidString ToLiquidString();

        public void setIndent(int indent)
        {
            if (indent >= 0) this.indent = indent;
        }

        protected static LiquidString CreateIndent(int indent)
        {
            var indentedString = new StringBuilder();
            for(int i = 0; i < indent; i++)
            {
                indentedString.Append("\t");
            }
            return LiquidString.Create(indentedString.ToString());
        }
    }
}
