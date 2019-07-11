using System;

namespace DasContract
{
    public class AttributeType
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public AttributeValueType ValueType { get; set; }

        public Guid EntityType { get; set; }
    }
}