using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Entities.Integrity.Analysis.Cases;

namespace DasContract.Editor.Entities.Integrity.Analysis
{
    public class ContractIntegrityAnalysisResult
    {
        public ContractIntegrityAnalysisResult(List<ContractIntegrityAnalysisDeleteCase> deleteRisks,
            List<ContractIntegrityAnalysisResult> childrenAnalyses = null)
        {
            DeleteRisks = deleteRisks;
            if (childrenAnalyses != null)
                ChildrenAnalyses = childrenAnalyses;
        }

        public List<ContractIntegrityAnalysisDeleteCase> DeleteRisks { get; private set; } = new List<ContractIntegrityAnalysisDeleteCase>();

        public List<ContractIntegrityAnalysisResult> ChildrenAnalyses { get; private set; } = new List<ContractIntegrityAnalysisResult>();

        public bool HasDeleteRisks()
        {
            if (DeleteRisks.Count > 0)
                return true;
            foreach (var child in ChildrenAnalyses)
                if (child.HasDeleteRisks())
                    return true;
            return false;
        }

        public void ResolveDeleteRisks()
        {
            foreach (var childAnalysin in ChildrenAnalyses)
                childAnalysin.ResolveDeleteRisks();
            foreach (var risk in DeleteRisks)
                risk.Resolve();
        }

        public static ContractIntegrityAnalysisResult Empty()
        {
            return new ContractIntegrityAnalysisResult(new List<ContractIntegrityAnalysisDeleteCase>());
        }
    }
}
