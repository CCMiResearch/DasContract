using DasContract.Abstraction.Processes.Dmn;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Blockchain.Solidity.Converters.DecisionTable;
using DasContract.Blockchain.Solidity.Converters.Tasks;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System.Collections.Generic;
using Xunit;

namespace DasContract.Blockchain.Solidity.Tests.DMNBusinessRules
{
    public class CollectHitPolicyTest
    {
        [Fact]
        public void DecisionFunctionTest()
        {
            BusinessRuleTask ruleTask = new BusinessRuleTask();
            ruleTask.BusinessRule = new Definitions();

            var decision = new Decision();
            decision.Id = "decision_kowu89q";
            decision.DecisionTable.HitPolicy = "COLLECT";
            decision.DecisionTable.Aggregation = string.Empty;

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
            decision.DecisionTable.Outputs.Add(new DecisionTableOutput()
            {
                Name = "categoryTwo",
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
            rule1.OutputEntries.Add(new OutputEntry()
            {
                Text = "6"
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
            rule2.OutputEntries.Add(new OutputEntry()
            {
                Text = "4"
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
            rule3.OutputEntries.Add(new OutputEntry()
            {
                Text = "2"
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
                            "\tint categoryTwo;\n" +
                            "}\n" +
                            "\n" +
                            "function decision_kowu89q() internal view returns(Decision_kowu89qOutput[] memory){\n" +
                            "\tDecision_kowu89qOutput[4] memory priorities = [Decision_kowu89qOutput(5, 6), Decision_kowu89qOutput(3, 4), Decision_kowu89qOutput(1, 2), Decision_kowu89qOutput(0, 0)];\n" +
                            "\tbool[4] memory existsInOutput;\n" +
                            "\tuint outputSize = 0;\n" +
                            "\tbool matchedRule = false;\n" +
                            "\tif(person.age == 18 && keccak256(abi.encodePacked(person.name)) == keccak256(abi.encodePacked(\"Paul\"))){\n" +
                            "\t\tfor(uint256 i = 0; i < 4; i++){ \n" +
                            "\t\t\tif(!existsInOutput[i] && priorities[i].categoryOne == 5 && priorities[i].categoryTwo == 6){\n" +
                            "\t\t\t\texistsInOutput[i] = true;\n" +
                            "\t\t\t\toutputSize++;\n" +
                            "\t\t\t\tmatchedRule = true;\n" +
                            "\t\t\t\tbreak;\n" +
                            "\t\t\t}\n" +
                            "\t\t}\n" +
                            "\t}\n" +
                            "\tif(person.age > 18 && keccak256(abi.encodePacked(person.name)) == keccak256(abi.encodePacked(\"Peter\"))){\n" +
                            "\t\tfor(uint256 i = 0; i < 4; i++){ \n" +
                            "\t\t\tif(!existsInOutput[i] && priorities[i].categoryOne == 3 && priorities[i].categoryTwo == 4){\n" +
                            "\t\t\t\texistsInOutput[i] = true;\n" +
                            "\t\t\t\toutputSize++;\n" +
                            "\t\t\t\tmatchedRule = true;\n" +
                            "\t\t\t\tbreak;\n" +
                            "\t\t\t}\n" +
                            "\t\t}\n" +
                            "\t}\n" +
                            "\tif(person.dateOfBirth > 20000101){\n" +
                            "\t\tfor(uint256 i = 0; i < 4; i++){ \n" +
                            "\t\t\tif(!existsInOutput[i] && priorities[i].categoryOne == 1 && priorities[i].categoryTwo == 2){\n" +
                            "\t\t\t\texistsInOutput[i] = true;\n" +
                            "\t\t\t\toutputSize++;\n" +
                            "\t\t\t\tmatchedRule = true;\n" +
                            "\t\t\t\tbreak;\n" +
                            "\t\t\t}\n" +
                            "\t\t}\n" +
                            "\t}\n" +
                            "\tfor(uint256 i = 0; i < 4; i++){ \n" +
                            "\t\tif(!existsInOutput[i] && priorities[i].categoryOne == 0 && priorities[i].categoryTwo == 0){\n" +
                            "\t\t\texistsInOutput[i] = true;\n" +
                            "\t\t\toutputSize++;\n" +
                            "\t\t\tmatchedRule = true;\n" +
                            "\t\t\tbreak;\n" +
                            "\t\t}\n" +
                            "\t}\n" +
                            "\tuint j = 0;\n" +
                            "\tDecision_kowu89qOutput[] memory output = new Decision_kowu89qOutput[](outputSize);\n" +
                            "\tfor(uint256 i = 0; i < 4; i++){ \n" +
                            "\t\tif(existsInOutput[i]){\n" +
                            "\t\t\toutput[j] = Decision_kowu89qOutput(priorities[i].categoryOne, priorities[i].categoryTwo);\n" +
                            "\t\t\tj++;\n" +
                            "\t\t}\n" +
                            "\t}\n" +
                            "\tif(!matchedRule){\n" +
                            "\t\trevert('Undefined output');\n" +
                            "\t}\n" +
                            "\treturn output;\n" +
                            "}\n\n";
            Assert.Equal(expected, given);
        }
    }
}
