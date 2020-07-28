using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DasToSolidity.Solidity;
using DasToSolidity.SolidityConverter;

namespace DasContractTests.DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityModifierTest
    {
        [Fact]
        public void EmptyModifierTest()
        {
            SolidityModifier modifier = new SolidityModifier("isEnabled");

            var actual = modifier.ToString();
            var expected = "modifier isEnabled{\n" +
                "\t_;\n}\n";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ModifierWithBodyTest()
        {
            SolidityModifier modifier = new SolidityModifier("isEnabled");
            modifier.AddToBody(new SolidityStatement("a = b"));

            var actual = modifier.ToString();
            var expected = "modifier isEnabled{\n" +
                "\ta = b;\n" +
                "\t_;\n}\n";

            Assert.Equal(expected, actual);
        }
    }
}
