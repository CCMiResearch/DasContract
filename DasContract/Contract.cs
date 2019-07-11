using System;

namespace DasContract
{
    public class Contract
    {
        public Guid Id;

        public string Name;

        public string Description;

        public ActorRole[] ActorRoles;

        public DataModel[] DataModel;

        public Process[] Processes;

        public ActionRule[] Rules;

        public TransactionKind[] TransactionKinds;
    }
}
