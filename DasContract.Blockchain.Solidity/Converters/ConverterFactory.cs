using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Gateways;
using DasContract.Abstraction.Processes.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasToSolidity.Solidity.ConversionHelpers
{
    class ConverterFactory
    {
        public static ElementConverter CreateConverter(ProcessElement element)
        {
            var elementType = element.GetType();

            if (elementType == typeof(UserTask))
                return new UserTaskConverter((UserTask)element);
            else if (elementType == typeof(ScriptTask))
                return new ScriptTaskConverter((ScriptTask)element);
            else if (elementType == typeof(EndEvent))
                return new EndEventConverter((EndEvent)element);
            else if (elementType == typeof(ExclusiveGateway))
                return new ExclusiveGatewayConverter((ExclusiveGateway)element);
            else if (elementType == typeof(ParallelGateway))
                return new ParallelGatewayConverter((ParallelGateway)element);
            else if (elementType == typeof(StartEvent))
                return new StartEventConverter((StartEvent)element);
            return null;
        }
    }
}
