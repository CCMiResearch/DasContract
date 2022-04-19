using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    public class OutputOrderHPConverter : HitPolicyConverter
    {
        private int priorityOffset;

        public OutputOrderHPConverter(int offset)
        {
            priorityOffset = offset;
        }

        public override SolidityFunction CreateDecisionFunction(Decision decision)
        {
            //Define function's header
            var functionName = Regex.Replace(decision.Id, @" ", "").ToLowerCamelCase();
            SolidityFunction function = new SolidityFunction(functionName, SolidityVisibility.Internal, $"{outputStructName}[] memory", true);
            //Add declaration of helper varaibles
            var prioritiesFormatted = GetPriorities(decision);
            var noUniqueOutputs = Regex.Matches(prioritiesFormatted, $"{outputStructName}").Count;
            function.AddToBody(new SolidityStatement($"{outputStructName}[{noUniqueOutputs}] memory priorities = {prioritiesFormatted}", true));
            function.AddToBody(new SolidityStatement($"bool[{noUniqueOutputs}] memory existsInOutput", true));
            function.AddToBody(new SolidityStatement($"uint outputSize = 0", true));
            function.AddToBody(new SolidityStatement($"bool matchedRule = false", true));

            var rules = GetAllConditions(decision);
            foreach (var rule in rules.Skip(priorityOffset).Select((value, i) => new { i, value }))
            {
                string priorityListCheckBody = "existsInOutput[i] = true;\n" +
                                               "outputSize++;\n" +
                                               "matchedRule = true;\n" +
                                               "break;";
                var priorityListCheck = new SolidityIfElse();
                string priorityListCheckCond = "!existsInOutput[i]";
                foreach (var outputEntry in decision.DecisionTable.Rules[rule.i + priorityOffset].OutputEntries.Select((value, i) => new { i, value }))
                {
                    var comparisonVar = $"priorities[i].{decision.DecisionTable.Outputs[outputEntry.i].Name}";
                    var comparisonType = decision.DecisionTable.Outputs[outputEntry.i].TypeRef;
                    var comparisonEntry = outputEntry.value.Text;
                    var comparison = ConvertExpressionToCondition(comparisonVar, comparisonType, comparisonEntry);
                    priorityListCheckCond += $" && {comparison}";
                }
                priorityListCheck.AddConditionBlock(priorityListCheckCond, new SolidityStatement(priorityListCheckBody, false));
                var priorityCheckLoop = new SolidityFor("i", $"{noUniqueOutputs}");
                priorityCheckLoop.AddToBody(new SolidityStatement(priorityListCheck.ToString(), false));

                //If the row is empty then do not put the logic into conditional statement
                if (string.IsNullOrEmpty(rule.value))
                {
                    function.AddToBody(new SolidityStatement(priorityCheckLoop.ToString(), false));
                }
                else
                {
                    var condition = new SolidityIfElse();
                    condition.AddConditionBlock(rule.value, new SolidityStatement(priorityCheckLoop.ToString(), false));
                    function.AddToBody(condition);
                }
            }

            //Initialization of output list
            function.AddToBody(new SolidityStatement("uint j = 0", true));
            function.AddToBody(new SolidityStatement($"{outputStructName}[] memory output = new {outputStructName}[](outputSize)", true));
            var initForLoop = new SolidityFor("i", $"{noUniqueOutputs}");
            //Add only outputs that meet the conditions
            var initCheck = new SolidityIfElse();
            string initCheckBody = $"output[j] = {outputStructName}(";
            foreach (var output in decision.DecisionTable.Outputs.Select((value, i) => new { i, value }))
            {
                initCheckBody += $"priorities[i].{output.value.Name}";
                if (output.i + 1 < decision.DecisionTable.Outputs.Count)
                {
                    initCheckBody += ", ";
                }
            }
            initCheckBody += ");\n";
            initCheckBody += "j++;";
            initCheck.AddConditionBlock("existsInOutput[i]", new SolidityStatement(initCheckBody, false));
            initForLoop.AddToBody(new SolidityStatement(initCheck.ToString(), false));
            function.AddToBody(initForLoop);

            //Add the rest of the function
            var undefinedOutputCheck = new SolidityIfElse();
            undefinedOutputCheck.AddConditionBlock("!matchedRule", new SolidityStatement("revert('Undefined output')", true));
            function.AddToBody(undefinedOutputCheck);
            function.AddToBody(new SolidityStatement("return output", true));
            return function;
        }

        public virtual string GetPriorities(Decision decision)
        {
            var priorityAnnotation = decision.DecisionTable.Rules[0].Description;
            priorityAnnotation = priorityAnnotation.Replace("(", "");
            priorityAnnotation = priorityAnnotation.Replace(" ", "");
            var priorities = priorityAnnotation.Split(new char[]{')'}, StringSplitOptions.RemoveEmptyEntries);
            string prioritiesFormatted = "[";
            foreach (var priority in priorities.Select((value, i) => new { i, value }))
            {
                var outputs = priority.value.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
                prioritiesFormatted += $"{outputStructName}(";
                foreach (var output in outputs.Select((value, i) => new { i, value })) 
                {
                    var dataType = decision.DecisionTable.Outputs[output.i].TypeRef;
                    var convertedValue = ConvertToSolidityValue(output.value, dataType);
                    prioritiesFormatted += $"{convertedValue}";
                    if (output.i + 1 < outputs.Count())
                    {
                        prioritiesFormatted += ", ";
                    }
                }
                prioritiesFormatted += ")";
                if (priority.i + 1 < priorities.Count())
                {
                    prioritiesFormatted += ", ";
                }
            }
            prioritiesFormatted += "]";
            return prioritiesFormatted;
        }


    }
}
