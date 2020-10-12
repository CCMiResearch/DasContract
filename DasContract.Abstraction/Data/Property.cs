namespace DasContract.Abstraction.Data
{
    public class Property
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsMandatory { get; set; } = true;

        public PropertyType PropertyType { get; set; }

        /// <summary>
        /// The keytype in case of PropertyType=PropertyType.Dictionary
        /// </summary>
        public PropertyDataType KeyType { get; set; }
        public PropertyDataType DataType { get; set; }
        /// <summary>
        /// A linked entity's id in case of Type=PropertyDataType.Entity. 
        /// </summary>
        public string ReferencedDataType { get; set; }
    }
}
