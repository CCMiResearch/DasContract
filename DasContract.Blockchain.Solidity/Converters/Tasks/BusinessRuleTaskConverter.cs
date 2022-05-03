using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Dmn;
using DasContract.Abstraction.Processes.Dmn.Diagram;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Blockchain.Solidity.Converters.DecisionTable;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity.Converters.Tasks
{
    public class BusinessRuleTaskConverter : TaskConverter
    {
        public BusinessRuleTask BusinessRuleTaskElement { get; set; } = null;

        SolidityFunction MainFunction { get; set; } = null;

        public List<DecisionConverter> DecisionConverters { get; set; } = new List<DecisionConverter>();

        public BusinessRuleTaskConverter(BusinessRuleTask businessRuleTaskElement, ProcessConverter converterService)
        {
            BusinessRuleTaskElement = businessRuleTaskElement;
            processConverter = converterService;
        }

        public override void ConvertElementLogic()
        {
            foreach (var decision in BusinessRuleTaskElement.BusinessRule.Decisions)
            {
                var decisionConverter = new DecisionConverter(decision);
                decisionConverter.ConvertElementLogic();
                DecisionConverters.Add(decisionConverter);
            }
            MainFunction = CreateTaskFunction();
        }

        public override IList<SolidityComponent> GetGeneratedSolidityComponents()
        {
            var components = new List<SolidityComponent>();
            foreach (var decisionConverter in DecisionConverters)
            {
                components.AddRange(decisionConverter.GetGeneratedSolidityComponents());
            }
            components.Add(MainFunction);
            return components;
        }

        private bool CheckInformationRequirements(IList<InformationRequirement> requirements, IList<string> declaredDecisions)
        {
            foreach (var requirement in requirements)
            {
                //Get the source node without the '#' char
                var hrefID = requirement.RequiredDecision.Href.Remove(0, 1);
                //Check if its source table was already declared
                if (!declaredDecisions.Contains(hrefID))
                    return false;
            }
            //The decision function can be declared
            return true;
        }

        private IList<SolidityStatement> GetDecisionFunctionDeclarations()
        {
            var solidityStatements = new List<SolidityStatement>();
            var decisions = new List<Decision>(BusinessRuleTaskElement.BusinessRule.Decisions);
            var declaredDecisions = new List<Decision>();
            var lastCount = 0;
            //Do until all statements are declared
            while (decisions.Count != 0)
            {
                //Check if there is cycle in the DRD
                if (decisions.Count == lastCount)
                    throw new Exception($"The DRD is incorrect!");
                lastCount = decisions.Count;
                //Get decisions that can be declared
                var declared = decisions.Where(decision => CheckInformationRequirements(decision.InformationRequirements, declaredDecisions.Select(x => x.Id).ToList())).ToList();
                //Move them to declared decisions
                declared.ForEach(decision => decisions.Remove(decision));
                declaredDecisions.AddRange(declared);
            }

            foreach (var declaration in declaredDecisions)
            {
                var functionName = Regex.Replace(declaration.Id, @" ", "").ToLowerCamelCase();
                var decision = DecisionConverters.Single(d => d.Decision.Id == declaration.Id);
                solidityStatements.Add(new SolidityStatement($"{decision.OutputDeclaration} = {functionName}()", true));
                solidityStatements.AddRange(decision.OutputAssignments);
            }

            return solidityStatements;
        }

        private SolidityFunction CreateTaskFunction()
        {
            SolidityFunction function = new SolidityFunction(GetElementCallName(), SolidityVisibility.Internal);
            function.AddParameters(processConverter.GetIdentifiersAsParameters());
            //Add the calls of the decision function in the right order
            var declarations = GetDecisionFunctionDeclarations();
            foreach (var declaration in declarations)
            {
                function.AddToBody(declaration);
            }
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
