using System;

namespace DasContract.ProcessClasses
{
    public class Transactor
    {
        public Guid Id;

        public Guid ActorRole;

        public WaitLink[] WaitLinks;

        public InspectionLink[] InspectionLinks;
    }
}