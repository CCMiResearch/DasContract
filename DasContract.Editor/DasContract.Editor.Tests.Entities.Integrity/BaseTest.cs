using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using DasContract.Editor.Entities;
using DasContract.Editor.Entities.DataModels;
using DasContract.Editor.Entities.DataModels.Entities;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Primitive;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Reference;
using DasContract.Editor.Entities.Forms;
using DasContract.Editor.Entities.Processes;
using DasContract.Editor.Entities.Processes.Process;
using DasContract.Editor.Entities.Processes.Process.Activities;
using NUnit.Framework;

namespace DasContract.Editor.Tests.Entities.Integrity
{
    public class BaseTest
    {
        protected PrimitiveContractProperty property1;
        protected ReferenceContractProperty property2;
        protected CollectionReferenceContractProperty property3;
        protected PrimitiveContractProperty property4;

        protected ContractEntity entity1;
        protected ContractEntity entity2;
        protected ContractEntity entity3;

        protected EditorContract contract;

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
    }
}
