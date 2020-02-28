using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Primitive;
using DasContract.Editor.Entities.Integrity.Extensions;
using NUnit.Framework;

namespace DasContract.Editor.Tests.Entities.Integrity.Extensions
{
    public class ContractDataModelExtensionsTests: BaseTest
    {
        [Test]
        public void GetEntityOf()
        {
            Assert.AreEqual(entity1, contract.DataModel.GetEntityOf(property1));
            Assert.AreEqual(entity2, contract.DataModel.GetEntityOf(property2));
        }

        [Test]
        public void GetEntityOfNotFound()
        {
            try
            {
                contract.DataModel.GetEntityOf(new PrimitiveContractProperty());
                Assert.Fail();
            }
            catch (Exception) { }
        }
    }
}
