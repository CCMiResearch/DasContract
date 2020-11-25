using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Data
{
    public class Token : Entity
    {
        public string Symbol { get; set; }
        public bool IsFungible { get; set; }
        public bool IsIssued { get; set; }

        public string MintScript { get; set; }
        public string TransferScript { get; set; }
    }
}
