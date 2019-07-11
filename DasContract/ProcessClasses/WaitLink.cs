using System;

namespace DasContract
{
    public class WaitLink
    {
        public Guid Id { get; set; }

        public Guid WaitingForTransactor { get; set; }
    }
}