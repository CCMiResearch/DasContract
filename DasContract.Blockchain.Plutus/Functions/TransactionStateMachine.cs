using System.Collections.Generic;
using System.Linq;

namespace DasContract.Blockchain.Plutus.Functions
{
    public class TransactionStateMachine : Function
    {
        public string TransactionKindName { get; set; }

        public string ProcessName { get; set; }

        public string Fact { get; set; }

        public string CoordinationAct { get; set; }

        public string Parameters { get; set; }
    }
}