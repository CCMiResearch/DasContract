using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System.Linq;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    public class AnyHPConverter : HitPolicyConverter
    {
        public override SolidityFunction CreateDecisionFunction(Decision decision)
        {
            var functionName = Regex.Replace(decision.Id, @" ", "").ToLowerCamelCase();
            SolidityFunction function = new SolidityFunction(functionName, SolidityVisibility.Internal, $"{outputStructName} memory", true);
            //Add declaration of helper varaibles
            function.AddToBody(new SolidityStatement($"{outputStructName} memory output", true));
            function.AddToBody(new SolidityStatement($"bool matchedRule = false", true));

            var rules = GetAllConditions(decision);
            foreach (var rule in rules.Select((value, i) => new { i, value }))
            {
                var conditionCheck = new SolidityIfElse();
                //Assign output if there is already the match
                string matchBody = $"output = {outputStructName}(";
                foreach (var outputEntry in decision.DecisionTable.Rules[rule.i].OutputEntries.Select((value, i) => new { i, value }))
                {
                    var dataType = decision.DecisionTable.Outputs[outputEntry.i].TypeRef;
                    var convertedValue = ConvertToSolidityValue(outputEntry.value.Text, dataType);
                    matchBody += $"{convertedValue}";
                    if (outputEntry.i + 1 < decision.DecisionTable.Rules[rule.i].OutputEntries.Count)
                    {
                        matchBody += ", ";
                    }

                }
                matchBody += ");\n";
                matchBody += "matchedRule = true;";
                conditionCheck.AddConditionBlock("!matchedRule", new SolidityStatement(matchBody, false));

                //Assign output if there is no match yet
                string noMatchCondition = string.Empty;
                foreach (var outputEntry in decision.DecisionTable.Rules[rule.i].OutputEntries.Select((value, i) => new { i, value }))
                {
                    var comparisonVar = $"priorities[i].{decision.DecisionTable.Outputs[outputEntry.i].Name}";
                    var comparisonType = decision.DecisionTable.Outputs[outputEntry.i].TypeRef;
                    var comparisonEntry = outputEntry.value.Text;
                    var comparison = ConvertExpressionToCondition(comparisonVar, comparisonType, comparisonEntry, false);
                    noMatchCondition += $"{comparison}";
                    if (outputEntry.i + 1 < decision.DecisionTable.Rules[rule.i].OutputEntries.Count)
                    {
                        noMatchCondition += " || ";
                    }
                }
                conditionCheck.AddConditionBlock(noMatchCondition, new SolidityStatement("revert('Undefined output')", true));

                //If the row is empty then do not put the logic into conditional statement
                if (string.IsNullOrEmpty(rule.value))
                {
                    function.AddToBody(new SolidityStatement(conditionCheck.ToString(), false));
                }
                else
                {
                    var condition = new SolidityIfElse();
                    condition.AddConditionBlock(rule.value, new SolidityStatement(conditionCheck.ToString(), false));
                    function.AddToBody(condition);
                }
            }

            //Add the rest of the function
            var undefinedOutputCheck = new SolidityIfElse();
            undefinedOutputCheck.AddConditionBlock("!matchedRule", new SolidityStatement("revert('Undefined output')", true));
            function.AddToBody(undefinedOutputCheck);
            function.AddToBody(new SolidityStatement("return output", true));
            return function;
        }
    }
}
