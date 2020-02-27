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
using DasContract.Editor.Entities.Integrity.Extensions;
using DasContract.Editor.Entities.Processes;
using DasContract.Editor.Entities.Processes.Process;
using DasContract.Editor.Entities.Processes.Process.Activities;
using NUnit.Framework;

namespace DasContract.Editor.Tests.Entities.Integrity
{
    public class ContractEntityPropertyIntegrityTests
    {
        PrimitiveContractProperty property1;
        ReferenceContractProperty property2;
        CollectionReferenceContractProperty property3;
        PrimitiveContractProperty property4;

        ContractEntity entity1;
        ContractEntity entity2;
        ContractEntity entity3;

        EditorContract contract;

        [SetUp]
        public void Setup()
        {
            property1 = new PrimitiveContractProperty();
            property4 = new PrimitiveContractProperty();
            entity1 = new ContractEntity()
            {
                PrimitiveProperties = new List<PrimitiveContractProperty>()
                {
                    property1,
                    property4
                }
            };

            property2 = new ReferenceContractProperty()
            {
                Entity = entity1
            };
            property3 = new CollectionReferenceContractProperty()
            {
                Entities = new ObservableCollection<ContractEntity>()
                {
                    entity1
                }
            };

            entity2 = new ContractEntity()
            {
                ReferenceProperties = new List<ReferenceContractProperty>()
                {
                    property2
                }
            };

            entity3 = new ContractEntity()
            {
                CollectionReferenceProperties = new List<CollectionReferenceContractProperty>()
                {
                    property3
                }
            };

            contract = new EditorContract()
            {
                DataModel = new ContractDataModel()
                {
                    Entities = new List<ContractEntity>()
                    {
                        entity1,
                        entity2,
                        entity3
                    },
                },
                Processes = new ContractProcesses()
                {
                    Main = new ContractProcess()
                    {
                        ProcessElements = new List<ContractProcessElement>()
                          {
                              new ContractUserActivity()
                              {
                                   Form = new ContractForm()
                                   {
                                        Fields = new List<ContractFormField>()
                                        {
                                             new ContractFormField()
                                             {
                                                  PropertyBinding = new ContractPropertyBinding()
                                                  {
                                                     Property = property4
                                                  }
                                             }
                                        }
                                   }
                              }
                          }
                    }
                }

            };
        }

        [Test]
        public void GetEntityOf()
        {
            Assert.AreEqual(entity1, contract.DataModel.GetEntityOf(property1));
            Assert.AreEqual(entity2, contract.DataModel.GetEntityOf(property2));
            Assert.AreEqual(entity3, contract.DataModel.GetEntityOf(property3));
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

        
    }
}
