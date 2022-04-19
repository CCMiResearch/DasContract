using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    public class PriorityHPConverter : HitPolicyConverter
    {
        private int priorityOffset;

        public PriorityHPConverter(int offset)
        {
            priorityOffset = offset;
        }

        public override SolidityFunction CreateDecisionFunction(Decision decision)
        {
            var functionName = Regex.Replace(decision.Id, @" ", "").ToLowerCamelCase();
            SolidityFunction function = new SolidityFunction(functionName, SolidityVisibility.Internal, $"{outputStructName} memory", true);
            //Add declaration of helper varaibles
            var prioritiesFormatted = GetPriorities(decision);
            var noUniqueOutputs = Regex.Matches(prioritiesFormatted, $"{outputStructName}").Count;
            function.AddToBody(new SolidityStatement($"{outputStructName}[{noUniqueOutputs}] memory priorities = {prioritiesFormatted}", true));
            function.AddToBody(new SolidityStatement($"{outputStructName} memory output", true));
            function.AddToBody(new SolidityStatement($"bool matchedRule = false", true));

            var rules = GetAllConditions(decision);            
            foreach (var rule in rules.Skip(priorityOffset).Select((value, i) => new { i, value }))
            {
                var priorityCheck = new SolidityIfElse();
                //Assign output if there is already the match
                string matchBody = $"output = {outputStructName}(";
                foreach (var outputEntry in decision.DecisionTable.Rules[rule.i + priorityOffset].OutputEntries.Select((value, i) => new { i, value }))
                {
                    var dataType = decision.DecisionTable.Outputs[outputEntry.i].TypeRef;
                    var convertedValue = ConvertToSolidityValue(outputEntry.value.Text, dataType);
                    matchBody += $"{convertedValue}";
                    if (outputEntry.i + 1 < decision.DecisionTable.Rules[rule.i + priorityOffset].OutputEntries.Count)
                    {
                        matchBody += ", ";
                    }

                }
                matchBody += ");\n";
                matchBody += "matchedRule = true;";
                priorityCheck.AddConditionBlock("!matchedRule", new SolidityStatement(matchBody, false));

                //Assign output if there is no match yet
                string noMatchBody = $"output = {functionName}_decideByPriority(priorities, output, {outputStructName}(";
                foreach (var outputEntry in decision.DecisionTable.Rules[rule.i + priorityOffset].OutputEntries.Select((value, i) => new { i, value }))
                {
                    var dataType = decision.DecisionTable.Outputs[outputEntry.i].TypeRef;
                    var convertedValue = ConvertToSolidityValue(outputEntry.value.Text, dataType);
                    noMatchBody += $"{convertedValue}";
                    if (outputEntry.i + 1 < decision.DecisionTable.Rules[rule.i + priorityOffset].OutputEntries.Count)
                    {
                        noMatchBody += ", ";
                    }

                }
                noMatchBody += "))";
                priorityCheck.AddConditionBlock("", new SolidityStatement(noMatchBody, true));

                //If the row is empty then do not put the logic into conditional statement
                if (string.IsNullOrEmpty(rule.value))
                {
                    function.AddToBody(new SolidityStatement(priorityCheck.ToString(), false));
                }
                else
                {
                    var condition = new SolidityIfElse();
                    condition.AddConditionBlock(rule.value, new SolidityStatement(priorityCheck.ToString(), false));
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

        public override SolidityFunction CreateHelperFunction(Decision decision)
        {
            var functionName = Regex.Replace(decision.Id, @" ", "").ToLowerCamelCase();
            SolidityFunction function = new SolidityFunction($"{functionName}_decideByPriority", SolidityVisibility.Internal, $"{outputStructName} memory", true);

            var prioritiesFormatted = GetPriorities(decision);
            var noUniqueOutputs = Regex.Matches(prioritiesFormatted, $"{outputStructName}").Count;
            function.AddParameter(new SolidityParameter($"{outputStructName}[{noUniqueOutputs}] memory", "priorities"));
            function.AddParameter(new SolidityParameter($"{outputStructName} memory", "currentOutput"));
            function.AddParameter(new SolidityParameter($"{outputStructName} memory", "newOutput"));

            var priorityComparison = new SolidityIfElse();
            string ifCondition = string.Empty;
            //Make function of those cycles
            foreach (var outputVarable in decision.DecisionTable.Outputs.Select((value, i) => new { i, value }))
            {
                ifCondition += $"priorities[i].{outputVarable.value.Name} == currentOutput.{outputVarable.value.Name}";
                if (outputVarable.i + 1 < decision.DecisionTable.Outputs.Count)
                {
                    ifCondition += " && ";
                }
            }
            priorityComparison.AddConditionBlock(ifCondition, new SolidityStatement("return currentOutput", true));
            string elseCondition = string.Empty;
            foreach (var outputVarable in decision.DecisionTable.Outputs.Select((value, i) => new { i, value }))
            {
                elseCondition += $"priorities[i].{outputVarable.value.Name} == newOutput.{outputVarable.value.Name}";
                if (outputVarable.i + 1 < decision.DecisionTable.Outputs.Count)
                {
                    elseCondition += " && ";
                }
            }
            priorityComparison.AddConditionBlock(elseCondition, new SolidityStatement("return newOutput", true));
            
            var priorityIteration = new SolidityFor("i", $"{noUniqueOutputs}");
            priorityIteration.AddToBody(new SolidityStatement(priorityComparison.ToString(), false));
            function.AddToBody(priorityIteration);
            function.AddToBody(new SolidityStatement("revert('Undefined output')", true));
            return function;
        }

        public string GetPriorities(Decision decision)
        {
            var priorityAnnotation = decision.DecisionTable.Rules[0].Description;
            priorityAnnotation = priorityAnnotation.Replace("(", "");
            priorityAnnotation = priorityAnnotation.Replace(" ", "");
            var priorities = priorityAnnotation.Split(new char[] { ')' }, StringSplitOptions.RemoveEmptyEntries);
            string prioritiesFormatted = "[";
            foreach (var priority in priorities.Select((value, i) => new { i, value }))
            {
                var outputs = priority.value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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
