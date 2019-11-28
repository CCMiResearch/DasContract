using DasContract.Abstraction.Exceptions;
using DasContract.Abstraction.Processes;
using System;
using System.Linq;
using System.Xml.Linq;

namespace DasContract.Abstraction
{
    public class ContractFactory
    {
        public static XNamespace BPMNNS = "http://www.omg.org/spec/BPMN/20100524/MODEL";
        public static Contract FromBpmn(string bpmnXml)
        {
            var xDoc = XDocument.Parse(bpmnXml);
            var contract = new Contract();
            var processes = xDoc.Descendants(BPMNNS + "process").ToList();
            if (processes.Count != 1)
                throw new InvalidProcessCountException("The number of proccesses defined in the model must be 1, not " + processes.Count);
            contract.Process = CreateProcess(processes.First());

            return contract; 
        }

        static Process CreateProcess(XElement processXElement)
        {
            return ProcessFactory.FromBPMN(processXElement);
        }
    }
}
