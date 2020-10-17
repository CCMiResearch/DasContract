using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Gateways;
using DasContract.Abstraction.Processes.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.Converters
{
    public static class ConverterFactory
    {
        public static ElementConverter CreateConverter(ProcessElement element, ProcessConverter processConverter)
        {
            var elementType = element.GetType();

            if (elementType == typeof(UserTask))
                return new UserTaskConverter((UserTask)element, processConverter);
            else if (elementType == typeof(ScriptTask))
                return new ScriptTaskConverter((ScriptTask)element, processConverter);
            else if (elementType == typeof(EndEvent))
                return new EndEventConverter((EndEvent)element, processConverter);
            else if (elementType == typeof(ExclusiveGateway))
                return new ExclusiveGatewayConverter((ExclusiveGateway)element, processConverter);
            else if (elementType == typeof(ParallelGateway))
                return new ParallelGatewayConverter((ParallelGateway)element, processConverter);
            else if (elementType == typeof(StartEvent))
                return new StartEventConverter((StartEvent)element, processConverter);
            else if (elementType == typeof(CallActivity))
                return new CallActivityConverter((CallActivity)element, processConverter);
            return null;
        }
    }
}
