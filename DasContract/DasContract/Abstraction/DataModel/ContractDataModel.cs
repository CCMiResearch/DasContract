using DasContract.Abstraction.DataModel.Entity;
using DasContract.Abstraction.DataModel.Property.Primitive;
using DasContract.DasContract.Abstraction.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.DataModel
{
    public class ContractDataModel: IIdentifiable
    {
        public string Id { get; set; }

        /// <summary>
        /// Entities of this data model
        /// </summary>
        public IList<ContractEntity> Entities { get; set; } = new List<ContractEntity>();
    }
}
