using System;
using System.Collections.Generic;
using System.Text;

namespace BpmnToSolidity.SolidityConverter
{
    /*
    class SolidityConstructor : SolidityComponent
    {
        string firstState;
        List<SolidityComponent> body;
        public SolidityConstructor(string firstState)
        {
            this.firstState = firstState;
            body = new List<SolidityComponent>();
        }
        public override string GenerateCode()
        {
            string code = CreateIndent(indent) + "constructor() public {\n";
            code += CreateIndent(indent + 1) + "state = State." + firstState + ";\n";
            code += CreateIndent(indent) + "}\n";
            return code;
        }
    }
    */
}
