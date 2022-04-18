using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    public class CollectSumHPConverter : HitPolicyConverter
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
                //Add to sum if the conditions are met
                string conditionBody = String.Empty;
                foreach (var outputEntry in decision.DecisionTable.Rules[rule.i].OutputEntries.Select((value, i) => new { i, value }))
                {
                    conditionBody += $"output.{decision.DecisionTable.Outputs[outputEntry.i].Name} += {outputEntry.value.Text};\n";
                }
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
