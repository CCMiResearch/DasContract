using System.Collections.Generic;

namespace DasContract.Abstraction.Data
{
    public class Entity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IList<Property> Properties { get; set; } = new List<Property>(); 
    }
}
