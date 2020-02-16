using DasContract.DasContract.Abstraction.Interface;

namespace DasContract.Abstraction.Data
{
    public class ContractProperty: IIdentifiable
    {
        public string Id { get; set; }


        public string Name { get; set; }
        public bool IsMandatory { get; set; } = true;

        public PropertyType Type { get; set; }
        /// <summary>
        /// A linked entity in case of Type=PropertyType.Entity. 
        /// </summary>
        public ContractEntity Entity { get; set; }
    }
}
