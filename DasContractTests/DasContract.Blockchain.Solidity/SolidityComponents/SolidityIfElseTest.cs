using System;
using System.Collections.Generic;
using System.Text;
using DasToSolidity.SolidityConverter;
using Xunit;


namespace DasContractTests.DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityIfElseTest
    {
        [Fact]
        public void OneConditionTest()
        {
            SolidityIfElse ifelse = new SolidityIfElse();
            ifelse.AddConditionBlock("a == 2", new SolidityStatement("a = 0"));

            string expected = "if(a == 2){\n" +
                "\ta = 0;\n" +
                "}\n";
            Assert.Equal(expected, ifelse.ToString());
        }

        [Fact]
        public void ThreeConditionsTest()
        {
            SolidityIfElse ifelse = new SolidityIfElse();
            ifelse.AddConditionBlock("a == 2", new SolidityStatement("a = 0"));
            ifelse.AddConditionBlock("b == 2", new SolidityStatement("b = 0"));
            ifelse.AddConditionBlock("c == 2", new SolidityStatement("c = 0"));


            string expected = "if(a == 2){\n" +
                "\ta = 0;\n" +
                "}\n" +
                "else if(b == 2){\n" +
                "\tb = 0;\n" +
                "}\n" +
                "else if(c == 2){\n" +
                "\tc = 0;\n" +
                "}\n";
            Assert.Equal(expected, ifelse.ToString());
        }
    }
}
