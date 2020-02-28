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
        protected PrimitiveContractProperty property4;
        protected ReferenceContractProperty property5;
        protected ReferenceContractProperty property6;
        protected ReferenceContractProperty property7;

        protected ContractEntity entity1;
        protected ContractEntity entity2;
        protected ContractEntity entity3;
        protected ContractEntity entity4;
        protected ContractEntity entity5;

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
            property5 = new ReferenceContractProperty()
            {
                Entity = entity1
            };

            entity2 = new ContractEntity()
            {
                ReferenceProperties = new List<ReferenceContractProperty>()
                {
                    property2,
                    property5
                }
            };

            entity3 = new ContractEntity()
            {
                
            };


            property6 = new ReferenceContractProperty();
            property7 = new ReferenceContractProperty();
            entity4 = new ContractEntity()
            {
                ReferenceProperties = new List<ReferenceContractProperty>()
                {
                    property6
                }
            };
            entity5 = new ContractEntity()
            {
                ReferenceProperties = new List<ReferenceContractProperty>()
                {
                    property7
                }
            };
            property6.Entity = entity5;
            property7.Entity = entity4;

            contract = new EditorContract()
            {
                DataModel = new ContractDataModel()
                {
                    Entities = new List<ContractEntity>()
                    {
                        entity1,
                        entity2,
                        entity3,
                        entity4,
                        entity5
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
                                             },
                                              new ContractFormField()
                                             {
                                                  PropertyBinding = new ContractPropertyBinding()
                                                  {
                                                     Property = property5
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
