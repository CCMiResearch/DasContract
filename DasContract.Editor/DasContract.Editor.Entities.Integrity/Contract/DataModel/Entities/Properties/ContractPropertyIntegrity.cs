using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Entities.DataModels;
using DasContract.Editor.Entities.DataModels.Entities;
using DasContract.Editor.Entities.DataModels.Entities.Properties;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Primitive;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Reference;
using DasContract.Editor.Entities.Integrity.Analysis;
using DasContract.Editor.Entities.Integrity.Analysis.Cases;
using DasContract.Editor.Entities.Integrity.Extensions;

namespace DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities.Properties
{
    public static class ContractPropertyIntegrity
    {
        //--------------------------------------------------
        //               PROPERTY ANALYSIS
        //--------------------------------------------------

        public static List<ContractIntegrityAnalysisDeleteCase> AnalyzeDeleteRisksOf(this EditorContract contract, ContractProperty property)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (property == null)
                throw new ArgumentNullException(nameof(property));

            var deleteRisks = new List<ContractIntegrityAnalysisDeleteCase>();

            //Check for contract property bindings delete risks
            foreach (var activity in contract.Processes.Main.UserActivities)
            {
                foreach (var field in activity.Form.Fields)
                {
                    if (field.PropertyBinding == null || field.PropertyBinding.Property != property)
                        continue;

                    var currentField = field;
                    deleteRisks.Add(new ContractIntegrityAnalysisDeleteCase(
                        $"Property binding on form field {currentField.Name} will be removed",
                        () => { currentField.PropertyBinding = null; }));
                }
            }

            return deleteRisks;
        }


    }
}
