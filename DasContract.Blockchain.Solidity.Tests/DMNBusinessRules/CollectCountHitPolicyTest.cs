using DasContract.Abstraction.Processes.Dmn;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Blockchain.Solidity.Converters.DecisionTable;
using DasContract.Blockchain.Solidity.Converters.Tasks;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System.Collections.Generic;
using Xunit;

namespace DasContract.Blockchain.Solidity.Tests.DMNBusinessRules
{
    public class CollectCountHitPolicyTest
    {
        [Fact]
        public void DecisionFunctionTest()
        {
            BusinessRuleTask ruleTask = new BusinessRuleTask();
            ruleTask.BusinessRule = new Definitions();

            var decision = new Decision();
            decision.Id = "decision_kowu89q";
            decision.DecisionTable.HitPolicy = "COLLECT";
            decision.DecisionTable.Aggregation = "COUNT";

            decision.DecisionTable.Inputs.Add(new DecisionTableInput()
            {
                InputExpression = new InputExpression()
                {
                    Text = "person.age",
                    TypeRef = "number"
                }
            });
            decision.DecisionTable.Inputs.Add(new DecisionTableInput()
            {
                InputExpression = new InputExpression()
                {
                    Text = "person.name",
                    TypeRef = "string"
                }
            });
            decision.DecisionTable.Inputs.Add(new DecisionTableInput()
            {
                InputExpression = new InputExpression()
                {
                    Text = "person.dateOfBirth",
                    TypeRef = "date"
                }
            });

            decision.DecisionTable.Outputs.Add(new DecisionTableOutput()
            {
                Name = "categoryOne",
                TypeRef = "number"
            });

            var rule1 = new DecisionTableRule();
            rule1.InputEntries.Add(new InputEntry()
            {
                Text = "18"
            });
            rule1.InputEntries.Add(new InputEntry()
            {
                Text = "\"Paul\""
            });
            rule1.InputEntries.Add(new InputEntry()
            {
                Text = string.Empty
            });
            rule1.OutputEntries.Add(new OutputEntry()
            {
                Text = "5"
            });
            decision.DecisionTable.Rules.Add(rule1);

            var rule2 = new DecisionTableRule();
            rule2.InputEntries.Add(new InputEntry()
            {
                Text = "> 18"
            });
            rule2.InputEntries.Add(new InputEntry()
            {
                Text = "\"Peter\""
            });
            rule2.InputEntries.Add(new InputEntry()
            {
                Text = string.Empty
            });
            rule2.OutputEntries.Add(new OutputEntry()
            {
                Text = "3"
            });
            decision.DecisionTable.Rules.Add(rule2);

            var rule3 = new DecisionTableRule();
            rule3.InputEntries.Add(new InputEntry()
            {
                Text = string.Empty
            });
            rule3.InputEntries.Add(new InputEntry()
            {
                Text = string.Empty
            });
            rule3.InputEntries.Add(new InputEntry()
            {
                Text = "> date(\"2000 - 01 - 01\")"
            });
            rule3.OutputEntries.Add(new OutputEntry()
            {
                Text = "1"
            });
            decision.DecisionTable.Rules.Add(rule3);

            var rule4 = new DecisionTableRule();
            rule4.InputEntries.Add(new InputEntry()
            {
                Text = string.Empty
            });
            rule4.InputEntries.Add(new InputEntry()
            {
                Text = string.Empty
            });
            rule4.InputEntries.Add(new InputEntry()
            {
                Text = string.Empty
            });
            rule4.OutputEntries.Add(new OutputEntry()
            {
                Text = "0"
            });
            decision.DecisionTable.Rules.Add(rule4);

            ruleTask.BusinessRule.Decisions.Add(decision);

            BusinessRuleTaskConverter ruleConverter = new BusinessRuleTaskConverter(ruleTask, null);
            foreach (var decisionRC in ruleConverter.BusinessRuleTaskElement.BusinessRule.Decisions)
            {
                var decisionConverter = new DecisionConverter(decisionRC);
                decisionConverter.ConvertElementLogic();
                ruleConverter.DecisionConverters.Add(decisionConverter);
            }
            string given = string.Empty;

            var components = new List<SolidityComponent>();
            foreach (var decisionConverter in ruleConverter.DecisionConverters)
            {
                components.AddRange(decisionConverter.GetGeneratedSolidityComponents());
            }
            foreach (var component in components)
            {
                given += $"{component.ToString()}\n";
            }

            string expected = "struct Decision_kowu89qOutput{\n" +
                            "\tint categoryOne;\n" +
                            "}\n" +
                            "\n" +
                            "function decision_kowu89q() internal view returns(Decision_kowu89qOutput memory){\n" +
                            "\tDecision_kowu89qOutput memory output;\n" +
                            "\toutput.categoryOne = 0;\n" +
                            "\tif(person.age == 18 && keccak256(abi.encodePacked(person.name)) == keccak256(abi.encodePacked(\"Paul\"))){\n" +
                            "\t\toutput.categoryOne++;\n" +
                            "\t}\n" +
                            "\tif(person.age > 18 && keccak256(abi.encodePacked(person.name)) == keccak256(abi.encodePacked(\"Peter\"))){\n" +
                            "\t\toutput.categoryOne++;\n" +
                            "\t}\n" +
                            "\tif(person.dateOfBirth > 20000101){\n" +
                            "\t\toutput.categoryOne++;\n" +
                            "\t}\n" +
                            "\toutput.categoryOne++;\n" +
                            "\treturn output;\n" +
                            "}\n\n";
            Assert.Equal(expected, given);
        }
    }
}
