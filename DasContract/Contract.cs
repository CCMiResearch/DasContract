using System.Collections.Generic;
using System;

namespace DasContract
{
    public enum CardinalityOption
    {
        Zero,
        One,
        More
    }

    public class Contract
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IList<ActorRole> ActorRoles { get; set; } = new List<ActorRole>();

        public IList<DataModel> DataModels { get; set; } = new List<DataModel>();

        public IList<Process> Processes { get; set; } = new List<Process>();

        public IList<ActionRule> Rules { get; set; } = new List<ActionRule>();

        public IList<TransactionKind> TransactionKinds { get; set; } = new List<TransactionKind>();
    }
}
