using DasContract.Enum;
using System;

namespace DasContract.ProcessClasses
{
    public class ElementaryTransactor : Transactor
    {
        public Guid TransactionKind;

        public TransactionCardinality Cardinality;

        public CAct SourceCAct;

        public ElementaryTransactor[] Children;
    }
}