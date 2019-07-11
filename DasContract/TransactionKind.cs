using System;

namespace DasContract
{
    public class TransactionKind
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public TransactionSort TrasactionSort { get; set; }

        public Guid Executor { get; set; }

        public string IdentificationNumber { get; set; }
    }
}