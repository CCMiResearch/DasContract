namespace DasContract.Abstraction.Data
{
    public class Property
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsMandatory { get; set; } = true;
        public bool IsCollection { get; set; } = false;
        public PropertyType Type { get; set; }
        /// <summary>
        /// A linked entity in case of Type=PropertyType.Entity. 
        /// </summary>
        public Entity Entity { get; set; }
    }
}
