using DasContract.DasContract.Abstraction.Interface;

namespace DasContract.Abstraction.DataModel.Property
{
    public abstract class ContractProperty<TType> : IIdentifiable, INamable
    {
        public string Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Tells if this property is allowed to be the default value or not
        /// </summary>
        public bool IsMandatory { get; set; } = true;

        /// <summary>
        /// Data type of this property
        /// </summary>
        public abstract TType Type { get; set; }
    }
}
