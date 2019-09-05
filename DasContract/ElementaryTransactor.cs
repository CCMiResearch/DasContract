using System.Collections.Generic;
using System;

namespace DasContract
{
    public enum CAct
    {
        Request,
        Promise,
        Decline,
        Quit,
        Execute,
        State,
        Accept,
        Reject,
        Stop,
        RevokeRequest,
        RevokePromise,
        RevokeState,
        RevokeAccept
    }

    public class ElementaryTransactor : Transactor
    {
        public Guid TransactionKind { get; set; }

        public TransactionCardinality Cardinality { get; set; }

        public CAct SourceCAct { get; set; }

        public IList<ElementaryTransactor> Children { get; set; } = new List<ElementaryTransactor>();
    }
}