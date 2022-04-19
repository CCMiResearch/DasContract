using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    public class RuleOrderHPConverter : OutputOrderHPConverter
    {
        public RuleOrderHPConverter(int offset) : base(offset) { }

        public override string GetPriorities(Decision decision)
        {
            List<string> possibleOutputs = new List<string>();
            foreach (var rule in decision.DecisionTable.Rules)
            {
                string ruleOutputs = $"{outputStructName}(";
                foreach (var outputEntry in rule.OutputEntries.Select((value, i) => new { i, value }))
                {
                    var dataType = decision.DecisionTable.Outputs[outputEntry.i].TypeRef;
                    var convertedValue = ConvertToSolidityValue(outputEntry.value.Text, dataType);
                    ruleOutputs += $"{convertedValue}";
                    if (outputEntry.i + 1 < rule.OutputEntries.Count)
                        ruleOutputs += ", ";
                }
                ruleOutputs += ")";
                possibleOutputs.Add(ruleOutputs);
            }
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
