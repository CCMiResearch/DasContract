using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Entities.Integrity.Analysis;
using DasContract.Editor.Entities.Integrity.Analysis.Cases;
using DasContract.Editor.Entities.Processes.Diagrams;
using DasContract.Editor.Entities.Processes.Process.Activities;

namespace DasContract.Editor.Entities.Integrity.Contract.Processes.Process.Activities
{
    public static class ContractBusinessRuleActivityIntegrity
    {
        public static void ReplaceSafely(this EditorContract contract, ContractBusinessRuleActivity activity, DMNProcessDiagram newDiagram)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            if (newDiagram == null)
                throw new ArgumentNullException(nameof(newDiagram));

            if (DMNProcessDiagram.IsNullOrEmpty(newDiagram))
            {
                activity.Diagram = newDiagram;  
                return;
            }

            activity.Diagram = newDiagram;
        }

        public static void ValidatePotentialDiagram(this EditorContract contract, ContractBusinessRuleActivity activity, DMNProcessDiagram newDiagram)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            if (newDiagram == null)
                throw new ArgumentNullException(nameof(newDiagram));
        }

        public static ContractIntegrityAnalysisResult AnalyzeIntegrityWhenReplacedWith(this EditorContract contract, ContractBusinessRuleActivity activity, DMNProcessDiagram newDiagram)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            if (newDiagram == null)
                throw new ArgumentNullException(nameof(newDiagram));

            var deleteRisks = new List<ContractIntegrityAnalysisDeleteCase>();
            var childrenAnalyses = new List<ContractIntegrityAnalysisResult>();

            return new ContractIntegrityAnalysisResult(deleteRisks, childrenAnalyses);
        }
    }
}
