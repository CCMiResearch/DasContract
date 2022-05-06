using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System.Linq;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    //First Hit Policy - Multiple rules can be matched, but the first matched output is returned.
    public class FirstHPConverter : HitPolicyConverter
    {
        public override SolidityFunction CreateDecisionFunction()
        {
            FunctionName = Regex.Replace(Decision.Id, @" ", "").ToLowerCamelCase();
            SolidityFunction function = new SolidityFunction(FunctionName, SolidityVisibility.Internal, $"{OutputStructName} memory", true);
            //Add declaration of helper varaibles
            function.AddToBody(new SolidityStatement($"{OutputStructName} memory output", true));
            var emptyRule = false;

            //Add checks of conditions for matches and their bodies
            var rules = GetAllConditions();
            foreach (var rule in rules.Select((value, i) => new { i, value }))
            {
                //Assign output if there is already the match
                string conditionBody = GetConditionBody(rule.i);
                
                //If the row is empty then do not put the logic into conditional statement
                if (string.IsNullOrEmpty(rule.value))
                {
                    var condition = new SolidityStatement(conditionBody, false);
                    function.AddToBody(condition);
                    emptyRule = true;
                    break;
                }
                else
                {
                    var condition = new SolidityIfElse();
                    condition.AddConditionBlock(rule.value, new SolidityStatement(conditionBody, false));
                    function.AddToBody(condition);
                }
            }

            //Add the rest of the function
            if (!emptyRule)
                function.AddToBody(new SolidityStatement("revert('Undefined output')", true));
            return function;
        }

        //Returns output for section representing situation if there is already the match
        private string GetConditionBody(int ruleIndex)
        {
            string conditionBody = $"output = {OutputStructName}(";
            foreach (var outputEntry in Decision.DecisionTable.Rules[ruleIndex].OutputEntries.Select((value, i) => new { i, value }))
            {
                var dataType = Decision.DecisionTable.Outputs[outputEntry.i].TypeRef;
                var convertedValue = ConvertToSolidityValue(outputEntry.value.Text, dataType);
                conditionBody += $"{convertedValue}";
                if (outputEntry.i + 1 < Decision.DecisionTable.Rules[ruleIndex].OutputEntries.Count)
                {
                    conditionBody += ", ";
                }

            }
            conditionBody += ");\n";
            conditionBody += "return output;";
            return conditionBody;
        }
    }
}
