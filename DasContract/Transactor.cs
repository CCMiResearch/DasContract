using System.Collections.Generic;
using System;

namespace DasContract
{
    public abstract class Transactor
    {
        public Guid Id { get; set; }

        public Guid ActorRole { get; set; }

        public IList<WaitLink> WaitLinks { get; set; } = new List<WaitLink>();

        public IList<InspectionLink> InspectionLinks { get; set; } = new List<InspectionLink>();
    }
}