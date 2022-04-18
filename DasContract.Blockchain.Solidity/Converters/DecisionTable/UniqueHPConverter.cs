using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    public class UniqueHPConverter : HitPolicyConverter
    {
        public override SolidityFunction CreateDecisionFunction(Decision decision)
        {
            var functionName = Regex.Replace(decision.Id, @" ", "").ToLowerCamelCase();
            SolidityFunction function = new SolidityFunction(functionName, SolidityVisibility.Internal, $"{outputStructName} memory", true);
            return function;
        }
    }
}
