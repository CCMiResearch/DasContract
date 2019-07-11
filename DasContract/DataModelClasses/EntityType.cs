using System;

namespace DasContract
{
    public class EntityType
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsExternal { get; set; }

        public ProductKind ProductKind { get; set; }
    }
}