using System.Collections.Generic;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Blockchain.Solidity.SolidityComponents;

namespace DasContract.Blockchain.Solidity.Converters.Tasks
{
    public class ScriptTaskConverter : TaskConverter
    {
        ScriptTask scriptTaskElement;

        SolidityFunction mainFunction;

        public ScriptTaskConverter(ScriptTask scriptTaskElement, ProcessConverter converterService)
        {
            this.scriptTaskElement = scriptTaskElement;
            processConverter = converterService;
        }

        public override void ConvertElementLogic()
        {
            mainFunction = CreateTaskFunction();
        }

        public override IList<SolidityComponent> GetGeneratedSolidityComponents()
        {
            return new List<SolidityComponent>
            {
                mainFunction
            };
        }

        private SolidityFunction CreateTaskFunction()
        {
            SolidityFunction function = new SolidityFunction(GetElementCallName(), SolidityVisibility.Internal);
            function.AddParameters(processConverter.GetIdentifiersAsParameters());
            //Add the script logic
            function.AddToBody(new SolidityStatement(scriptTaskElement.Script, false));
            //Get the delegation logic of the next connected element
            function.AddToBody(processConverter.GetStatementOfNextElement(scriptTaskElement));
            return function;
        }

        public override string GetElementId()
        {
            return scriptTaskElement.Id;
        }

        public override string GetElementCallName()
        {
            return GetElementCallName(scriptTaskElement);
        }

        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            SolidityStatement statement = new SolidityStatement();
            statement.Add(GetFunctionCallStatement());
            return statement;
        }

    }
}
