using DasToSolidity.SolidityConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DasContractTests.DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityStatementTest
    {
        string line1 = "uint256 value = 0";
        string line2 = "value += 1";

        [Fact]
        public void EmptyStatementTest()
        {
            var statement = new SolidityStatement();

            Assert.Equal("", statement.ToString());
        }

        [Fact]
        public void OneLineStatementTest()
        {
            var statement = new SolidityStatement();

            statement.Add(line1);
            Assert.Equal(line1 + ";\n", statement.ToString());
        }

        [Fact]
        public void MultipleLineStatementTest()
        {
            var statement = new SolidityStatement();

            statement.Add(line1);
            statement.Add(line2);
            Assert.Equal(line1 + ";\n" + line2 + ";\n", statement.ToString());
        }

        [Fact]
        public void NoSemicolonTest()
        {
            var statement = new SolidityStatement();

            statement.Add(line1, false);
            Assert.Equal(line1 + "\n", statement.ToString());
        }

        [Fact]
        public void FromListTest()
        {
            List<string> list = new List<string>();

            list.Add(line1);
            list.Add(line2);

            var statement = new SolidityStatement(list);

            Assert.Equal(line1 + ";\n" + line2 + ";\n", statement.ToString());
        }

        [Fact]
        public void GetStatementsTest()
        {
            List<string> list = new List<string>();

            list.Add(line1);
            list.Add(line2);

            var statement = new SolidityStatement(list);
            var actual = statement.GetStatements();
            Assert.True(!list.Except(actual).Any() && list.Count == actual.Count);
        }
    }
}
