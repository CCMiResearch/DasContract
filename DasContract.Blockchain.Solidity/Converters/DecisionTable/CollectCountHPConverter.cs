using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    //Collect Hit Policy with Count Aggregation - Tthe result of the decision table is the number of outputs.
    public class CollectCountHPConverter : HitPolicyConverter
    {
        public override string CreateOutputDeclaration()
        {
            FunctionName = Regex.Replace(Decision.Id, @" ", "").ToLowerCamelCase();
            OutputStructName = "int";
            return $"{OutputStructName} {FunctionName}Output";
        }

        public override SolidityFunction CreateDecisionFunction()
        {
            //Define function's header
            FunctionName = Regex.Replace(Decision.Id, @" ", "").ToLowerCamelCase();
            SolidityFunction function = new SolidityFunction(FunctionName, SolidityVisibility.Internal, "int", true);
            //Add declaration of helper varaibles
            function.AddToBody(new SolidityStatement("int count = 0", true));

            //For each row representing rule create condition for if statement
            var conditionBody = "count++"; 
            var rules = GetAllConditions(); 
            foreach (var rule in rules)
            {
                function.AddToBody(AddBodyBasedOnRule(rule, conditionBody));
            }

            //Add the rest of the function
            function.AddToBody(new SolidityStatement("return count", true));
            return function;
        }

        //Collect Count Hit Policy is the only one that does not return any output struct
        public override SolidityStruct CreateOutputStruct() 
        {
            OutputStructName = "int";
            return null; 
        }
    }
}
