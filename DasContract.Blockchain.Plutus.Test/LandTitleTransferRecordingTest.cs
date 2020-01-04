using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DasContract.Blockchain.Plutus.Test
{
    [TestClass]
    public class LandTitleTransferRecordingTest
    {
        [TestMethod]
        public void LandTitleTransferRecordingSmartContract()
        {
            var contract = new Contract() { Id = Guid.NewGuid(), Name = "Land Title Transfer Recording", Description = "Use-Case introduced in the bachelor thesis." };
            LoadDEMOContract(contract);

            var model = new SmartContractModel();
            model.CreateStructureFromContract(contract);

            var generator = new PlutusSmartContractFileGenerator();
            generator.Generate(model, @"\");
        }

        public Contract LoadDEMOContract(Contract contract)
        {
            AddActorRoles(contract);
            AddTransactionKinds(contract);

            var dataModel = new DataModel() { Id = Guid.NewGuid(), Name = "Land Title Trasfer Recording OFD" };
            AddEntityTypes(dataModel);
            AddAttributeTypes(dataModel);
            contract.DataModels.Add(dataModel);

            AddProcesses(contract);

            return contract;
        }

        public Contract AddActorRoles(Contract contract)
        {
            contract.ActorRoles.Add(new ActorRole()
            {
                Id = Guid.NewGuid(),
                Name = "Land Title Transfer Sender",
                IdentificationNumber = "CA-1",
                Type = ActorRoleType.Composite
            });
            contract.ActorRoles.Add(new ActorRole()
            {
                Id = Guid.NewGuid(),
                Name = "Payment Sender",
                IdentificationNumber = "CA-2",
                Type = ActorRoleType.Composite
            });
            contract.ActorRoles.Add(new ActorRole()
            {
                Id = Guid.NewGuid(),
                Name = "Payment Collector",
                IdentificationNumber = "CA-3",
                Type = ActorRoleType.Composite
            });
            contract.ActorRoles.Add(new ActorRole()
            {
                Id = Guid.NewGuid(),
                Name = "Land Title Transfer Collector",
                IdentificationNumber = "CA-4",
                Type = ActorRoleType.Composite
            });
            contract.ActorRoles.Add(new ActorRole()
            {
                Id = Guid.NewGuid(),
                Name = "Land Title Transfer Returner",
                IdentificationNumber = "CA-5",
                Type = ActorRoleType.Composite
            });
            contract.ActorRoles.Add(new ActorRole()
            {
                Id = Guid.NewGuid(),
                Name = "Payment Returner",
                IdentificationNumber = "CA-6",
                Type = ActorRoleType.Composite
            });
            contract.ActorRoles.Add(new ActorRole()
            {
                Id = Guid.NewGuid(),
                Name = "Land Title Transfer Completer",
                IdentificationNumber = "A-1",
                Type = ActorRoleType.Elementary
            });
            return contract;
        }

        public Contract AddTransactionKinds(Contract contract)
        {
            contract.TransactionKinds.Add(new TransactionKind()
            {
                Id = Guid.NewGuid(),
                Name = "Land Title Transfer Completion",
                TransactionSort = TransactionSort.Original,
                Executor = contract.ActorRoles.FirstOrDefault(s => Equals(s.IdentificationNumber, "A-1")).Id,
                IdentificationNumber = "T-1"
            });
            contract.TransactionKinds.Add(new TransactionKind()
            {
                Id = Guid.NewGuid(),
                Name = "Land Title Transfer Sending",
                TransactionSort = TransactionSort.Original,
                Executor = contract.ActorRoles.FirstOrDefault(s => Equals(s.IdentificationNumber, "CA-1")).Id,
                IdentificationNumber = "T-2"
            });
            contract.TransactionKinds.Add(new TransactionKind()
            {
                Id = Guid.NewGuid(),
                Name = "Payment Sending",
                TransactionSort = TransactionSort.Original,
                Executor = contract.ActorRoles.FirstOrDefault(s => Equals(s.IdentificationNumber, "CA-2")).Id,
                IdentificationNumber = "T-3"
            });
            contract.TransactionKinds.Add(new TransactionKind()
            {
                Id = Guid.NewGuid(),
                Name = "Payment Collection",
                TransactionSort = TransactionSort.Original,
                Executor = contract.ActorRoles.FirstOrDefault(s => Equals(s.IdentificationNumber, "CA-3")).Id,
                IdentificationNumber = "T-4"
            });
            contract.TransactionKinds.Add(new TransactionKind()
            {
                Id = Guid.NewGuid(),
                Name = "Land Title Transfer Collection",
                TransactionSort = TransactionSort.Original,
                Executor = contract.ActorRoles.FirstOrDefault(s => Equals(s.IdentificationNumber, "CA-4")).Id,
                IdentificationNumber = "T-5"
            });
            contract.TransactionKinds.Add(new TransactionKind()
            {
                Id = Guid.NewGuid(),
                Name = "Land Title Transfer Returning",
                TransactionSort = TransactionSort.Original,
                Executor = contract.ActorRoles.FirstOrDefault(s => Equals(s.IdentificationNumber, "CA-5")).Id,
                IdentificationNumber = "T-6"
            });
            contract.TransactionKinds.Add(new TransactionKind()
            {
                Id = Guid.NewGuid(),
                Name = "Payment Returning",
                TransactionSort = TransactionSort.Original,
                Executor = contract.ActorRoles.FirstOrDefault(s => Equals(s.IdentificationNumber, "CA-6")).Id,
                IdentificationNumber = "T-7"
            });
            return contract;
        }

        public DataModel AddEntityTypes(DataModel dataModel)
        {
            dataModel.EntityTypes.Add(new EntityType()
            {
                Id = Guid.NewGuid(),
                Name = "Land Title Transfer",
                IsExternal = false,
            });
            
            dataModel.EntityTypes.Add(new EntityType()
            {
                Id = Guid.NewGuid(),
                Name = "Payment",
                IsExternal = false,
            });
            return dataModel;
        }

        public DataModel AddAttributeTypes(DataModel dataModel)
        {
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "land title",
                ValueType = AttributeValueType.ByteString,
                EntityType = dataModel.EntityTypes.FirstOrDefault(s => s.Name.Contains("Land Title Transfer")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "price",
                ValueType = AttributeValueType.Integer,
                EntityType = dataModel.EntityTypes.FirstOrDefault(s => s.Name.Contains("Land Title Transfer")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "current owner",
                ValueType = AttributeValueType.PubKey,
                EntityType = dataModel.EntityTypes.FirstOrDefault(s => s.Name.Contains("Land Title Transfer")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "new owner",
                ValueType = AttributeValueType.PubKey,
                EntityType = dataModel.EntityTypes.FirstOrDefault(s => s.Name.Contains("Land Title Transfer")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "amount paid",
                ValueType = AttributeValueType.Integer,
                EntityType = dataModel.EntityTypes.FirstOrDefault(s => s.Name.Contains("Payment")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "payer",
                ValueType = AttributeValueType.PubKey,
                EntityType = dataModel.EntityTypes.FirstOrDefault(s => s.Name.Contains("Payment")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "receiver",
                ValueType = AttributeValueType.PubKey,
                EntityType = dataModel.EntityTypes.FirstOrDefault(s => s.Name.Contains("Payment")).Id
            });
            return dataModel;
        }

        public Contract AddProcesses(Contract contract)
        {
            var t1id = Guid.NewGuid();
            var t2id = Guid.NewGuid();
            var t3id = Guid.NewGuid();
            var t4id = Guid.NewGuid();
            var t5id = Guid.NewGuid();
            var t6id = Guid.NewGuid();
            var t7id = Guid.NewGuid();
            var t1ils = new List<InspectionLink>();
            t1ils.Add(new InspectionLink() { Id = Guid.NewGuid(), InspectionTargetTransactor = t2id });
            t1ils.Add(new InspectionLink() { Id = Guid.NewGuid(), InspectionTargetTransactor = t3id });
            t1ils.Add(new InspectionLink() { Id = Guid.NewGuid(), InspectionTargetTransactor = t4id });
            t1ils.Add(new InspectionLink() { Id = Guid.NewGuid(), InspectionTargetTransactor = t5id });
            t1ils.Add(new InspectionLink() { Id = Guid.NewGuid(), InspectionTargetTransactor = t6id });
            t1ils.Add(new InspectionLink() { Id = Guid.NewGuid(), InspectionTargetTransactor = t7id });
            var t1wls = new List<WaitLink>();
            t1wls.Add(new WaitLink() { Id = Guid.NewGuid(), WaitingForTransactor = t3id });
            contract.Processes.Add(new Process()
            {
                Id = Guid.NewGuid(),
                Name = "Land Title Transfer Completion",
                Root = new SelfActivatingTransactor()
                {
                    Id = t1id,
                    ActorRole = contract.ActorRoles.FirstOrDefault(s => Equals(s.IdentificationNumber, "CA-1")).Id,
                    InspectionLinks = t1ils,
                    WaitLinks = t1wls,
                    TransactionKind = contract.TransactionKinds.FirstOrDefault(s => Equals(s.IdentificationNumber, "T-1")).Id,
                },
            });
            var t2ils = new List<InspectionLink>();
            t2ils.Add(new InspectionLink() { Id = Guid.NewGuid(), InspectionTargetTransactor = t1id });
            contract.Processes.Add(new Process()
            {
                Id = Guid.NewGuid(),
                Name = "Land Title Transfer Sending",
                Root = new CompositeTransactor()
                {
                    Id = t2id,
                    ActorRole = contract.ActorRoles.FirstOrDefault(s => Equals(s.IdentificationNumber, "A-1")).Id,
                    InspectionLinks = t2ils
                },
            });
            contract.Processes.Add(new Process()
            {
                Id = Guid.NewGuid(),
                Name = "Payment Sending",
                Root = new CompositeTransactor()
                {
                    Id = t3id,
                    ActorRole = contract.ActorRoles.FirstOrDefault(s => Equals(s.IdentificationNumber, "A-1")).Id,
                },
            });
            contract.Processes.Add(new Process()
            {
                Id = Guid.NewGuid(),
                Name = "Payment Collection",
                Root = new CompositeTransactor()
                {
                    Id = t4id,
                    ActorRole = contract.ActorRoles.FirstOrDefault(s => Equals(s.IdentificationNumber, "A-1")).Id,
                },
            });
            contract.Processes.Add(new Process()
            {
                Id = Guid.NewGuid(),
                Name = "Land Title Transfer Collection",
                Root = new CompositeTransactor()
                {
                    Id = t5id,
                    ActorRole = contract.ActorRoles.FirstOrDefault(s => Equals(s.IdentificationNumber, "A-1")).Id,
                },
            });
            contract.Processes.Add(new Process()
            {
                Id = Guid.NewGuid(),
                Name = "Land Title Transfer Returning",
                Root = new CompositeTransactor()
                {
                    Id = t6id,
                    ActorRole = contract.ActorRoles.FirstOrDefault(s => Equals(s.IdentificationNumber, "A-1")).Id,
                },
            });
            contract.Processes.Add(new Process()
            {
                Id = Guid.NewGuid(),
                Name = "Payment Returning",
                Root = new CompositeTransactor()
                {
                    Id = t7id,
                    ActorRole = contract.ActorRoles.FirstOrDefault(s => Equals(s.IdentificationNumber, "A-1")).Id,
                },
            });
            return contract;
        }
    }
}
