using System.Collections.Generic;
using System;

namespace DasContract
{
    public class DataModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IList<EntityType> EntityTypes { get; set; } = new List<EntityType>();

        public IList<AttributeType> AttributeTypes { get; set; } = new List<AttributeType>();

        public IList<Connection> Connections { get; set; } = new List<Connection>();
    }
}