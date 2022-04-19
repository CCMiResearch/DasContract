using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    //Output Order Hit Policy - Returns all matched outputs in decreased order of provided priority.
    //Priority is provided in the Annotation of the first rule row. This row is then skipped. Format: (output1, output2, ...), (output1, output2, ...), ...
    public class OutputOrderHPConverter : HitPolicyConverter
    {
        private int PriorityOffset { get; set; } = 0;

        public OutputOrderHPConverter(int offset)
        {
            PriorityOffset = offset;
        }

        public override SolidityStatement CreateOutputDeclaration()
        {
            FunctionName = Regex.Replace(Decision.Id, @" ", "").ToLowerCamelCase();
            OutputStructName = string.Concat(Regex.Replace(Decision.Id, @" ", "").ToUpperCamelCase(), "Output");
            return new SolidityStatement($"{OutputStructName}[] {FunctionName}_Output", true);
        }

        public override SolidityFunction CreateDecisionFunction()
        {
            //Define function's header
            FunctionName = Regex.Replace(Decision.Id, @" ", "").ToLowerCamelCase();
            SolidityFunction function = new SolidityFunction(FunctionName, SolidityVisibility.Internal, $"{OutputStructName}[] memory", true);
            //Add declaration of helper varaibles
            var prioritiesFormatted = GetPriorities();
            var noUniqueOutputs = Regex.Matches(prioritiesFormatted, $"{OutputStructName}").Count;
            function.AddToBody(new SolidityStatement($"{OutputStructName}[{noUniqueOutputs}] memory priorities = {prioritiesFormatted}", true));
            function.AddToBody(new SolidityStatement($"bool[{noUniqueOutputs}] memory existsInOutput", true));
            function.AddToBody(new SolidityStatement($"uint outputSize = 0", true));
            function.AddToBody(new SolidityStatement($"bool matchedRule = false", true));

            var rules = GetAllConditions();
            foreach (var rule in rules.Skip(PriorityOffset).Select((value, i) => new { i, value }))
            {
                string priorityListCheck = GetPirorityListCheck(rule.i + PriorityOffset);
                var priorityCheckLoop = new SolidityFor("i", $"{noUniqueOutputs}");
                priorityCheckLoop.AddToBody(new SolidityStatement(priorityListCheck, false));

                function.AddToBody(AddBodyBasedOnRule(rule.value, priorityCheckLoop.ToString()));
            }

            //Initialization of output list
            function.AddToBody(new SolidityStatement("uint j = 0", true));
            function.AddToBody(new SolidityStatement($"{OutputStructName}[] memory output = new {OutputStructName}[](outputSize)", true));
            var initForLoop = GetInitializationForLoop(noUniqueOutputs);
            function.AddToBody(initForLoop);

            //Add the rest of the function
            var undefinedOutputCheck = new SolidityIfElse();
            undefinedOutputCheck.AddConditionBlock("!matchedRule", new SolidityStatement("revert('Undefined output')", true));
            function.AddToBody(undefinedOutputCheck);
            function.AddToBody(new SolidityStatement("return output", true));
            return function;
        }

        //Returns string representation of section checking existence of matched output in the priority list
        private string GetPirorityListCheck(int ruleIndex)
        {
            var priorityListCheck = new SolidityIfElse();
            string priorityListCheckBody = "existsInOutput[i] = true;\n" +
                                               "outputSize++;\n" +
                                               "matchedRule = true;\n" +
                                               "break;";
            string priorityListCheckCond = "!existsInOutput[i]";
            foreach (var outputEntry in Decision.DecisionTable.Rules[ruleIndex].OutputEntries.Select((value, i) => new { i, value }))
            {
                var comparisonVar = $"priorities[i].{Decision.DecisionTable.Outputs[outputEntry.i].Name}";
                var comparisonType = Decision.DecisionTable.Outputs[outputEntry.i].TypeRef;
                var comparisonEntry = outputEntry.value.Text;
                var comparison = ConvertExpressionToCondition(comparisonVar, comparisonType, comparisonEntry, true);
                priorityListCheckCond += $" && {comparison}";
            }
            priorityListCheck.AddConditionBlock(priorityListCheckCond, new SolidityStatement(priorityListCheckBody, false));
            return priorityListCheck.ToString();
        }

        //Retruns for loop component initializing the output
        private SolidityFor GetInitializationForLoop(int noUniqueOutputs)
        {
            var initForLoop = new SolidityFor("i", $"{noUniqueOutputs}");
            //Add only outputs that meet the conditions
            var initCheck = new SolidityIfElse();
            string initCheckBody = $"output[j] = {OutputStructName}(";
            foreach (var output in Decision.DecisionTable.Outputs.Select((value, i) => new { i, value }))
            {
                initCheckBody += $"priorities[i].{output.value.Name}";
                if (output.i + 1 < Decision.DecisionTable.Outputs.Count)
                {
                    initCheckBody += ", ";
                }
            }
            initCheckBody += ");\n";
            initCheckBody += "j++;";
            initCheck.AddConditionBlock("existsInOutput[i]", new SolidityStatement(initCheckBody, false));
            initForLoop.AddToBody(new SolidityStatement(initCheck.ToString(), false));
            return initForLoop;
        }

        public virtual string GetPriorities()
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
