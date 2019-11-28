using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace BpmnToSolidity.SolidityConverter
{

    class SolidityParameter
    {
        string dataType;
        string name;

        public SolidityParameter(string dataType, string name)
        {
            this.dataType = dataType;
            this.name = name;
        }

        public LiquidString ToLiquidString()
        {
            return LiquidString.Create(dataType + " " + name);
        }
    }
}
