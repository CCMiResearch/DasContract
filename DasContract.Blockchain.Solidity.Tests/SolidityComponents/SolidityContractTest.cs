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

        [Fact]
        public void TestMultipleInheritance()
        {
            SolidityContract contract = new SolidityContract("foo");
            contract.AddInheritance("Bar1");
            contract.AddInheritance("Bar2");

            var actual = contract.ToString();
            var expected = "contract foo is Bar1, Bar2{ \n }";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestSingleInheritance()
        {
            SolidityContract contract = new SolidityContract("foo");
            contract.AddInheritance("Bar1");

            var actual = contract.ToString();
            var expected = "contract foo is Bar1{ \n }";

            Assert.Equal(expected, actual);
        }
    }
}
