using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System;

namespace DasContract.Blockchain.Plutus.Test
{
    [TestClass]
    public class MortgageUseCaseTest
    {
        [TestMethod]
        public void MortgageSmartContractGeneration ()
        {
            var contract = new Contract() { Id = Guid.NewGuid(), Name = "DasContract Mortgage", Description = "Mortgage model" };
            AddCompositeActorRoles(contract);
            AddElementaryActorRoles(contract);
            AddTransactionKinds(contract);

            var dataModel = new DataModel() { Id = Guid.NewGuid(), Name = "Mortgage OFD" };
            AddEntityTypes(dataModel);
            AddAttributeTypes(dataModel);
            AddConnections(dataModel);
            contract.DataModels.Add(dataModel);

            AddActionRules(contract);
            AddProcesses(contract);

            var generator = new PlutusSmartContractGenerator();

            //Choose a smart contract name.
            var model = new SmartContractModel() { Name = "..." };

            //Choose a path to store the smart contract.
            generator.Generate(model, @"...");
        }

        public Contract AddCompositeActorRoles ( Contract contract )
        {
            contract.ActorRoles.Add(new ActorRole()
            {
                Id = Guid.NewGuid(),
                Name = "Mortgage starter",
                IdentificationNumber = "CA-0",
                Type = ActorRoleType.Composite
            });
            contract.ActorRoles.Add(new ActorRole()
            {
                Id = Guid.NewGuid(),
                Name = "Ownership transferer",
                IdentificationNumber = "CA-1",
                Type = ActorRoleType.Composite
            });
            contract.ActorRoles.Add(new ActorRole()
            {
                Id = Guid.NewGuid(),
                Name = "Insurer",
                IdentificationNumber = "CA-2",
                Type = ActorRoleType.Composite
            });
            contract.ActorRoles.Add(new ActorRole()
            {
                Id = Guid.NewGuid(),
                Name = "Mortgage payer",
                IdentificationNumber = "CA-3",
                Type = ActorRoleType.Composite
            });
            contract.ActorRoles.Add(new ActorRole()
            {
                Id = Guid.NewGuid(),
                Name = "Lien releaser",
                IdentificationNumber = "CA-4",
                Type = ActorRoleType.Composite
            });
            return contract;
        }

        public Contract AddElementaryActorRoles ( Contract contract )
        {
            contract.ActorRoles.Add(new ActorRole()
            {
                Id = Guid.NewGuid(),
                Name = "Mortgage completer",
                IdentificationNumber = "A-1",
                Type = ActorRoleType.Elementary
            });
            contract.ActorRoles.Add(new ActorRole()
            {
                Id = Guid.NewGuid(),
                Name = "Money releaser",
                IdentificationNumber = "A-2",
                Type = ActorRoleType.Elementary
            });
            contract.ActorRoles.Add(new ActorRole()
            {
                Id = Guid.NewGuid(),
                Name = "Mortgage paying off controler",
                IdentificationNumber = "A-3",
                Type = ActorRoleType.Elementary
            });
            return contract;
        }

        public Contract AddTransactionKinds ( Contract contract )
        {
            contract.TransactionKinds.Add(new TransactionKind()
            {
                Id = Guid.NewGuid(),
                Name = "Mortgage completion",
                TransactionSort = TransactionSort.Informational,
                Executor = contract.ActorRoles.First(s => s.IdentificationNumber.Contains("CA-0")).Id,
                IdentificationNumber = "T-1"
            });
            contract.TransactionKinds.Add(new TransactionKind()
            {
                Id = Guid.NewGuid(),
                Name = "Property insurance",
                TransactionSort = TransactionSort.Informational,
                Executor = contract.ActorRoles.First(s => s.IdentificationNumber.Contains("A-1")).Id,
                IdentificationNumber = "T-2"
            });
            contract.TransactionKinds.Add(new TransactionKind()
            {
                Id = Guid.NewGuid(),
                Name = "Property ownership transfer",
                TransactionSort = TransactionSort.Informational,
                Executor = contract.ActorRoles.First(s => s.IdentificationNumber.Contains("A-1")).Id,
                IdentificationNumber = "T-3"
            });
            contract.TransactionKinds.Add(new TransactionKind()
            {
                Id = Guid.NewGuid(),
                Name = "Property payment",
                TransactionSort = TransactionSort.Informational,
                Executor = contract.ActorRoles.First(s => s.IdentificationNumber.Contains("CA-1")).Id,
                IdentificationNumber = "T-4"
            });
            contract.TransactionKinds.Add(new TransactionKind()
            {
                Id = Guid.NewGuid(),
                Name = "Mortgage paying off",
                TransactionSort = TransactionSort.Informational,
                Executor = contract.ActorRoles.First(s => s.IdentificationNumber.Contains("A-1")).Id,
                IdentificationNumber = "T-5"
            });
            contract.TransactionKinds.Add(new TransactionKind()
            {
                Id = Guid.NewGuid(),
                Name = "Mortgage payment",
                TransactionSort = TransactionSort.Informational,
                Executor = contract.ActorRoles.First(s => s.IdentificationNumber.Contains("A-3")).Id,
                IdentificationNumber = "T-6"
            });
            contract.TransactionKinds.Add(new TransactionKind()
            {
                Id = Guid.NewGuid(),
                Name = "Property lien release",
                TransactionSort = TransactionSort.Informational,
                Executor = contract.ActorRoles.First(s => s.IdentificationNumber.Contains("A-1")).Id,
                IdentificationNumber = "T-7"
            });
            return contract;
        }

        public DataModel AddEntityTypes ( DataModel dataModel )
        {
            dataModel.EntityTypes.Add(new EntityType()
            {
                Id = Guid.NewGuid(),
                Name = "Mortgage",
                IsExternal = false,
            });
            dataModel.EntityTypes.Add(new EntityType()
            {
                Id = Guid.NewGuid(),
                Name = "Paid Mortgage",
                IsExternal = false,
                ProductKind = new ProductKind()
                {
                    Id = Guid.NewGuid(),
                    Formulation = "the Mortgage is paid",
                    IdentificationNumber = "P6"
                }
            });
            dataModel.EntityTypes.Add(new EntityType()
            {
                Id = Guid.NewGuid(),
                Name = "Paid off Mortgage",
                IsExternal = false,
                ProductKind = new ProductKind()
                {
                    Id = Guid.NewGuid(),
                    Formulation = "the Mortgage is paid off",
                    IdentificationNumber = "P5"
                }
            });
            dataModel.EntityTypes.Add(new EntityType()
            {
                Id = Guid.NewGuid(),
                Name = "Completed Mortgage",
                IsExternal = false,
                ProductKind = new ProductKind()
                {
                    Id = Guid.NewGuid(),
                    Formulation = "the Mortgage is completed",
                    IdentificationNumber = "P1"
                }
            });

            dataModel.EntityTypes.Add(new EntityType()
            {
                Id = Guid.NewGuid(),
                Name = "Property",
                IsExternal = false,
            });
            dataModel.EntityTypes.Add(new EntityType()
            {
                Id = Guid.NewGuid(),
                Name = "Insured Property",
                IsExternal = false,
                ProductKind = new ProductKind()
                {
                    Id = Guid.NewGuid(),
                    Formulation = "the Property is insured",
                    IdentificationNumber = "P2"
                }
            });
            dataModel.EntityTypes.Add(new EntityType()
            {
                Id = Guid.NewGuid(),
                Name = "Ownership transferred Property",
                IsExternal = false,
                ProductKind = new ProductKind()
                {
                    Id = Guid.NewGuid(),
                    Formulation = "the Property ownership is transferred",
                    IdentificationNumber = "P3"
                }
            });
            dataModel.EntityTypes.Add(new EntityType()
            {
                Id = Guid.NewGuid(),
                Name = "Paid Property",
                IsExternal = false,
                ProductKind = new ProductKind()
                {
                    Id = Guid.NewGuid(),
                    Formulation = "the Property is paid",
                    IdentificationNumber = "P4"
                }
            });
            dataModel.EntityTypes.Add(new EntityType()
            {
                Id = Guid.NewGuid(),
                Name = "Lein released Property",
                IsExternal = false,
                ProductKind = new ProductKind()
                {
                    Id = Guid.NewGuid(),
                    Formulation = "the Property lien is released",
                    IdentificationNumber = "P7"
                }
            });

            dataModel.EntityTypes.Add(new EntityType()
            {
                Id = Guid.NewGuid(),
                Name = "Client",
                IsExternal = true,
            });
            dataModel.EntityTypes.Add(new EntityType()
            {
                Id = Guid.NewGuid(),
                Name = "Insurer",
                IsExternal = true,
            });
            dataModel.EntityTypes.Add(new EntityType()
            {
                Id = Guid.NewGuid(),
                Name = "Property releaser",
                IsExternal = true,
            });
            return dataModel;
        }

        public DataModel AddAttributeTypes ( DataModel dataModel )
        {
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "amount",
                ValueType = AttributeValueType.UInt,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Mortgage")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "annual percentage rate",
                ValueType = AttributeValueType.UInt,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Mortgage")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "final amount",
                ValueType = AttributeValueType.UInt,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Mortgage")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "number of payments",
                ValueType = AttributeValueType.UInt,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Mortgage")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "amount of payment",
                ValueType = AttributeValueType.UInt,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Mortgage")).Id
            });

            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "amount paid",
                ValueType = AttributeValueType.UInt,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Paid Mortgage")).Id
            });

            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "amount paid off",
                ValueType = AttributeValueType.UInt,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Paid off Mortgage")).Id
            });

            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "id",
                ValueType = AttributeValueType.UInt,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Property")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "owner",
                ValueType = AttributeValueType.Address,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Property")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "market value",
                ValueType = AttributeValueType.UInt,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Property")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "amount to pay",
                ValueType = AttributeValueType.UInt,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Property")).Id
            });

            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "new owner",
                ValueType = AttributeValueType.Address,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Ownership transferred Property")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "lein",
                ValueType = AttributeValueType.String,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Ownership transferred Property")).Id
            });

            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "amount to paid",
                ValueType = AttributeValueType.UInt,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Paid Property")).Id
            });

            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "lein",
                ValueType = AttributeValueType.String,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Lein released Property")).Id
            });

            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "personal data",
                ValueType = AttributeValueType.String,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Client")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "income",
                ValueType = AttributeValueType.UInt,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Client")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "employment",
                ValueType = AttributeValueType.String,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Client")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "marital status",
                ValueType = AttributeValueType.String,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Client")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "credit report",
                ValueType = AttributeValueType.String,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Client")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "account statements",
                ValueType = AttributeValueType.String,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Client")).Id
            });
            dataModel.AttributeTypes.Add(new AttributeType()
            {
                Id = Guid.NewGuid(),
                Name = "income certificates",
                ValueType = AttributeValueType.String,
                EntityType = dataModel.EntityTypes.First(s => s.Name.Contains("Client")).Id
            });
            return dataModel;
        }

        public DataModel AddConnections ( DataModel dataModel )
        {
            dataModel.Connections.Add(new Connection()
            {
                Id = Guid.NewGuid(),
                Name = "the reciever of Mortgage is Client",
                FromCardinality = new ConnectionCardinality() { MaximumCardinality = CardinalityOption.One, MinimumCardinality = CardinalityOption.One },
                ToCardinality = new ConnectionCardinality() { MaximumCardinality = CardinalityOption.One, MinimumCardinality = CardinalityOption.One },
                From = dataModel.EntityTypes.First(s => s.Name.Contains("Mortgage")).Id,
                To = dataModel.EntityTypes.First(s => s.Name.Contains("Client")).Id,
                Type = ConnectionType.Generalisation
            });
            dataModel.Connections.Add(new Connection()
            {
                Id = Guid.NewGuid(),
                Name = "the property of Mortgage is Property",
                FromCardinality = new ConnectionCardinality() { MaximumCardinality = CardinalityOption.One, MinimumCardinality = CardinalityOption.One },
                ToCardinality = new ConnectionCardinality() { MaximumCardinality = CardinalityOption.One, MinimumCardinality = CardinalityOption.One },
                From = dataModel.EntityTypes.First(s => s.Name.Contains("Mortgage")).Id,
                To = dataModel.EntityTypes.First(s => s.Name.Contains("Property")).Id,
                Type = ConnectionType.Generalisation
            });
            dataModel.Connections.Add(new Connection()
            {
                Id = Guid.NewGuid(),
                Name = "the insurer of Property is Insurer",
                FromCardinality = new ConnectionCardinality() { MaximumCardinality = CardinalityOption.One, MinimumCardinality = CardinalityOption.One },
                ToCardinality = new ConnectionCardinality() { MaximumCardinality = CardinalityOption.One, MinimumCardinality = CardinalityOption.One },
                From = dataModel.EntityTypes.First(s => s.Name.Contains("Insured Property")).Id,
                To = dataModel.EntityTypes.First(s => s.Name.Contains("Insurer")).Id,
                Type = ConnectionType.Generalisation
            });
            dataModel.Connections.Add(new Connection()
            {
                Id = Guid.NewGuid(),
                Name = "the lien releaser of Property is Property releaser",
                FromCardinality = new ConnectionCardinality() { MaximumCardinality = CardinalityOption.One, MinimumCardinality = CardinalityOption.One },
                ToCardinality = new ConnectionCardinality() { MaximumCardinality = CardinalityOption.One, MinimumCardinality = CardinalityOption.One },
                From = dataModel.EntityTypes.First(s => s.Name.Contains("Lein released Property")).Id,
                To = dataModel.EntityTypes.First(s => s.Name.Contains("Property releaser")).Id,
                Type = ConnectionType.Generalisation
            });
            return dataModel;
        }

        public Contract AddActionRules ( Contract contract )
        {
            contract.Rules.Add(new ActionRule()
            {
                Id = Guid.NewGuid(),
                Name = "Action Rule 1"
            });
            contract.Rules.Add(new ActionRule()
            {
                Id = Guid.NewGuid(),
                Name = "Action Rule 2"
            });
            contract.Rules.Add(new ActionRule()
            {
                Id = Guid.NewGuid(),
                Name = "Action Rule 3"
            });
            contract.Rules.Add(new ActionRule()
            {
                Id = Guid.NewGuid(),
                Name = "Action Rule 4"
            });
            contract.Rules.Add(new ActionRule()
            {
                Id = Guid.NewGuid(),
                Name = "Action Rule 5"
            });
            contract.Rules.Add(new ActionRule()
            {
                Id = Guid.NewGuid(),
                Name = "Action Rule 6"
            });
            contract.Rules.Add(new ActionRule()
            {
                Id = Guid.NewGuid(),
                Name = "Action Rule 7"
            });
            contract.Rules.Add(new ActionRule()
            {
                Id = Guid.NewGuid(),
                Name = "Action Rule 8"
            });
            contract.Rules.Add(new ActionRule()
            {
                Id = Guid.NewGuid(),
                Name = "Action Rule 9"
            });
            contract.Rules.Add(new ActionRule()
            {
                Id = Guid.NewGuid(),
                Name = "Action Rule 10"
            });
            contract.Rules.Add(new ActionRule()
            {
                Id = Guid.NewGuid(),
                Name = "Action Rule 11"
            });
            contract.Rules.Add(new ActionRule()
            {
                Id = Guid.NewGuid(),
                Name = "Action Rule 12"
            });
            return contract;
        }

        public Contract AddProcesses ( Contract contract)
        {
            contract.Processes.Add(new Process()
            {

            });
            return contract;
        }
    }
}
