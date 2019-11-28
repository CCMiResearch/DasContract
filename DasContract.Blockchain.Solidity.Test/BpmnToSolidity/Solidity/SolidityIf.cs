using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace BpmnToSolidity.SolidityConverter
{
    /*
    class SolidityIf : SolidityComponent
    {

        string condition;
        bool isElse;
        List<SolidityComponent> body;
        public SolidityIf(string condition, bool isElse)
        {
            this.isElse = isElse;
            this.condition = condition;
            body = new List<SolidityComponent>();
        }

        public SolidityIf AddToBody(SolidityComponent component)
        {
            if (component == null) throw new ArgumentNullException();
            component.setIndent(indent + 1);
            body.Add(component);
            return this;
        }
        public override LiquidString ToLiquidString()
        {
            string code = CreateIndent(indent) + GenerateElseIf() + "(" + condition + ") {\n";
            foreach (var component in body)
            {
                code += component.GenerateCode();
            }
            code += CreateIndent(indent) + "}\n";
            return code;
        }

        string GenerateElseIf()
        {
            if (isElse) return "else if";
            return "if";
        }
    }
    */
}
