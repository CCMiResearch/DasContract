﻿using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System.Linq;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    //Unique Hit Policy - All rules are disjoint and only one rule can be matched.
    public class UniqueHPConverter : HitPolicyConverter
    {
        public override SolidityFunction CreateDecisionFunction()
        {
            FunctionName = Regex.Replace(Decision.Id, @" ", "").ToLowerCamelCase();
            SolidityFunction function = new SolidityFunction(FunctionName, SolidityVisibility.Internal, $"{OutputStructName} memory", true);
            //Add declaration of helper varaibles
            function.AddToBody(new SolidityStatement($"{OutputStructName} memory output", true));
            function.AddToBody(new SolidityStatement($"bool matchedRule = false", true));

            //Add checks of conditions for matches and their bodies
            var rules = GetAllConditions();
            foreach (var rule in rules.Select((value, i) => new { i, value }))
            {
                var conditionCheck = GetConditionCheck(rule.i);
                function.AddToBody(AddBodyBasedOnRule(rule.value, conditionCheck.ToString()));
            }

            //Add the rest of the function
            var undefinedOutputCheck = new SolidityIfElse();
            undefinedOutputCheck.AddConditionBlock("!matchedRule", new SolidityStatement("revert('Undefined output')", true));
            function.AddToBody(undefinedOutputCheck);
            function.AddToBody(new SolidityStatement("return output", true));
            return function;
        }

        private SolidityIfElse GetConditionCheck(int ruleIndex)
        {
            var conditionCheck = new SolidityIfElse();
            //Assign output if there is already the match
            string matchBody = GetMatchBody(ruleIndex);
            conditionCheck.AddConditionBlock("!matchedRule", new SolidityStatement(matchBody, false));
            //Assign output if there is no match yet
            conditionCheck.AddConditionBlock("", new SolidityStatement("revert('Undefined output')", true));

            return conditionCheck;
        }

        //Returns output for section representing situation if there is already the match
        private string GetMatchBody(int ruleIndex)
        {
            string matchBody = $"output = {OutputStructName}(";
            foreach (var outputEntry in Decision.DecisionTable.Rules[ruleIndex].OutputEntries.Select((value, i) => new { i, value }))
            {
                var dataType = Decision.DecisionTable.Outputs[outputEntry.i].TypeRef;
                var convertedValue = ConvertToSolidityValue(outputEntry.value.Text, dataType);
                matchBody += $"{convertedValue}";
                if (outputEntry.i + 1 < Decision.DecisionTable.Rules[ruleIndex].OutputEntries.Count)
                {
                    matchBody += ", ";
                }

            }
            matchBody += ");\n";
            matchBody += "matchedRule = true;";
            return matchBody;
        }
    }
}
