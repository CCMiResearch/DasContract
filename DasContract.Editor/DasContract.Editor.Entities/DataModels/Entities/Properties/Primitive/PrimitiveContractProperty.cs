using DasContract.Editor.Entities.DataModels.Entities.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DasContract.Editor.Entities.DataModels.Entities.Properties.Primitive
{
    public class PrimitiveContractProperty : ContractProperty
    {
        /// <summary>
        /// Data type of this property
        /// </summary>
        [DisplayName("Data type")]
        [Description("Sets a data type for this property")]
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
