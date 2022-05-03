using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Dmn;
using DasContract.Blockchain.Solidity.Converters.Tasks;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.Converters.DecisionTable
{
    public class DecisionConverter : ElementConverter
    {
        public Decision Decision { get; set; } = null;

        public string OutputDeclaration { get; set; } = null;

        public IList<SolidityStatement> OutputAssignments { get; set; } = new List<SolidityStatement>();

        SolidityStruct OutputStructure { get; set; } = null;

        SolidityFunction DecisionFunction { get; set; } = null;

        SolidityFunction HelperFunction { get; set; } = null;

        HitPolicyConverter HitPolicy { get; set; } = null;

        public DecisionConverter(Decision decision, ProcessConverter converterService)
        {
            Decision = decision;
            processConverter = converterService;
        }

        public override void ConvertElementLogic()
        {
            HitPolicy = GetHitPolicyConverter();
            HitPolicy.Decision = Decision;
            OutputStructure = HitPolicy.CreateOutputStruct();
            OutputDeclaration = HitPolicy.CreateOutputDeclaration();
            DecisionFunction = HitPolicy.CreateDecisionFunction();
            HelperFunction = HitPolicy.CreateHelperFunction();
            OutputAssignments = GetOutputAssignments();
        }

        //Recognition of Hit Policy
        private HitPolicyConverter GetHitPolicyConverter()
        {
            var hitPolicy = Decision.DecisionTable.HitPolicy;
            var aggregation = Decision.DecisionTable.Aggregation;
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
                throw new Exception($"Invalid Hit Policy or Aggregation of Decision Table id: {Decision.DecisionTable.Id}, hitPolicy: {hitPolicy}, aggregation: {aggregation}.");
        }

        public override IList<SolidityComponent> GetGeneratedSolidityComponents()
        {
            var components = new List<SolidityComponent>();
            if (OutputStructure != null)
                components.Add(OutputStructure);
            if (HelperFunction != null)
                components.Add(HelperFunction);
            components.Add(DecisionFunction);
            return components;
        }

        public IList<SolidityStatement> GetOutputAssignments()
        {
            return HitPolicy.GetOutputAssignments();
        }

        public override string GetElementId()
        {
            return Decision.Id;
        }

        public override string GetElementCallName()
        {
            return Decision.Name;
        }

        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            SolidityStatement statement = new SolidityStatement();
            statement.Add(GetFunctionCallStatement());
            return statement;
        }
    }
}
