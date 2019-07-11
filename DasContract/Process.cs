using System.Collections.Generic;
using System;

namespace DasContract
{
    public class Process
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Transactor Root { get; set; }

        public IList<string> Implementation { get; set; } = new List<string>();
    }
}