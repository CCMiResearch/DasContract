using System;
using System.Collections.Generic;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Blockchain.Solidity.SolidityComponents;

namespace DasContract.Blockchain.Solidity.Converters.Events
{
    public class StartEventConverter : ElementConverter
    {
        StartEvent startEventElement;

        SolidityConstructor constructor;

        public override void ConvertElementLogic()
        {
            if(processConverter.IsRootProcess)
                constructor = CreateConstructor();
        }

        private SolidityConstructor CreateConstructor()
        {
            SolidityConstructor constructor = new SolidityConstructor();
            constructor.AddToBody(processConverter.GetStatementOfNextElement(startEventElement));
            return constructor;
        }

        public StartEventConverter(StartEvent startEventElement, ProcessConverter converterService)
        {
            this.startEventElement = startEventElement;
            processConverter = converterService;
        }
        public override IList<SolidityComponent> GetGeneratedSolidityComponents()
        {
            var solidityComponents = new List<SolidityComponent>();
            if (processConverter.IsRootProcess)
                solidityComponents.Add(constructor);
            return solidityComponents;
        }

        public override string GetElementId()
        {
            return startEventElement.Id;
        }

        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            throw new NotImplementedException();
        }

        public override string GetElementCallName()
        {
            return GetElementCallName(startEventElement);
        }
    }
}
