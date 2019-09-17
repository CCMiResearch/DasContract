using System;

namespace DasContract
{
    public enum TransactionSort
    {
        Original,
        Physical,
        Informational,
        Documental
    }

    public class TransactionKind
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public TransactionSort TransactionSort { get; set; }

        public Guid Executor { get; set; }

        public string IdentificationNumber { get; set; }
    }
}