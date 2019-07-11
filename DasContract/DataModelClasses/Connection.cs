using DasContract.Enum;
using System;

namespace DasContract.DataModelClasses
{
    public class Connection
    {
        public Guid Id;

        public string Name;

        public ConnectionCardinality FromCardinality;

        public ConnectionCardinality ToCrdinality;

        public Guid From;

        public Guid To;

        public ConnectionType Type;
    }
}