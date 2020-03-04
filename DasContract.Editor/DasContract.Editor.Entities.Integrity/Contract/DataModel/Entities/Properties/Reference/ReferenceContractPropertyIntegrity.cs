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
using DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities.Properties;
using DasContract.Editor.Entities.Integrity.Extensions;

namespace DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities.Properties.Reference
{
    public static class ReferenceContractPropertyIntegrity
    {
        //--------------------------------------------------
        //               REFERENCE PROPERTY
        //--------------------------------------------------
        public static void AddSafely(this EditorContract contract, ContractEntity entity, ReferenceContractProperty property)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (property == null)
                throw new ArgumentNullException(nameof(property));

            //entity.ReferenceProperties.Add(property);
            entity.AddProperty(property);
        }

        public static void RemoveSafely(this EditorContract contract, ReferenceContractProperty property)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (property == null)
                throw new ArgumentNullException(nameof(property));

            //Remove all risks
            contract.AnalyzeIntegrityOf(property).ResolveDeleteRisks();

            //Remove this
            var entity = contract.DataModel.GetEntityOf(property);
            //entity.ReferenceProperties.Remove(property);
            entity.RemoveProperty(property);
        }

        public static ContractIntegrityAnalysisResult AnalyzeIntegrityOf(this EditorContract contract, ReferenceContractProperty property)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (property == null)
                throw new ArgumentNullException(nameof(property));

            var deleteRisks = contract.AnalyzeDeleteRisksOf(property);
            return new ContractIntegrityAnalysisResult(deleteRisks);
        }
    }
}
