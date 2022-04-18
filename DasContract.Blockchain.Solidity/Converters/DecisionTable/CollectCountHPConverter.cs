using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    public class CollectCountHPConverter : HitPolicyConverter
    {
        public override SolidityFunction CreateDecisionFunction(Decision decision)
        {
            //Define function's header
            var functionName = Regex.Replace(decision.Id, @" ", "").ToLowerCamelCase();
            SolidityFunction function = new SolidityFunction(functionName, SolidityVisibility.Internal, "int", true);
            //Add declaration of helper varaibles
            function.AddToBody(new SolidityStatement("int count = 0", true));

            //For each row representing rule create condition for if statement
            var conditionBody = "count++"; 
            var rules = GetAllConditions(decision); 
            foreach (var rule in rules)
            {
                //If the row is empty then do not put the logic into conditional statement
                if (String.IsNullOrEmpty(rule))
                {
                    function.AddToBody(new SolidityStatement(conditionBody, true));
                }
                else
                {
                    var condition = new SolidityIfElse();
                    condition.AddConditionBlock(rule, new SolidityStatement(conditionBody, true));
                    function.AddToBody(condition);
                }
            }

            //Add the rest of the function
            function.AddToBody(new SolidityStatement("return count", true));
            return function;
        }

        //Collect Count Hit Policy is the only one that does not return any output struct
        public override SolidityStruct CreateOutputStruct(Decision decision) { return null; }
    }
}
