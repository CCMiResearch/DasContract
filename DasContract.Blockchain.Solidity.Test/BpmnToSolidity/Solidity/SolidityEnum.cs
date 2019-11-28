using System;
using System.Collections.Generic;
using System.Text;

namespace BpmnToSolidity.SolidityConverter
{
    /*
    class SolidityEnum : SolidityComponent
    {
        List<string> enumList;
        string name;
        public SolidityEnum(string name, List<string> enumList)
        {
            this.name = name;
            this.enumList = enumList;
        }
        public override string GenerateCode()
        {
            string code = CreateIndent(indent) + "enum " + name + " {";
            bool first = true;
            foreach(var e in enumList)
            {
                if (first) first = false;
                else code += ",";
                code += e;
            }
            code += "}\n";
            return code;
        }
    }
    */
}
