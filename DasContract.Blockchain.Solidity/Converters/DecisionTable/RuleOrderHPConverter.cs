using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    //Rule Order Hit Policy - Returns all matched values in the order of the rule definition from top to bottom.
    public class RuleOrderHPConverter : OutputOrderHPConverter
    {
        public RuleOrderHPConverter(int offset) : base(offset) { }

        public override string CreateOutputDeclaration()
        {
            FunctionName = Regex.Replace(Decision.Id, @" ", "").ToLowerCamelCase();
            OutputStructName = string.Concat(Regex.Replace(Decision.Id, @" ", "").ToUpperCamelCase(), "Output");
            return $"{OutputStructName}[] memory {FunctionName}Output";
        }

        public override string GetPriorities()
        {
            //Parse annotation before removing duplicates
            List<string> possibleOutputs = new List<string>();
            foreach (var rule in Decision.DecisionTable.Rules)
            {
                string ruleOutputs = $"{OutputStructName}(";
                foreach (var outputEntry in rule.OutputEntries.Select((value, i) => new { i, value }))
                {
                    var dataType = Decision.DecisionTable.Outputs[outputEntry.i].TypeRef;
                    var convertedValue = ConvertToSolidityValue(outputEntry.value.Text, dataType);
                    ruleOutputs += $"{convertedValue}";
                    if (outputEntry.i + 1 < rule.OutputEntries.Count)
                        ruleOutputs += ", ";
                }
                ruleOutputs += ")";
                possibleOutputs.Add(ruleOutputs);
            }
            //Remove duplicates and format the priority list
            var uniqueOutputs = possibleOutputs.Distinct().ToList();
            string prioritiesFormatted = "[";
            foreach (var output in uniqueOutputs.Select((value, i) => new { i, value }))
            {
                prioritiesFormatted += output.value;
                if (output.i + 1 < uniqueOutputs.Count)
                    prioritiesFormatted += ", ";
            }
            prioritiesFormatted += "]";
            return prioritiesFormatted;
        }
    }
}
