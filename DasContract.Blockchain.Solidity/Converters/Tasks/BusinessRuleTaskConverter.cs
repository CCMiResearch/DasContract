using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Dmn;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.Tasks
{
    public class BusinessRuleTaskConverter : TaskConverter
    {
        BusinessRuleTask businessRuleTaskElement;

        SolidityStruct outputStructure = null;

        SolidityFunction mainFunction = null;

        SolidityFunction decisionFunction = null;

        SolidityFunction helperFunction = null;

        public BusinessRuleTaskConverter(BusinessRuleTask businessRuleTaskElement, ProcessConverter converterService)
        {
            this.businessRuleTaskElement = businessRuleTaskElement;
            processConverter = converterService;
        }

        public override void ConvertElementLogic()
        {
            outputStructure = CreateOutputStruct();
            decisionFunction = CreateDecisionFunction();
            mainFunction = CreateTaskFunction();
        }

        public override IList<SolidityComponent> GetGeneratedSolidityComponents()
        {
            var components = new List<SolidityComponent>{
                outputStructure,
                decisionFunction,
                mainFunction
            };
            if (helperFunction != null)
                components.Add(helperFunction);
            return components;
        }

        //Print declaration template of Solidity struct wrapping up output clauses.
        private SolidityStruct CreateOutputStruct()
        {
            var decision = businessRuleTaskElement.BusinessRule.Decisions.Find(d => !String.IsNullOrEmpty(d.DecisionTable.Id));
            var outputStructName = String.Concat(Regex.Replace(decision.Id, @" ", "").ToUpperCamelCase(), "Output");
            var outputStruct = new SolidityStruct(outputStructName);
            //Print struct's Solidity property based on its DMN modeller counterpart.
            foreach (var outputClause in decision.DecisionTable.Outputs)
            {   
                outputStruct.AddToBody(new SolidityStatement($"{ConvertDecisionDataType(outputClause.TypeRef)} {outputClause.Name.ToLowerCamelCase()}"));
            }
            return outputStruct;
        }

        private SolidityFunction CreateDecisionFunction()
        {
            var decision = businessRuleTaskElement.BusinessRule.Decisions.Find(d => !String.IsNullOrEmpty(d.DecisionTable.Id));
            var functionName = Regex.Replace(decision.Id, @" ", "").ToLowerCamelCase();
            SolidityFunction function = new SolidityFunction(functionName, SolidityVisibility.Internal, "int", true);
            //RECOGNITION OF HIT POLICY
            var hitPolicy = decision.DecisionTable.HitPolicy;
            var aggregation = decision.DecisionTable.Aggregation;
            if (String.IsNullOrEmpty(hitPolicy))
                AppendUniqieHitPolicyBody(function, decision);
            else if (hitPolicy == "ANY")
                AppendAnyHitPolicyBody(function, decision);
            else if (hitPolicy == "PRIORITY")
                AppendPriorityHitPolicyBody(function, decision);
            else if (hitPolicy == "FIRST")
                AppendFirstHitPolicyBody(function, decision);
            else if (hitPolicy == "OUTPUT ORDER")
                AppendOutputOrderHitPolicyBody(function, decision);
            else if (hitPolicy == "RULE ORDER")
                AppendRuleOrderHitPolicyBody(function, decision);
            else if (hitPolicy == "COLLECT" && String.IsNullOrEmpty(aggregation))
                AppendRuleOrderHitPolicyBody(function, decision);
            else if (hitPolicy == "COLLECT" && aggregation == "SUM")
                AppendCollectSumHitPolicyBody(function, decision);
            else if (hitPolicy == "COLLECT" && aggregation == "MIN")
                AppendCollectMinHitPolicyBody(function, decision);
            else if (hitPolicy == "COLLECT" && aggregation == "MAX")
                AppendCollectMaxHitPolicyBody(function, decision);
            else if (hitPolicy == "COLLECT" && aggregation == "COUNT")
                AppendCollectCountHitPolicyBody(function, decision);
            else
                throw new Exception($"Invalid Hit Policy or Aggregation of Decision Table id: {decision.DecisionTable.Id}, hitPolicy: {hitPolicy}, aggregation: {aggregation}.");
            return function;
        }

        private SolidityFunction CreateTaskFunction()//TODOTODO
        {
            SolidityFunction function = new SolidityFunction(GetElementCallName(), SolidityVisibility.Internal);
            function.AddParameters(processConverter.GetIdentifiersAsParameters());
            //Add the script logic
            function.AddToBody(new SolidityStatement("Hello its me", false));
            //function.AddToBody(new SolidityStatement(scriptTaskElement.Script, false));
            //Get the delegation logic of the next connected element
            function.AddToBody(processConverter.GetStatementOfNextElement(businessRuleTaskElement));
            return function;
        }

        //Logic for different hit policies and helper functions

        private SolidityFunction AppendUniqieHitPolicyBody(SolidityFunction function, Decision decision)
        {
            return function;
        }

        private SolidityFunction AppendAnyHitPolicyBody(SolidityFunction function, Decision decision)
        {
            return function;
        }

        private SolidityFunction AppendPriorityHitPolicyBody(SolidityFunction function, Decision decision)
        {
            return function;
        }

        private SolidityFunction AppendFirstHitPolicyBody(SolidityFunction function, Decision decision)
        {
            return function;
        }

        private SolidityFunction AppendOutputOrderHitPolicyBody(SolidityFunction function, Decision decision)
        {
            return function;
        }

        private SolidityFunction AppendRuleOrderHitPolicyBody(SolidityFunction function, Decision decision)
        {
            return function;
        }

        private SolidityFunction AppendCollectSumHitPolicyBody(SolidityFunction function, Decision decision)
        {
            return function;
        }

        private SolidityFunction AppendCollectMinHitPolicyBody(SolidityFunction function, Decision decision)
        {
            return function;
        }

        private SolidityFunction AppendCollectMaxHitPolicyBody(SolidityFunction function, Decision decision)
        {
            return function;
        }

        private SolidityFunction AppendCollectCountHitPolicyBody(SolidityFunction function, Decision decision)
        {
            var rules = GetAllConditions(decision);
            function.AddToBody(new SolidityStatement("int count = 0", true));
            foreach (var rule in rules)
            {
                if (String.IsNullOrEmpty(rule))
                {
                    function.AddToBody(new SolidityStatement("count++", true));
                }
                else
                {
                    var condition = new SolidityIfElse();
                    condition.AddConditionBlock(rule, new SolidityStatement("count++", true));
                    function.AddToBody(condition);
                }
            }
            function.AddToBody(new SolidityStatement("return count", true));
            return function;
        }

        //Helpers

        //Get list of conditions clauses that will be inserted into the SolidityIfElse constructors
        private List<string> GetAllConditions(Decision decision)
        {
            var conditions = new List<string>();
            foreach (var rule in decision.DecisionTable.Rules)
            {
                var conditionString = String.Empty;
                //Input's values and data types are in separate lists
                foreach (var inputEntry in rule.InputEntries.Select((value, i) => new { i, value }))
                {
                    //Skipp if value of boxed expression is empty
                    if (!String.IsNullOrEmpty(inputEntry.value.Text))
                    {
                        var inputDataType = decision.DecisionTable.Inputs[inputEntry.i].InputExpression.TypeRef;
                        var inputExpression = decision.DecisionTable.Inputs[inputEntry.i].InputExpression.Text;
                        if (!String.IsNullOrEmpty(conditionString))
                            conditionString += $" && ";
                        conditionString += ConvertExpressionToCondition(inputExpression, inputDataType, inputEntry.value.Text);
                    }
                }
                conditions.Add(conditionString);
            }
            return conditions;
        }

        //Conversion of DMN modeller data types to Solidity data types
        private string ConvertDecisionDataType(string dataType)
        {
            if (dataType == "string")
                return "string memory";
            else if (dataType == "number")
                return "int";
            else if (dataType == "boolean")
                return "bool";
            //dateTime data type can be compared in solidity as unsigned integer
            else if (dataType == "dateTime" || dataType == "date" || dataType == "time")
                return "uint256";
            else
                throw new Exception($"Invalid Data Type: {dataType} of an Output Clause.");
        }

        //Return condition in string format based on the data type of the input
        private string ConvertExpressionToCondition(string inputExpression, string dataType, string inputCondition)
        {
            string condition;
            //String comparison
            //Strings are compared in Solidity using hash values
            if (dataType == "string")
                condition = $"keccak256(abi.encodePacked({inputExpression})) == keccak256(abi.encodePacked({inputCondition}))";
            //Integer comparison
            else if (dataType == "number")
                if (inputCondition.Contains("<") || inputCondition.Contains(">"))
                    //Insert whitespace between equaility symbol and number value
                    condition = $"{inputExpression} {Regex.Replace(inputCondition, @"([<>=])(\d)", "$1 $2")}";
                else
                    condition = $"{inputExpression} == {inputCondition}";
            //Boolean comparison
            else if (dataType == "boolean")
                condition = $"{inputExpression} == {inputCondition}";
            //DateTime comparison
            else if (dataType == "dateTime")
            {
                var splitCondition = DateTimeSeparateEqualityCharacters(inputCondition);
                var parsedDateTime = splitCondition[1].Split('\"');
                condition = $"{inputExpression} {splitCondition[0]} {DateTime.Parse(parsedDateTime[1]).ToString("yyyyMMddHHmmss")}";
            }
            //Date comparison
            else if (dataType == "date")
            {
                var splitCondition = DateTimeSeparateEqualityCharacters(inputCondition);
                var parsedDateTime = splitCondition[1].Split('\"');
                condition = $"{inputExpression} {splitCondition[0]} {DateTime.Parse(parsedDateTime[1]).ToString("yyyyMMdd")}";
            }
            //Time comparison
            else if (dataType == "time")
            {
                var splitCondition = DateTimeSeparateEqualityCharacters(inputCondition);
                var parsedDateTime = splitCondition[1].Split('\"');
                condition = $"{inputExpression} {splitCondition[0]} {DateTime.Parse(parsedDateTime[1]).ToString("HHmmss")}";
            }
            else
                throw new Exception($"Invalid Data Type: {dataType} of an Output Clause.");
            return condition;
        }

        //Helper function returning separated equaility sign symbols and actual value
        private List<string> DateTimeSeparateEqualityCharacters(string inputCondition)
        {
            List<string> signWithValue = new List<string>();
            if (inputCondition.Contains(">="))
            {
                signWithValue.Add(">=");
                var splitString = inputCondition.Split('=');
                signWithValue.Add(splitString[1]);
            }
            else if (inputCondition.Contains("<="))
            {
                signWithValue.Add("<=");
                var splitString = inputCondition.Split('=');
                signWithValue.Add(splitString[1]);
            }
            else if (inputCondition.Contains(">"))
            {
                signWithValue.Add(">");
                var splitString = inputCondition.Split('>');
                signWithValue.Add(splitString[1]);
            }
            else if (inputCondition.Contains("<"))
            {
                signWithValue.Add("<");
                var splitString = inputCondition.Split('<');
                signWithValue.Add(splitString[1]);
            }
            //Equality check is done when the value is without the equaility symbol
            else
            {
                signWithValue.Add("==");
                signWithValue.Add(inputCondition);
            }
            return signWithValue;
        }

        //TMP OTHER

        public override string GetElementId()
        {
            return businessRuleTaskElement.Id;
        }

        public override string GetElementCallName()
        {
            return GetElementCallName(businessRuleTaskElement);
        }

        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            SolidityStatement statement = new SolidityStatement();
            statement.Add(GetFunctionCallStatement());
            return statement;
        }
    }
}
