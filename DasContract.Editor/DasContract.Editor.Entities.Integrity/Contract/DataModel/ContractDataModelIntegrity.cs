using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Entities.DataModels;
using DasContract.Editor.Entities.DataModels.Entities;

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

            contract.DataModel.Entities.Add(entity);
        }
    }
}
