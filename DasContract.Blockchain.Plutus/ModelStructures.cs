using System.Collections.Generic;

namespace DasContract.Blockchain.Plutus
{
    public class SCStateMachine
    {
        public IList<string> States { get; set; } = new List<string>();

        public IList<string> Actions { get; set; } = new List<string>();
    }

    public class SCAttribute
    {
        public string Name { get; set; }

        public string Type { get; set; }
    }

    public class SCFact
    {
        public string Name { get; set; }

        public IList<SCAttribute> Attributes { get; set; } = new List<SCAttribute>();
    }

    public class SCTransactionKind
    {
        public SCTransactionKind ()
        {
            CActs.Add("Initial");
            CActs.Add("Request");
            CActs.Add("Promise");
            CActs.Add("Decline");
            CActs.Add("Quit");
            CActs.Add("State");
            CActs.Add("Accept");
            CActs.Add("Reject");
            CActs.Add("Stop");
        }

        public string Name { get; set; }

        public SCFact Fact { get; set; }

        public IList<string> CActs { get; set; } = new List<string>();
    }
}
