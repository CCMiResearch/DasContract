using System.Collections.Generic;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Blockchain.Solidity.SolidityComponents;

namespace DasContract.Blockchain.Solidity.Converters.Tasks
{
    //TODO: State guard is unnecessary here, discuss whether to remove it
    public class ScriptTaskConverter : TaskConverter
    {
        ScriptTask scriptTaskElement;

        SolidityFunction mainFunction;
        //SolidityModifier stateGuard;

        public ScriptTaskConverter(ScriptTask scriptTaskElement, ProcessConverter converterService)
        {
            this.scriptTaskElement = scriptTaskElement;
            processConverter = converterService;
        }

        public override void ConvertElementLogic()
        {
            //Create main function containing the script logic 
            mainFunction = CreateTaskFunction();
            //Create the state guard modifier
            //stateGuard = ConversionTemplates.StateGuardModifier(GetElementCallName());
        }

        public override IList<SolidityComponent> GetGeneratedSolidityComponents()
        {
            return new List<SolidityComponent>
            {
                mainFunction
                //stateGuard
            };
        }

        private SolidityFunction CreateTaskFunction()
        {
            SolidityFunction function = new SolidityFunction(GetElementCallName(), SolidityVisibility.Internal);
            //Add state guard modifier
            //function.AddModifier(ConversionTemplates.StateGuardModifierName(GetElementCallName()));
            //Add a statement that disables the current active state
            SolidityStatement disableFunctionStatement = ConversionTemplates.ChangeActiveStateStatement(GetElementCallName(), false);
            function.AddToBody(disableFunctionStatement);
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
            List<string> statements = new List<string>();
            statements.Add("ActiveStates[\"" + GetElementCallName() + "\"] = true");
            statements.Add(GetElementCallName() + "()");
            return new SolidityStatement(statements);
        }

    }
}
