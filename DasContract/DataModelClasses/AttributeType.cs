using DasContract.Enum;
using System;

namespace DasContract.DataModelClasses
{
    public class AttributeType
    {
        public Guid Id;

        public string Name;

        public AttributeValueType ValueType;

        public Guid EntityType;
    }
}