using DasContract.Editor.Entities.DataModels.Entities.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Entities.DataModels.Entities.Properties.Primitive
{
    public class PrimitiveContractProperty : ContractProperty
    {
        /// <summary>
        /// Data type of this property
        /// </summary>
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
