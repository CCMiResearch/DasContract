using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.DataModel.Property.Primitive
{
    public class PrimitiveContractProperty: ContractProperty
    {
        public PrimitiveContractPropertyType Type
        {
            get => type;
            set
            {
                if (value != type)
                    migrator.Notify(() => type, b => type = b);
                type = value;
            }
        }
        PrimitiveContractPropertyType type;
    }
}
