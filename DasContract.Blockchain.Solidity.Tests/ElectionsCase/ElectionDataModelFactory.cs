using DasContract.Abstraction.Data;
using DasContract.Abstraction.Data.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.Tests.ElectionsCase
{
    public static class ElectionDataModelFactory
    {
        public static IList<DataType> CreateDataModel()
        {
            var dataTypes = new List<DataType>();
            dataTypes.AddRange(CreateContractEnums());
            dataTypes.AddRange(CreateContractTokens());
            dataTypes.AddRange(CreateContractEntities());
            return dataTypes;
        }

        private static IList<Abstraction.Data.Enum> CreateContractEnums()
        {
            var enums = new List<Abstraction.Data.Enum>();

            enums.Add(new Abstraction.Data.Enum
            {
                Values = new List<string> { "ClosedList", "OpenList", "SingleTransferable" },
                Id = "Enum_1",
                Name = "VotingSystem"
            });

            return enums;
        }

        private static IList<Entity> CreateContractEntities()
        {
            return new List<Entity>
            {
                CreateElectionsEntity(),
                CreateCountryElectionsEntity(),
                CreatePoliticalPartyEntity(),
                CreateCandidateEntity()
            };
        }

        private static Entity CreateCandidateEntity()
        {
            var entity = new Entity
            {
                Id = "Entity_4",
                Name = "Candidate"
            };

            entity.Properties = new List<Property>
            {
                new Property
                {
                    Id = "Property_18",
                    Name = "id",
                    DataType = PropertyDataType.Address,
                    PropertyType = PropertyType.Single
                },
                new Property
                {
                    Id = "Property_19",
                    Name = "name",
                    DataType = PropertyDataType.String,
                    PropertyType = PropertyType.Single
                },
                new Property
                {
                    Id = "Property_20",
                    Name = "website",
                    DataType = PropertyDataType.String,
                    PropertyType = PropertyType.Single
                },
                new Property
                {
                    Id = "Property_21",
                    Name = "voteCount",
                    DataType = PropertyDataType.Int,
                    PropertyType = PropertyType.Single
                },
                new Property
                {
                    Id = "Property_22",
                    Name = "hasSeat",
                    DataType = PropertyDataType.Bool,
                    PropertyType = PropertyType.Single
                }
            };
            return entity;
        }

        private static Entity CreatePoliticalPartyEntity()
        {
            var entity = new Entity
            {
                Id = "Entity_3",
                Name = "PoliticalParty"
            };

            entity.Properties = new List<Property>
            {
                new Property
                {
                    Id = "Property_12",
                    Name = "id",
                    DataType = PropertyDataType.Address,
                    PropertyType = PropertyType.Single
                },
                new Property
                {
                    Id = "Property_13",
                    Name = "name",
                    DataType = PropertyDataType.String,
                    PropertyType = PropertyType.Single
                },
                new Property
                {
                    Id = "Property_14",
                    Name = "code",
                    DataType = PropertyDataType.String,
                    PropertyType = PropertyType.Single
                },
                new Property
                {
                    Id = "Property_15",
                    Name = "website",
                    DataType = PropertyDataType.String,
                    PropertyType = PropertyType.Single
                },
                new Property
                {
                    Id = "Property_16",
                    Name = "voteCount",
                    DataType = PropertyDataType.Int,
                    PropertyType = PropertyType.Single
                },
                new Property
                {
                    Id = "Property_17",
                    Name = "allocatedSeats",
                    DataType = PropertyDataType.Int,
                    PropertyType = PropertyType.Single
                }
            };
            return entity;
        }

        private static Entity CreateCountryElectionsEntity()
        {
            var entity = new Entity
            {
                Id = "Entity_2",
                Name = "CountryElections"
            };

            entity.Properties = new List<Property>
            {
                new Property
                {
                    Id = "Property_6",
                    Name = "countryName",
                    DataType = PropertyDataType.String,
                    PropertyType = PropertyType.Single
                },
                new Property
                {
                    Id = "Property_7",
                    Name = "electionDates",
                    DataType = PropertyDataType.DateTime,
                    PropertyType = PropertyType.Collection
                },
                new Property
                {
                    Id = "Property_8",
                    Name = "votingSystem",
                    DataType = PropertyDataType.Reference,
                    PropertyType = PropertyType.Single,
                    ReferencedDataType = "Enum_1"
                },
                new Property
                {
                    Id = "Property_9",
                    Name = "availableSeats",
                    DataType = PropertyDataType.Int,
                    PropertyType = PropertyType.Single
                },
                new Property
                {
                    Id = "Property_10",
                    Name = "electoralTreshold",
                    DataType = PropertyDataType.Byte,
                    PropertyType = PropertyType.Single
                },
                new Property
                {
                    Id = "Property_11",
                    Name = "minimumAge",
                    DataType = PropertyDataType.Byte,
                    PropertyType = PropertyType.Single
                }
            };

            return entity;
        }

        private static Entity CreateElectionsEntity()
        {
            var entity = new Entity
            {
                Id = "Entity_1",
                Name = "Elections"
            };

            entity.Properties = new List<Property>
            {
            new Property
            {
                DataType = PropertyDataType.DateTime,
                Id = "Property_2",
                Name = "startDate",
                PropertyType = PropertyType.Single
            },
            new Property
            {
                DataType = PropertyDataType.DateTime,
                Id = "Property_3",
                Name = "partyRegistrationEnd",
                PropertyType = PropertyType.Single
            },
            new Property
            {
                DataType = PropertyDataType.DateTime,
                Id = "Property_4",
                Name = "candidateRegistrationEnd",
                PropertyType = PropertyType.Single
            },
            new Property
            {
                DataType = PropertyDataType.DateTime,
                Id = "Property_5",
                Name = "candidateApprovalEnd",
                PropertyType = PropertyType.Single
            }
        };

            return entity;
        }

        private static IList<Token> CreateContractTokens()
        {
            var tokens = new List<Token>();

            var token = new Token
            {
                Id = "Token_1",
                IsFungible = false,
                Name = "VotingToken",
                Symbol = "VOTE",
            };

            var tokenIdCounterProperty = new Property
            {
                DataType = PropertyDataType.Uint,
                Id = "Property_1",
                Name = "tokenIdCounter",
                PropertyType = PropertyType.Single
            };

            token.Properties.Add(tokenIdCounterProperty);

            tokens.Add(token);

            return tokens;
        }
    }
}
