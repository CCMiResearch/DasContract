using DasContract.Abstraction.Data;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DasContract.Blockchain.Solidity.Tests.SolidityComponents
{
    public class SolidityMappingTest
    {
        [Fact]
        public void SimpleMappingTest()
        {
            var mapping = new SolidityMappingStatement("string", "bool", "mapper");
            var actual = mapping.ToString();

            var expected = "mapping(string => bool) mapper";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NestedMappingTest()
        {
            var nestedProperties = new List<Property>
            {
                new Property
                {
                    DataType = PropertyDataType.String
                },
                new Property
                {
                    DataType = PropertyDataType.Int
                }
            };

            var mapping = new SolidityMappingStatement("string", "bool", "mapper", nestedProperties);
            var actual = mapping.ToString();

            var expected = "mapping(string => mapping(int => mapping(string => bool))) mapper";

            Assert.Equal(expected, actual);
        }
    }
}
