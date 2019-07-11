using System;

namespace DasContract.DataModelClasses
{
    public class EntityType
    {
        public Guid Id;

        public string Name;

        public bool IsExternal;

        public ProductKind _ProductKind;
    }
}