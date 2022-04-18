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
            var hitPolicy = GetHitPolicyConverter();
            var decision = businessRuleTaskElement.BusinessRule.Decisions.Find(d => !String.IsNullOrEmpty(d.DecisionTable.Id));
            outputStructure = hitPolicy.CreateOutputStruct(decision);
            decisionFunction = hitPolicy.CreateDecisionFunction(decision);
            helperFunction = hitPolicy.CreateHelperFunction(decision);
            mainFunction = CreateTaskFunction();
        }

        private HitPolicyConverter GetHitPolicyConverter()
        {
            var decision = businessRuleTaskElement.BusinessRule.Decisions.Find(d => !String.IsNullOrEmpty(d.DecisionTable.Id));
            //RECOGNITION OF HIT POLICY
            var hitPolicy = decision.DecisionTable.HitPolicy;
            var aggregation = decision.DecisionTable.Aggregation;
            if (String.IsNullOrEmpty(hitPolicy))
               return new UniqueHPConverter();
            else if (hitPolicy == "ANY")
               return new AnyHPConverter();
            else if (hitPolicy == "PRIORITY")
               return new PriorityHPConverter();
            else if (hitPolicy == "FIRST")
               return new FirstHPConverter();
            else if (hitPolicy == "OUTPUT ORDER")
               return new OutputOrderHPConverter();
            else if (hitPolicy == "RULE ORDER")
               return new RuleOrderHPConverter();
            else if (hitPolicy == "COLLECT" && String.IsNullOrEmpty(aggregation))
               return new RuleOrderHPConverter();
            else if (hitPolicy == "COLLECT" && aggregation == "SUM")
               return new CollectSumHPConverter();
            else if (hitPolicy == "COLLECT" && aggregation == "MIN")
               return new CollectMinHPConverter();
            else if (hitPolicy == "COLLECT" && aggregation == "MAX")
               return new CollectMaxHPConverter();
            else if (hitPolicy == "COLLECT" && aggregation == "COUNT")
               return new CollectCountHPConverter();
            else
               throw new Exception($"Invalid Hit Policy or Aggregation of Decision Table id: {decision.DecisionTable.Id}, hitPolicy: {hitPolicy}, aggregation: {aggregation}.");
        }

        public override IList<SolidityComponent> GetGeneratedSolidityComponents()
        {
            var components = new List<SolidityComponent>();
            if (outputStructure != null)
                components.Add(outputStructure);
            if (helperFunction != null)
                components.Add(helperFunction);
            components.Add(decisionFunction);
            components.Add(mainFunction);
            return components;
        }

        private SolidityFunction CreateTaskFunction()//TODOTODO
        {
            SolidityFunction function = new SolidityFunction(GetElementCallName(), SolidityVisibility.Internal);
            function.AddParameters(processConverter.GetIdentifiersAsParameters());
            //Add the script logic
            function.AddToBody(new SolidityStatement("DummyBody", false));
            //function.AddToBody(new SolidityStatement(scriptTaskElement.Script, false));
            //Get the delegation logic of the next connected element
            function.AddToBody(processConverter.GetStatementOfNextElement(businessRuleTaskElement));
            return function;
        }

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
