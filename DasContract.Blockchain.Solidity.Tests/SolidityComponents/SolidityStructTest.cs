﻿using DasContract.Abstraction.Data;
using DasContract.Blockchain.Solidity.SolidityComponents;
using Xunit;

namespace DasContractTests.DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityStructTest
    {
        string structName = "foo";
        string structId = "1";
        string propertyName = "bar";
        string propertyId = "2";
        PropertyDataType propertyType = PropertyDataType.String;

        private SolidityStruct GetSampleStruct()
        {
            Entity entity = new Entity();
            Property property = new Property();

            property.Id = propertyId;
            property.Name = propertyName;
            property.DataType = propertyType;

            entity.Id = structId;
            entity.Name = structName;
            entity.Properties.Add(property);

            var s = new SolidityStruct(entity.Name);
            s.AddToBody(new SolidityStatement(propertyType.ToString().ToLower() + " " + propertyName));
            return s;
        }

        [Fact]
        public void ToStringTest()
        {
            SolidityStruct st = GetSampleStruct();
            string expected = "struct Foo{\n" +
                "\tstring bar;\n" +
                 "}\n"; //+
                 //"Foo foo = Foo({bar: \"\"});\n";
            Assert.Equal(expected, st.ToString());

        }

    }
}
