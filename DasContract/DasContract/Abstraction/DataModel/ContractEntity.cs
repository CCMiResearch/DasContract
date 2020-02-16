using DasContract.Abstraction.DataModel.Property.Primitive;
using DasContract.Abstraction.DataModel.Property.Reference;
using DasContract.DasContract.Abstraction.Interface;
using System.Collections.Generic;

namespace DasContract.Abstraction.DataModel
{
    public class ContractEntity: IIdentifiable
    {
        public string Id { get; set; }

        /// <summary>
        /// Display name of this contract entity
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Primitive properties of this entity
        /// </summary>
        public IList<PrimitiveContractProperty> PrimitiveProperties { get; set; } = new List<PrimitiveContractProperty>();

        /// <summary>
        /// Reference properties of this entity
        /// </summary>
        public IList<ReferenceContractProperty> ReferenceProperties { get; set; } = new List<ReferenceContractProperty>();
    }
}
