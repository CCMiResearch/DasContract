using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    public abstract class HitPolicyConverter
    {
        protected string outputStructName = string.Empty;

        public abstract SolidityFunction CreateDecisionFunction(Decision decision);

        public virtual SolidityFunction CreateHelperFunction(Decision decision) { return null; }

        //Print declaration template of Solidity struct wrapping up output clauses.
        public virtual SolidityStruct CreateOutputStruct(Decision decision)
        {
            outputStructName = string.Concat(Regex.Replace(decision.Id, @" ", "").ToUpperCamelCase(), "Output");
            var outputStruct = new SolidityStruct(outputStructName);
            //Print struct's Solidity property based on its DMN modeller counterpart.
            foreach (var outputClause in decision.DecisionTable.Outputs)
            {
                outputStruct.AddToBody(new SolidityStatement($"{ConvertDecisionDataType(outputClause.TypeRef)} {outputClause.Name.ToLowerCamelCase()}"));
            }
            return outputStruct;
        }

        //Conversion of DMN modeller data types to Solidity data types
        protected string ConvertDecisionDataType(string dataType)
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

        //Get list of conditions clauses that will be inserted into the SolidityIfElse constructors
        protected List<string> GetAllConditions(Decision decision)
        {
            var conditions = new List<string>();
            foreach (var rule in decision.DecisionTable.Rules)
            {
                var conditionString = string.Empty;
                //Input's values and data types are in separate lists
                foreach (var inputEntry in rule.InputEntries.Select((value, i) => new { i, value }))
                {
                    //Skipp if value of boxed expression is empty
                    if (!string.IsNullOrEmpty(inputEntry.value.Text))
                    {
                        var inputDataType = decision.DecisionTable.Inputs[inputEntry.i].InputExpression.TypeRef;
                        var expression = decision.DecisionTable.Inputs[inputEntry.i].InputExpression.Text;
                        if (!string.IsNullOrEmpty(conditionString))
                            conditionString += $" && ";
                        conditionString += ConvertExpressionToCondition(expression, inputDataType, inputEntry.value.Text, true);
                    }
                }
                conditions.Add(conditionString);
            }
            return conditions;
        }

        protected string ConvertToSolidityValue(string value, string dataType)
        {
            if (dataType == "string" || dataType == "number" || dataType == "boolean")
            {
                return value;
            }
            else if (dataType == "dateTime")
            {
                var parsedDateTime = value.Split('\"');
                return DateTime.Parse(parsedDateTime[1]).ToString("yyyyMMddHHmmss");
            }
            else if (dataType == "date")
            {
                var parsedDateTime = value.Split('\"');
                return DateTime.Parse(parsedDateTime[1]).ToString("yyyyMMdd");
            }
            else if (dataType == "time")
            {
                var parsedDateTime = value.Split('\"');
                return DateTime.Parse(parsedDateTime[1]).ToString("HHmmss");
            }
            else
            {
                throw new Exception($"Invalid Data Type: {dataType} of an Output Clause.");
            }
        }

        //Return condition in string format based on the data type of the input
        protected string ConvertExpressionToCondition(string expression, string dataType, string entry, bool equalityComparison)
        {
            string comparisonSign;
            if (equalityComparison)
                comparisonSign = "==";
            else
                comparisonSign = "!=";
            string condition;
            //String comparison
            //Strings are compared in Solidity using hash values
            if (dataType == "string")
            {
                condition = $"keccak256(abi.encodePacked({expression})) {comparisonSign} keccak256(abi.encodePacked({entry}))";
            }
            //Integer comparison
            else if (dataType == "number")
            {
                if (entry.Contains("<") || entry.Contains(">"))
                {
                    //Insert whitespace between equaility symbol and number value
                    condition = $"{expression} {Regex.Replace(entry, @"([<>=])(\d)", "$1 $2")}";
                }
                else
                {
                    condition = $"{expression} {comparisonSign} {entry}";
                }
            }
            //Boolean comparison
            else if (dataType == "boolean")
            {
                condition = $"{expression} {comparisonSign} {entry}";
            }
            //DateTime comparison
            else if (dataType == "dateTime")
            {
                var splitCondition = DateTimeSeparateEqualityCharacters(entry);
                var parsedDateTime = splitCondition[1].Split('\"');
                if (!equalityComparison)
                    splitCondition[0] = comparisonSign;
                condition = $"{expression} {splitCondition[0]} {DateTime.Parse(parsedDateTime[1]).ToString("yyyyMMddHHmmss")}";
            }
            //Date comparison
            else if (dataType == "date")
            {
                var splitCondition = DateTimeSeparateEqualityCharacters(entry);
                var parsedDateTime = splitCondition[1].Split('\"');
                if (!equalityComparison)
                    splitCondition[0] = comparisonSign;
                condition = $"{expression} {splitCondition[0]} {DateTime.Parse(parsedDateTime[1]).ToString("yyyyMMdd")}";
            }
            //Time comparison
            else if (dataType == "time")
            {
                var splitCondition = DateTimeSeparateEqualityCharacters(entry);
                var parsedDateTime = splitCondition[1].Split('\"');
                if (!equalityComparison)
                    splitCondition[0] = comparisonSign;
                condition = $"{expression} {splitCondition[0]} {DateTime.Parse(parsedDateTime[1]).ToString("HHmmss")}";
            }
            else
            {
                throw new Exception($"Invalid Data Type: {dataType} of an Output Clause.");
            }
            return condition;
        }

        //Helper function returning separated equaility sign symbols and actual value
        protected List<string> DateTimeSeparateEqualityCharacters(string inputCondition)
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
    }
}
