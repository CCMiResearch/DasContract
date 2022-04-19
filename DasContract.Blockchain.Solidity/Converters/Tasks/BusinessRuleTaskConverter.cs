using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Blockchain.Solidity.Converters.DecisionTable;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;

namespace DasContract.Blockchain.Solidity.Converters.Tasks
{
    public class BusinessRuleTaskConverter : TaskConverter
    {
        BusinessRuleTask BusinessRuleTaskElement { get; set; } = null;

        SolidityStruct OutputStructure { get; set; } = null;

        SolidityStatement OutputDeclaration { get; set; } = null;

        SolidityFunction MainFunction { get; set; } = null;

        SolidityFunction DecisionFunction { get; set; } = null;

        SolidityFunction HelperFunction { get; set; } = null;

        HitPolicyConverter HitPolicy { get; set; } = null;

        public BusinessRuleTaskConverter(BusinessRuleTask businessRuleTaskElement, ProcessConverter converterService)
        {
            BusinessRuleTaskElement = businessRuleTaskElement;
            processConverter = converterService;
        }

        public override void ConvertElementLogic()
        {
            HitPolicy = GetHitPolicyConverter();
            var decision = BusinessRuleTaskElement.BusinessRule.Decisions.Find(d => !string.IsNullOrEmpty(d.DecisionTable.Id));
            HitPolicy.Decision = decision;
            OutputStructure = HitPolicy.CreateOutputStruct();
            OutputDeclaration = HitPolicy.CreateOutputDeclaration();
            DecisionFunction = HitPolicy.CreateDecisionFunction();
            HelperFunction = HitPolicy.CreateHelperFunction();
            MainFunction = CreateTaskFunction();
        }

        //Recognition of Hit Policy
        private HitPolicyConverter GetHitPolicyConverter()
        {
            var decision = BusinessRuleTaskElement.BusinessRule.Decisions.Find(d => !string.IsNullOrEmpty(d.DecisionTable.Id));
            var hitPolicy = decision.DecisionTable.HitPolicy;
            var aggregation = decision.DecisionTable.Aggregation;
            if (string.IsNullOrEmpty(hitPolicy))
               return new UniqueHPConverter();
            else if (hitPolicy == "ANY")
               return new AnyHPConverter();
            else if (hitPolicy == "PRIORITY")
               return new PriorityHPConverter(1);
            else if (hitPolicy == "FIRST")
               return new FirstHPConverter();
            else if (hitPolicy == "OUTPUT ORDER")
               return new OutputOrderHPConverter(1);
            else if (hitPolicy == "RULE ORDER")
               return new RuleOrderHPConverter(0);
            else if (hitPolicy == "COLLECT" && string.IsNullOrEmpty(aggregation))
               return new RuleOrderHPConverter(0);
            else if (hitPolicy == "COLLECT" && aggregation == "SUM")
               return new CollectSumHPConverter();
            else if (hitPolicy == "COLLECT" && aggregation == "MIN")
               return new CollectMaxMinHPConverter(false);
            else if (hitPolicy == "COLLECT" && aggregation == "MAX")
               return new CollectMaxMinHPConverter(true);
            else if (hitPolicy == "COLLECT" && aggregation == "COUNT")
               return new CollectCountHPConverter();
            else
               throw new Exception($"Invalid Hit Policy or Aggregation of Decision Table id: {decision.DecisionTable.Id}, hitPolicy: {hitPolicy}, aggregation: {aggregation}.");
        }

        public override IList<SolidityComponent> GetGeneratedSolidityComponents()
        {
            var components = new List<SolidityComponent>();
            if (OutputStructure != null)
                components.Add(OutputStructure);
            components.Add(OutputDeclaration);
            if (HelperFunction != null)
                components.Add(HelperFunction);
            components.Add(DecisionFunction);
            components.Add(MainFunction);
            return components;
        }

        private SolidityFunction CreateTaskFunction()
        {
            SolidityFunction function = new SolidityFunction(GetElementCallName(), SolidityVisibility.Internal);
            function.AddParameters(processConverter.GetIdentifiersAsParameters());
            //Add the call of the decision function
            function.AddToBody(new SolidityStatement($"{HitPolicy.FunctionName}_Output = {HitPolicy.FunctionName}()", true));
            //Get the delegation logic of the next connected element
            function.AddToBody(processConverter.GetStatementOfNextElement(BusinessRuleTaskElement));
            return function;
        }

        public override string GetElementId()
        {
            return BusinessRuleTaskElement.Id;
        }

        public override string GetElementCallName()
        {
            return GetElementCallName(BusinessRuleTaskElement);
        }

        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            SolidityStatement statement = new SolidityStatement();
            statement.Add(GetFunctionCallStatement());
            return statement;
        }
    }
}
