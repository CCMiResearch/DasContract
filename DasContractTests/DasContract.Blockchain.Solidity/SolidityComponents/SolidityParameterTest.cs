using System;
using System.Collections.Generic;
using System.Text;
using DasToSolidity.SolidityConverter;
using Xunit;


namespace DasContractTests.DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityParameterTest
    {
        [Fact]
        public void ParameterStringTest()
        {
            SolidityParameter par = new SolidityParameter("string", "foo");

            Assert.Equal("string memory foo", par.ToLiquidString().ToString());
        }

        [Fact]
        public void ParameterNonStringTest()
        {
            SolidityParameter par = new SolidityParameter("int", "foo");

            Assert.Equal("int foo", par.ToLiquidString().ToString());
        }
    }
}
