using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Entities.DataModels;
using DasContract.Editor.Entities.DataModels.Entities;
using DasContract.Editor.Entities.DataModels.Entities.Properties;

namespace DasContract.Editor.Entities.Integrity.Extensions
{
    public static class ContractDataModelExtensions
    {
        /// <summary>
        /// Returns entity that owns a property
        /// </summary>
        /// <param name="dataModel">The contract</param>
        /// <param name="property">The property</param>
        /// <returns></returns>
        public static ContractEntity GetEntityOf(this ContractDataModel dataModel, ContractProperty property)
        {
            if (dataModel == null)
                throw new ArgumentNullException(nameof(dataModel));

            if (property == null)
                throw new ArgumentNullException(nameof(property));

            foreach (var entity in dataModel.Entities)
            {
                foreach (var entityProperty in entity.PrimitiveProperties)
                    if (entityProperty == property)
                        return entity;
                foreach (var entityProperty in entity.ReferenceProperties)
                    if (entityProperty == property)
                        return entity;
            }

            throw new ArgumentException("No suitable entity found");
        }
    }
}
