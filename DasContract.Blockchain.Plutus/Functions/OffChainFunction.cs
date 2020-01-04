using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Plutus.Functions
{
    public class OffChainFunction : Function
    {
        public string CoordinationAct { get; set; }

        public string TransactionKindName { get; set; }

        public string ProcessName { get; set; }

        public IList<string> Initializations { get; set; } = new List<string>();

        public IList<string> Responses { get; set; } = new List<string>();

        public IList<string> Conditions { get; set; } = new List<string>();

        public IList<string> ValueActions { get; set; } = new List<string>();

        public IList<string> FactEdits { get; set; } = new List<string>();
    }
}
