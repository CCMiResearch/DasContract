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

namespace DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities.Properties.Primitive
{
    public static class PrimitiveContractPropertyIntegrity
    {
        //--------------------------------------------------
        //               PRIMITIVE PROPERTY
        //--------------------------------------------------
        public static void AddSafely(this EditorContract contract, ContractEntity entity, PrimitiveContractProperty property)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (property == null)
                throw new ArgumentNullException(nameof(property));

            //entity.PrimitiveProperties.Add(property);
            entity.AddProperty(property);
        }

        /// <summary>
        /// Safely removes a promitive contract property
        /// </summary>
        /// <param name="contract">A contract that holds the property</param>
        /// <param name="property">The property</param>
        public static void RemoveSafely(this EditorContract contract, PrimitiveContractProperty property)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (property == null)
                throw new ArgumentNullException(nameof(property));

            //Remove all risks
            contract.AnalyzeIntegrityOf(property).ResolveDeleteRisks();

            //Remove this
            var entity = contract.DataModel.GetEntityOf(property);
            //entity.PrimitiveProperties.Remove(property);
            entity.RemoveProperty(property);
        }

        public static ContractIntegrityAnalysisResult AnalyzeIntegrityOf(this EditorContract contract, PrimitiveContractProperty property)
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
