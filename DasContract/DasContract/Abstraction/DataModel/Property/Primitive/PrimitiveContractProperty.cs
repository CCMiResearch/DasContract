using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.DataModel.Property.Primitive
{
    public class PrimitiveContractProperty: ContractProperty<PrimitiveContractPropertyType>
    {
        public override PrimitiveContractPropertyType Type { get; set; }
    }
}
