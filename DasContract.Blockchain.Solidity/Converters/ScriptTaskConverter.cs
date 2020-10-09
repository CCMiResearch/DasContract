using System.Collections.Generic;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Blockchain.Solidity.SolidityComponents;

namespace DasContract.Blockchain.Solidity.Converters
{
    class ScriptTaskConverter : ElementConverter
    {

        ScriptTask scriptTask;
        public ScriptTaskConverter(ScriptTask scriptTask)
        {
            this.scriptTask = scriptTask;
        }

        public override IList<SolidityComponent> GetElementCode(List<ElementConverter> nextElements, IList<SequenceFlow> outgoingSeqFlows, IList<SolidityStruct> dataModel = null)
        {
            SolidityFunction function = new SolidityFunction(GetTaskName(), SolidityVisibility.Internal);
            function.AddModifier("is" + GetTaskName() + "State");
            SolidityStatement disableFunctionStatement = new SolidityStatement(ProcessConverter.ACTIVE_STATES_NAME + "[\"" + GetTaskName() + "\"] = false");
            function.AddToBody(disableFunctionStatement);
            function.AddToBody(new SolidityStatement(scriptTask.Script, false));
            function.AddToBody(nextElements[0].GetStatementForPrevious(scriptTask));
            return new List<SolidityComponent> { CreateStateGuard(), function };
        }

        SolidityModifier CreateStateGuard()
        {
            SolidityModifier stateGuard = new SolidityModifier("is" + GetTaskName() + "State");
            SolidityStatement requireStatement = new SolidityStatement(
                "require(" + ProcessConverter.IS_STATE_ACTIVE_FUNCTION_NAME + "(\"" + GetTaskName() + "\")==true)");

            stateGuard.AddToBody(requireStatement);
            return stateGuard;
        }

        public override string GetElementId()
        {
            return scriptTask.Id;
        }

        string GetTaskName()
        {
            if (scriptTask.Name != null)
                return scriptTask.Name;
            return scriptTask.Id;
        }

        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            List<string> statements = new List<string>();
            statements.Add("ActiveStates[\"" + GetTaskName() + "\"] = true");
            statements.Add(GetTaskName() + "()");
            return new SolidityStatement(statements);
        }
    }
}
