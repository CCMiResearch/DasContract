using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Entities.Integrity.Analysis.Cases
{
    public class ContractIntegrityAnalysisDeleteCase: ContractIntegrityAnalysisCase
    {
        public ContractIntegrityAnalysisDeleteCase(string consequenceMessage, Action resolve)
        {
            ConsequenceMessage = consequenceMessage;
            this.resolve = resolve;
        }

        public string ConsequenceMessage { get; set; }

        readonly Action resolve;

        public void Resolve()
        {
            resolve();
        }
    }
}
