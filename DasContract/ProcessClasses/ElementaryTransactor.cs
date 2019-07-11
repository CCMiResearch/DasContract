using System.Collections.Generic;
using System;

namespace DasContract
{
    public class ElementaryTransactor : Transactor
    {
        public Guid TransactionKind { get; set; }

        public TransactionCardinality Cardinality { get; set; }

        public CAct SourceCAct { get; set; }

        public IList<ElementaryTransactor> Children { get; set; } = new List<ElementaryTransactor>();
    }
}