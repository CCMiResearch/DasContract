using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    //Priority Hit Policy - Multiple rules can be matched and the returned output is based on a provided priority.
    //Priority is provided in the Annotation of the first rule row. This row is then skipped. Format: (output1, output2, ...), (output1, output2, ...), ...
    public class PriorityHPConverter : HitPolicyConverter
    {
        public int PriorityOffset { get; set; } = 0;

        public PriorityHPConverter(int offset)
        {
            PriorityOffset = offset;
        }

        public override SolidityFunction CreateDecisionFunction()
        {
            FunctionName = Regex.Replace(Decision.Id, @" ", "").ToLowerCamelCase();
            SolidityFunction function = new SolidityFunction(FunctionName, SolidityVisibility.Internal, $"{OutputStructName} memory", true);
            //Add declaration of helper varaibles
            var prioritiesFormatted = GetPriorities(Decision);
            var noUniqueOutputs = Regex.Matches(prioritiesFormatted, $"{OutputStructName}").Count;
            function.AddToBody(new SolidityStatement($"{OutputStructName}[{noUniqueOutputs}] memory priorities = {prioritiesFormatted}", true));
            function.AddToBody(new SolidityStatement($"{OutputStructName} memory output", true));
            function.AddToBody(new SolidityStatement($"bool matchedRule = false", true));

            var rules = GetAllConditions();            
            foreach (var rule in rules.Skip(PriorityOffset).Select((value, i) => new { i, value }))
            {
                string priorityCheck = GetPriorityCheckBody(rule.i + PriorityOffset);
                function.AddToBody(AddBodyBasedOnRule(rule.value, priorityCheck));
            }
            //Add the rest of the function
            var undefinedOutputCheck = new SolidityIfElse();
            undefinedOutputCheck.AddConditionBlock("!matchedRule", new SolidityStatement("revert('Undefined output')", true));
            function.AddToBody(undefinedOutputCheck);
            function.AddToBody(new SolidityStatement("return output", true));
            return function;
        }

        //Returns output for section representing situation if there is the match or not
        private string GetPriorityCheckBody(int ruleIndex)
        {
            var priorityCheck = new SolidityIfElse();
            //Assign output if there is already match
            string matchBody = GetMatchBody(ruleIndex);
            priorityCheck.AddConditionBlock("!matchedRule", new SolidityStatement(matchBody, false));

            //Assign output if there is no match yet
            string noMatchBody = GetNoMatchBody(ruleIndex);
            priorityCheck.AddConditionBlock("", new SolidityStatement(noMatchBody, true));
            return priorityCheck.ToString();
        }

        //Returns output for section representing situation if there is already match
        private string GetMatchBody(int ruleIndex)
        {
            string matchBody = $"output = {OutputStructName}(";
            foreach (var outputEntry in Decision.DecisionTable.Rules[ruleIndex].OutputEntries.Select((value, i) => new { i, value }))
            {
                var dataType = Decision.DecisionTable.Outputs[outputEntry.i].TypeRef;
                var convertedValue = ConvertToSolidityValue(outputEntry.value.Text, dataType);
                matchBody += $"{convertedValue}";
                if (outputEntry.i + 1 < Decision.DecisionTable.Rules[ruleIndex].OutputEntries.Count)
                    matchBody += ", ";
            }
            matchBody += ");\n";
            matchBody += "matchedRule = true;";
            return matchBody;
        }

        //Returns output for section representing situation if there is no match yet
        private string GetNoMatchBody(int ruleIndex)
        {
            string noMatchBody = $"output = {FunctionName}_decideByPriority(priorities, output, {OutputStructName}(";
            foreach (var outputEntry in Decision.DecisionTable.Rules[ruleIndex].OutputEntries.Select((value, i) => new { i, value }))
            {
                var dataType = Decision.DecisionTable.Outputs[outputEntry.i].TypeRef;
                var convertedValue = ConvertToSolidityValue(outputEntry.value.Text, dataType);
                noMatchBody += $"{convertedValue}";
                if (outputEntry.i + 1 < Decision.DecisionTable.Rules[ruleIndex].OutputEntries.Count)
                    noMatchBody += ", ";
            }
            noMatchBody += "))";
            return noMatchBody;
        }

        //Functions for comparison of matched rule and already matched outputs
        public override SolidityFunction CreateHelperFunction()
        {
            FunctionName = Regex.Replace(Decision.Id, @" ", "").ToLowerCamelCase();
            SolidityFunction function = new SolidityFunction($"{FunctionName}_decideByPriority", SolidityVisibility.Internal, $"{OutputStructName} memory", true);

            var prioritiesFormatted = GetPriorities(Decision);
            var noUniqueOutputs = Regex.Matches(prioritiesFormatted, $"{OutputStructName}").Count;
            function.AddParameter(new SolidityParameter($"{OutputStructName}[{noUniqueOutputs}] memory", "priorities"));
            function.AddParameter(new SolidityParameter($"{OutputStructName} memory", "currentOutput"));
            function.AddParameter(new SolidityParameter($"{OutputStructName} memory", "newOutput"));

            var priorityComparison = GetPriorityComparisonBody();            
            var priorityIteration = new SolidityFor("i", $"{noUniqueOutputs}");
            priorityIteration.AddToBody(new SolidityStatement(priorityComparison.ToString(), false));
            function.AddToBody(priorityIteration);
            function.AddToBody(new SolidityStatement("revert('Undefined output')", true));
            return function;
        }

        private string GetPriorityComparisonBody()
        {
            var priorityComparison = new SolidityIfElse();
            string ifCondition = string.Empty;
            //Make function of cycles comparing priorities
            foreach (var outputVarable in Decision.DecisionTable.Outputs.Select((value, i) => new { i, value }))
            {
                ifCondition += $"priorities[i].{outputVarable.value.Name} == currentOutput.{outputVarable.value.Name}";
                if (outputVarable.i + 1 < Decision.DecisionTable.Outputs.Count)
                    ifCondition += " && ";
            }
            priorityComparison.AddConditionBlock(ifCondition, new SolidityStatement("return currentOutput", true));
            string elseCondition = string.Empty;
            foreach (var outputVarable in Decision.DecisionTable.Outputs.Select((value, i) => new { i, value }))
            {
                elseCondition += $"priorities[i].{outputVarable.value.Name} == newOutput.{outputVarable.value.Name}";
                if (outputVarable.i + 1 < Decision.DecisionTable.Outputs.Count)
                    elseCondition += " && ";
            }
            priorityComparison.AddConditionBlock(elseCondition, new SolidityStatement("return newOutput", true));
            return priorityComparison.ToString();
        }

        public string GetPriorities(Decision Decision)
        {
            //Get string representation of priority list
            var priorityAnnotation = Decision.DecisionTable.Rules[0].Description;
            priorityAnnotation = priorityAnnotation.Replace("(", "");
            priorityAnnotation = priorityAnnotation.Replace(" ", "");
            var priorities = priorityAnnotation.Split(new char[] { ')' }, StringSplitOptions.RemoveEmptyEntries);
            string prioritiesFormatted = "[";
            //Parse list of outputs
            foreach (var priority in priorities.Select((value, i) => new { i, value }))
            {
                var outputs = priority.value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                prioritiesFormatted += $"{OutputStructName}(";
                //Format the priority list
                foreach (var output in outputs.Select((value, i) => new { i, value }))
                {
                    var dataType = Decision.DecisionTable.Outputs[output.i].TypeRef;
                    var convertedValue = ConvertToSolidityValue(output.value, dataType);
                    prioritiesFormatted += $"{convertedValue}";
                    if (output.i + 1 < outputs.Count())
                        prioritiesFormatted += ", ";
                }
                prioritiesFormatted += ")";
                if (priority.i + 1 < priorities.Count())
                    prioritiesFormatted += ", ";
            }
            prioritiesFormatted += "]";
            return prioritiesFormatted;
        }
    }
}
