using System;

namespace DasContract
{
    public enum AttributeValueType
    {
        String,
        Int,
        UInt,
        Address
    }

    public class AttributeType
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public AttributeValueType ValueType { get; set; }

        public Guid EntityType { get; set; }
    }
}