using System.Collections.Generic;
using System;

namespace DasContract
{
    public class SelfActivatingTransactor : Transactor
    {
        public Guid TransactionKind { get; set; }

        public IList<ElementaryTransactor> Children { get; set; } = new List<ElementaryTransactor>();
    }
}