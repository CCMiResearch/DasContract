using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DasContract.Editor.Entities;
using DasContract.Editor.Entities.DataModels;
using DasContract.Editor.Entities.DataModels.Entities;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Primitive;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Reference;
using DasContract.Editor.Entities.Forms;
using DasContract.Editor.Entities.Integrity.DataModel.Properties;
using DasContract.Editor.Entities.Integrity.DataModel.Properties.Primitive;
using DasContract.Editor.Entities.Integrity.DataModel.Properties.Reference;
using DasContract.Editor.Entities.Integrity.Extensions;
using DasContract.Editor.Entities.Processes;
using DasContract.Editor.Entities.Processes.Process;
using DasContract.Editor.Entities.Processes.Process.Activities;
using NUnit.Framework;

namespace DasContract.Editor.Tests.Entities.Integrity
{
    public class ContractEntityPropertyIntegrityTests : BaseTest
    {

        [Test]
        public void PrimitivePropertyAnalysis()
        {
            Assert.AreEqual(1, contract.AnalyzeIntegrityOf(property4).DeleteRisks.Count);
            
            contract.RemoveSafely(property4);
            Assert.AreEqual(0, contract.AnalyzeIntegrityOf(property4).DeleteRisks.Count);
            Assert.IsNull(contract.Processes.Main.UserActivities.First().Form.Fields[0].PropertyBinding);
            Assert.IsTrue(contract.DataModel.Entities[0].PrimitiveProperties.Where(e => e == property4).Count() == 0);
        }

        [Test]
        public void PrimitivePropertyAnalysis_NoRisks()
        {
            var analysis = contract.AnalyzeIntegrityOf(property1);
            Assert.AreEqual(0, analysis.DeleteRisks.Count);
        }


        [Test]
        public void ReferencePropertyAnalysis()
        {
            Assert.AreEqual(1, contract.AnalyzeIntegrityOf(property5).DeleteRisks.Count);

            contract.RemoveSafely(property5);
            Assert.AreEqual(0, contract.AnalyzeIntegrityOf(property5).DeleteRisks.Count);
            Assert.IsNull(contract.Processes.Main.UserActivities.First().Form.Fields[1].PropertyBinding);
            Assert.IsTrue(contract.DataModel.Entities[1].ReferenceProperties.Where(e => e == property5).Count() == 0);
        }

        [Test]
        public void ReferencePropertyAnalysis_NoRisks()
        {
            var analysis = contract.AnalyzeIntegrityOf(property2);
            Assert.AreEqual(0, analysis.DeleteRisks.Count);
        }

    }
}
