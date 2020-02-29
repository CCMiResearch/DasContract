using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Entities.Integrity.Analysis;
using DasContract.Editor.Entities.Processes.Process.Activities;

namespace DasContract.Editor.Entities.Integrity.Contract.Processes.Process.Activities
{
    public static class ContractActivityIntegrity
    {
        public static ContractIntegrityAnalysisResult AnalyzeIntegrityOf(this EditorContract contract, ContractActivity activity)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            return ContractIntegrityAnalysisResult.Empty();
        }
    }
}
