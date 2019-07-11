using System;

namespace DasContract
{
    public class ActorRole
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string IdentificationNumber { get; set; }

        public ActorRoleType Type { get; set; }
    }
}