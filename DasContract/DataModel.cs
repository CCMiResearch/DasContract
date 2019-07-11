using DasContract.DataModelClasses;
using System;

namespace DasContract
{
    public class DataModel
    {
        public Guid Id;

        public string Name;

        public EntityType[] EntityTypes;

        public AttributeType[] AttributeTypes;

        public Connection[] Connections;
    }
}