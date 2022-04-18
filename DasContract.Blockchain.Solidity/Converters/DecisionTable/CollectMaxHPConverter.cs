using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    public class CollectMaxHPConverter : HitPolicyConverter
    {
        public override SolidityFunction CreateDecisionFunction(Decision decision)
        {
            //Define function's header
            var functionName = Regex.Replace(decision.Id, @" ", "").ToLowerCamelCase();
            SolidityFunction function = new SolidityFunction(functionName, SolidityVisibility.Internal, $"{outputStructName} memory", true);
            //Add declaration of helper varaibles
            function.AddToBody(new SolidityStatement($"{outputStructName} memory output", true));
            function.AddToBody(new SolidityStatement($"bool matchedRule = false", true));

            //For each row representing rule create condition for if statement
            var rules = GetAllConditions(decision);
            foreach (var rule in rules.Select((value, i) => new { i, value }))
            {
                //Assign maximum if this is the first met condition
                var noMatchCheck = new SolidityIfElse();
                string noMatchCheckBody = String.Empty;
                foreach (var outputEntry in decision.DecisionTable.Rules[rule.i].OutputEntries.Select((value, i) => new { i, value }))
                {
                    noMatchCheckBody += $"output.{decision.DecisionTable.Outputs[outputEntry.i].Name} = {outputEntry.value.Text};";
                    if (outputEntry.i + 1 < decision.DecisionTable.Rules[rule.i].OutputEntries.Count())
                        noMatchCheckBody += "\n";
                }
                noMatchCheck.AddConditionBlock("!matchedRule", new SolidityStatement(noMatchCheckBody, false));

                //Assign maximum if the values are greater
                string matchCheckBody = String.Empty;
                foreach (var outputEntry in decision.DecisionTable.Rules[rule.i].OutputEntries.Select((value, i) => new { i, value }))
                {
                    var outputsCheck = new SolidityIfElse();
                    string outputsCheckCondition = $"output.{decision.DecisionTable.Outputs[outputEntry.i].Name} < {outputEntry.value.Text};";
                    string outputsCheckBody = $"output.{decision.DecisionTable.Outputs[outputEntry.i].Name} = {outputEntry.value.Text};";
                    outputsCheck.AddConditionBlock(outputsCheckCondition, new SolidityStatement(outputsCheckBody, false));
                    matchCheckBody += outputsCheck.ToString();
                }
                noMatchCheck.AddConditionBlock(String.Empty, new SolidityStatement(matchCheckBody, false));

                string conditionBody = noMatchCheck.ToString();
                conditionBody += "matchedRule = true;";
                //If the row is empty then do not put the logic into conditional statement
                if (String.IsNullOrEmpty(rule.value))
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
            var undefinedOutputCheck = new SolidityIfElse();
            undefinedOutputCheck.AddConditionBlock("!matchedRule", new SolidityStatement("revert('Undefined output')", true));
            function.AddToBody(undefinedOutputCheck);
            function.AddToBody(new SolidityStatement("return output", true));
            return function;
        }
    }
}
