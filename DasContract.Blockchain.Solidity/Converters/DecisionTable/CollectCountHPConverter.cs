using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    //Collect Hit Policy with Count Aggregation - The result of the decision table is the number of outputs.
    public class CollectCountHPConverter : HitPolicyConverter
    {

        public override SolidityFunction CreateDecisionFunction()
        {
            //Define function's header
            FunctionName = Regex.Replace(Decision.Id, @" ", "").ToLowerCamelCase();
            SolidityFunction function = new SolidityFunction(FunctionName, SolidityVisibility.Internal, $"{OutputStructName} memory", true);
            //Add declaration of helper varaibles
            function.AddToBody(new SolidityStatement($"{OutputStructName} memory output", true));
            function.AddToBody(new SolidityStatement($"output.{ Decision.DecisionTable.Outputs[0].Name.Replace(".", "__")} = 0", true));

            //For each row representing rule create condition for if statement
            var conditionBody = $"output.{ Decision.DecisionTable.Outputs[0].Name.Replace(".", "__")}++;"; 
            var rules = GetAllConditions(); 
            foreach (var rule in rules)
            {
                function.AddToBody(AddBodyBasedOnRule(rule, conditionBody));
            }

            //Add the rest of the function
            function.AddToBody(new SolidityStatement("return output", true));
            return function;
        }
    }
}
