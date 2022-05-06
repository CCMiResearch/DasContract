using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    //Collect Hit Policy with Max and Min Aggregation - The result of the Decision table is the largest/smallest value of all the outputs.
    public class CollectMaxMinHPConverter : HitPolicyConverter
    {
        public string Sign { get; set; } = string.Empty;

        public CollectMaxMinHPConverter(bool findMax)
        {
            if (findMax)
                Sign = "<";
            else
                Sign = ">";
        }

        public override SolidityFunction CreateDecisionFunction()
        {
            //Define function's header
            FunctionName = Regex.Replace(Decision.Id, @" ", "").ToLowerCamelCase();
            SolidityFunction function = new SolidityFunction(FunctionName, SolidityVisibility.Internal, $"{OutputStructName} memory", true);
            //Add declaration of helper varaibles
            function.AddToBody(new SolidityStatement($"{OutputStructName} memory output", true));
            function.AddToBody(new SolidityStatement($"bool matchedRule = false", true));

            //For each row representing rule create condition for if statement
            var rules = GetAllConditions();
            foreach (var rule in rules.Select((value, i) => new { i, value }))
            {
                var noMatchCheck = GetMatchOrNotBody(rule.i);
                string conditionBody = noMatchCheck;
                conditionBody += "matchedRule = true;";
                function.AddToBody(AddBodyBasedOnRule(rule.value, conditionBody));
            }

            //Add the rest of the function
            var undefinedOutputCheck = new SolidityIfElse();
            undefinedOutputCheck.AddConditionBlock("!matchedRule", new SolidityStatement("revert('Undefined output')", true));
            function.AddToBody(undefinedOutputCheck);
            function.AddToBody(new SolidityStatement("return output", true));
            return function;
        }

        //Returns output for section representing situation if there is the match or not
        private string GetMatchOrNotBody(int ruleIndex)
        {
            //Assign maximum/minimum if this is the first met condition
            var noMatchCheck = new SolidityIfElse();
            string noMatchCheckBody = string.Empty;
            foreach (var outputEntry in Decision.DecisionTable.Rules[ruleIndex].OutputEntries.Select((value, i) => new { i, value }))
            {
                var dataType = Decision.DecisionTable.Outputs[outputEntry.i].TypeRef;
                var convertedValue = ConvertToSolidityValue(outputEntry.value.Text, dataType);
                noMatchCheckBody += $"output.{Decision.DecisionTable.Outputs[outputEntry.i].Name.Replace(".", "__")} = {convertedValue};";
                if (outputEntry.i + 1 < Decision.DecisionTable.Rules[ruleIndex].OutputEntries.Count())
                    noMatchCheckBody += "\n";
            }
            noMatchCheck.AddConditionBlock("!matchedRule", new SolidityStatement(noMatchCheckBody, false));

            //Assign maximum/minimum if the values are greater/lesser
            string matchCheckBody = string.Empty;
            foreach (var outputEntry in Decision.DecisionTable.Rules[ruleIndex].OutputEntries.Select((value, i) => new { i, value }))
            {
                var outputsCheck = new SolidityIfElse();
                var dataType = Decision.DecisionTable.Outputs[outputEntry.i].TypeRef;
                var convertedValue = ConvertToSolidityValue(outputEntry.value.Text, dataType);
                string outputsCheckCondition = $"output.{Decision.DecisionTable.Outputs[outputEntry.i].Name} {Sign} {convertedValue}";
                string outputsCheckBody = $"output.{Decision.DecisionTable.Outputs[outputEntry.i].Name.Replace(".", "__")} = {convertedValue};";
                outputsCheck.AddConditionBlock(outputsCheckCondition, new SolidityStatement(outputsCheckBody, false));
                matchCheckBody += outputsCheck.ToString();
            }
            noMatchCheck.AddConditionBlock(string.Empty, new SolidityStatement(matchCheckBody, false));
            return noMatchCheck.ToString();
        }
    }
}
