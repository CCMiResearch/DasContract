using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    //Collect Hit Policy with Sum Aggregation - The result of the decision table is the sum of all the outputs.
    public class CollectSumHPConverter : HitPolicyConverter
    {
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
                //Add to sum if the conditions are met
                string conditionBody = string.Empty;
                foreach (var outputEntry in Decision.DecisionTable.Rules[rule.i].OutputEntries.Select((value, i) => new { i, value }))
                {
                    conditionBody += $"output.{Decision.DecisionTable.Outputs[outputEntry.i].Name.Replace(".", "__")} += {outputEntry.value.Text};\n";
                }
                conditionBody += "matchedRule = true;";

                //If the row is empty then do not put the logic into conditional statement
                function.AddToBody(AddBodyBasedOnRule(rule.value, conditionBody));
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
