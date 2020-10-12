using System.Collections.Generic;

namespace DasContract.Abstraction.Data
{
    public class Entity: DataType
    {
        public IList<Property> Properties { get; set; } = new List<Property>(); 
    }
}
