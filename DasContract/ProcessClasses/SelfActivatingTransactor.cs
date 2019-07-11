using System;

namespace DasContract.ProcessClasses
{
    public class SelfActivatingTransactor : Transactor
    {
        public Guid TransactionKind;

        public ElementaryTransactor[] Children;
    }
}