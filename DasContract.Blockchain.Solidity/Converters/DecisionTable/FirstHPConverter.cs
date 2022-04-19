using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System.Linq;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    public class FirstHPConverter : HitPolicyConverter
    {
        public override SolidityFunction CreateDecisionFunction(Decision decision)
        {
            var functionName = Regex.Replace(decision.Id, @" ", "").ToLowerCamelCase();
            SolidityFunction function = new SolidityFunction(functionName, SolidityVisibility.Internal, $"{outputStructName} memory", true);
            //Add declaration of helper varaibles
            function.AddToBody(new SolidityStatement($"{outputStructName} memory output", true));

            var rules = GetAllConditions(decision);
            foreach (var rule in rules.Select((value, i) => new { i, value }))
            {
                //Assign output if there is already the match
                string conditionBody = $"output = {outputStructName}(";
                foreach (var outputEntry in decision.DecisionTable.Rules[rule.i].OutputEntries.Select((value, i) => new { i, value }))
                {
                    var dataType = decision.DecisionTable.Outputs[outputEntry.i].TypeRef;
                    var convertedValue = ConvertToSolidityValue(outputEntry.value.Text, dataType);
                    conditionBody += $"{convertedValue}";
                    if (outputEntry.i + 1 < decision.DecisionTable.Rules[rule.i].OutputEntries.Count)
                    {
                        conditionBody += ", ";
                    }

                }
                conditionBody += ");\n";
                conditionBody += "return output;";


                //If the row is empty then do not put the logic into conditional statement
                if (string.IsNullOrEmpty(rule.value))
                {
                    function.AddToBody(new SolidityStatement(conditionBody, false));
                }
                else
                {
                    var condition = new SolidityIfElse();
                    condition.AddConditionBlock(rule.value, new SolidityStatement(conditionBody, false));
                    function.AddToBody(condition);
                }
            }

            //Add the rest of the function
            function.AddToBody(new SolidityStatement("revert('Undefined output')", true));
            return function;
        }
    }
}
