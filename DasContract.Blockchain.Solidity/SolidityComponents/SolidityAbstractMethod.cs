using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.SolidityComponents
{
    public abstract class SolidityAbstractMethod: SolidityComponent
    {
        protected List<SolidityParameter> parameters = new List<SolidityParameter>();
        protected List<SolidityComponent> body = new List<SolidityComponent>();

        public void AddParameter(SolidityParameter parameter)
        {
            parameters.Add(parameter);
        }

        public void AddParameters(List<SolidityParameter> parameters)
        {
            this.parameters.AddRange(parameters);
        }


        public void AddToBody(SolidityComponent component)
        {
            if (component != null)
                body.Add(component);
        }

        public void AddToBody(List<SolidityComponent> components)
        {
            foreach (var c in components)
            {
                AddToBody(c);
            }
        }

        protected LiquidCollection ParametersToLiquid()
        {
            var col = new LiquidCollection();
            foreach (var par in parameters)
            {
                if (par != parameters[parameters.Count - 1])
                    par.Name = par.Name + ", ";
                col.Add(par.ToLiquidString());
            }
            return col;
        }

        protected LiquidCollection BodyToLiquid(int indent)
        {
            var col = new LiquidCollection();
            foreach (var b in body)
                col.Add(b.ToLiquidString(indent + 1));
            return col;
        }
    }
}
