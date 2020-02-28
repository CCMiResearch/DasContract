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
    public class ContractEntityIntegrityTests : BaseTest
    {

        [Test]
        public void EntityAnalysis()
        {
            var analysis = contract.AnalyzeIntegrityOf(entity4);
            Assert.AreEqual(2, analysis.ChildrenAnalyses.Count);
            Assert.AreEqual(1, analysis.DeleteRisks.Count);
            
            contract.RemoveSafely(entity4);
            Assert.AreEqual(0, contract.AnalyzeIntegrityOf(entity4).DeleteRisks.Count);
            Assert.AreEqual(0, contract.DataModel.Entities.Where(e => e == entity4).Count());
            Assert.AreEqual(0, contract.DataModel.Entities.Where(e => e == entity5).Single().ReferenceProperties.Count());
        }

        [Test]
        public void EntityAnalysis_NoRisks()
        {
            var analysis = contract.AnalyzeIntegrityOf(entity2);
            Assert.AreEqual(0, analysis.DeleteRisks.Count);
            Assert.AreEqual(2, analysis.ChildrenAnalyses.Count);
        }
    }
}
