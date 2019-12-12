using System;
using System.Collections.Generic;
using System.Text;
using BpmnToSolidity.SolidityConverter;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Tasks;

namespace BpmnToSolidity.Solidity.ConversionHelpers
{
    class ScriptTaskConverter : ElementConverter
    {

        ScriptTask scriptTask;
        public ScriptTaskConverter(ScriptTask scriptTask)
        {
            this.scriptTask = scriptTask;
        }

        public override IList<SolidityComponent> GetElementCode(List<ElementConverter> nextElements, IList<SequenceFlow> outgoingSeqFlows)
        {
            SolidityFunction function = new SolidityFunction(GetTaskName(), SolidityVisibility.Internal);
            function.AddToBody(new SolidityStatement(scriptTask.Script));
            function.AddToBody(nextElements[0].GetStatementForPrevious());
            return new List<SolidityComponent> { function };
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

        public override SolidityStatement GetStatementForPrevious()
        {
            return new SolidityStatement(GetTaskName() + "()");
        }
    }
}
