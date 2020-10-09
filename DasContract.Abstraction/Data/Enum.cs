using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Data
{
    public class Enum
    {
        public IList<string> Values { get; set; } = new List<string>();
    }
}
