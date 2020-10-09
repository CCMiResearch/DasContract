using DasContract.Blockchain.Solidity.SolidityComponents;
using Xunit;

namespace DasContractTests.DasContract.Blockchain.Solidity.SolidityComponents
{

    public class SolidityConstructorTest
    {
        [Fact]
        public void TestEmptyConstructor()
        {
            SolidityConstructor constructor = new SolidityConstructor();

            var actual = constructor.ToString();
            var expected = "constructor () public payable{\n}\n";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TesyConstructorWithBody()
        {
            SolidityConstructor constructor = new SolidityConstructor();

            constructor.AddToBody(new SolidityStatement("a = 2"));

            var actual = constructor.ToString();
            var expected = "constructor () public payable{\n\ta = 2;\n}\n";

            Assert.Equal(expected, actual);
        }
    }
}
