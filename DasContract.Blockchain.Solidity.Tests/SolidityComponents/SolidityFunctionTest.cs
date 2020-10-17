using DasContract.Blockchain.Solidity.SolidityComponents;
using Xunit;


namespace DasContractTests.DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityFunctionTest
    {
        string functionName = "foo";
        SolidityVisibility visibility = SolidityVisibility.Public;
        private SolidityFunction GetFunction()
        {
            SolidityFunction f = new SolidityFunction(functionName, visibility);
            return f;
        }

        [Fact]
        public void EmptyFunctionTest()
        {
            var actual = GetFunction().ToString();
            var expected = "function foo() public {\n}\n";

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void FunctionWithParameterTest()
        {
            var f = GetFunction();
            f.AddParameter(new SolidityParameter("int", "bar"));

            var actual = f.ToString();
            var expected = "function foo(int bar) public {\n}\n";

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void FunctionWithModifierTest()
        {
            var f = GetFunction();
            f.AddModifier("isActive");

            var actual = f.ToString();
            var expected = "function foo() isActive public {\n}\n";

            Assert.Equal(actual, expected);
        }

    }
}
