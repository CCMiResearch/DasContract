using DasContract.Enum;
using System;

namespace DasContract
{
    public class TransactionKind
    {
        public Guid Id;

        public string Name;

        public TransactionSort _TrasactionSort;

        public Guid Executor;

        public string IdentificationNumber;
    }
}