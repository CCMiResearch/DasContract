using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace BpmnToSolidity.SolidityConverter
{
    /// <summary>
    /// All of the needed Solidity constructs inherit from this class, which provides
    /// the necessary interface for converting those constructs into Solidity.
    /// </summary>
    public abstract class SolidityComponent
    {
        protected SolidityComponent ()
        {
        }

        public abstract LiquidString ToLiquidString(int indent);

        public abstract string ToString(int indent = 0);

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
