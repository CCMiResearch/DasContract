using DasContract.Blockchain.Solidity.SolidityComponents;
using Xunit;

namespace DasContractTests.DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityContractTest
    {
        [Fact]
        public void TestEmptyContract()
        {
            SolidityContract contract = new SolidityContract("foo");

            var actual = contract.ToString();
            var expected = "contract foo { \n }";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestContractWithFunction()
        {
            SolidityContract contract = new SolidityContract("foo");

            contract.AddComponent(new SolidityFunction("bar", SolidityVisibility.Public));

            var actual = contract.ToString();
            var expected = "contract foo { \n" +
                "\tfunction bar() public {\n" +
                "\t}\n" +
                "\n }";

            Assert.Equal(expected, actual);
        }
    }
}
