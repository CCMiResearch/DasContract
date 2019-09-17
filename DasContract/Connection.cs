using System;

namespace DasContract
{
    public enum ConnectionType
    {
        Generalisation,
        Specialisation,
        Aggregation,
        Reference,
        Concerns,
        Excludes,
        Precedes,
        Preludes
    }

    public class Connection
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ConnectionCardinality FromCardinality { get; set; }

        public ConnectionCardinality ToCardinality { get; set; }

        public Guid From { get; set; }

        public Guid To { get; set; }

        public ConnectionType Type { get; set; }
    }
}