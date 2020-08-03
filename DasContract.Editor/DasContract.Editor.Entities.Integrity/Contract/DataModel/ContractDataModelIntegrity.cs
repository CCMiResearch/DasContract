using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Entities.DataModels;
using DasContract.Editor.Entities.DataModels.Entities;
using DasContract.Editor.Entities.Integrity.Analysis;
using DasContract.Editor.Entities.Integrity.Analysis.Cases;
using DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities.Properties.Primitive;
using DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities.Properties.Reference;
using DasContract.Editor.Entities.Integrity.Contract.Processes.Process.Activities;

namespace DasContract.Editor.Entities.Integrity.Contract.DataModel
{
    public static class ContractDataModelIntegrity
    {
        public static void AddSafely(this EditorContract contract, ContractEntity entity)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            contract.DataModel.AddEntity(entity);
            //contract.DataModel.Entities.Add(entity);
        }

        public static void RemoveSafely(this EditorContract contract, ContractEntity entity)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            //Remove all risks
            contract.AnalyzeIntegrityOf(entity).ResolveDeleteRisks();

            //Remove this
            //contract.DataModel.Entities.Remove(entity);
            contract.DataModel.RemoveEntity(entity);
        }

        public static ContractIntegrityAnalysisResult AnalyzeIntegrityOf(this EditorContract contract, ContractEntity entity)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var deleteRisks = new List<ContractIntegrityAnalysisDeleteCase>();
            var childrenAnalyses = new List<ContractIntegrityAnalysisResult>();

            //Search dependent reference properties
            foreach (var contractEntity in contract.DataModel.Entities)
            {
                if (contractEntity == entity)
                    continue;

                foreach (var property in contractEntity.ReferenceProperties)
                {
                    var currentEntity = contractEntity;
                    var currentProperty = property;

                    if (property.Entity == entity)
                    {
                        currentEntity.WithMigrator(contract.GetMigrator());
                        deleteRisks.Add(
                            new ContractIntegrityAnalysisDeleteCase(
                                $"Property {currentEntity.Name}.{currentProperty.Name} will be deleted",
                                () => { currentEntity.RemoveProperty(currentProperty); })
                                );

                        childrenAnalyses.Add(contract.AnalyzeIntegrityOf(currentProperty));
                    }
                }
            }

            //Analyze all child properties
            foreach (var property in entity.PrimitiveProperties)
                childrenAnalyses.Add(contract.AnalyzeIntegrityOf(property));
            foreach (var property in entity.ReferenceProperties)
                if (property.Entity != null)
                    childrenAnalyses.Add(contract.AnalyzeIntegrityOf(property));

            return new ContractIntegrityAnalysisResult(deleteRisks, childrenAnalyses);
        }
    }
}
