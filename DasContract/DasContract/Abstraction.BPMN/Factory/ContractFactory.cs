using DasContract.Abstraction.Entity;
using DasContract.Abstraction.Exceptions.Specific;
using DasContract.Abstraction.Processes;
using System;
using System.Linq;
using System.Xml.Linq;

namespace DasContract.Abstraction.BPMN.Factory
{
    public static class ContractFactory
    {
        /// <summary>
        /// Namespace of the PMN XML document
        /// </summary>
        public readonly static XNamespace BPMNNS = "http://www.omg.org/spec/BPMN/20100524/MODEL";

        public static Contract FromBPMN(string bpmnXml)
        {
            var xDoc = XDocument.Parse(bpmnXml);
            var contract = new Contract();
            var processes = xDoc.Descendants(BPMNNS + "process").ToList();
            if (processes.Count != 1)
                throw new InvalidProcessCountException("The number of proccesses defined in the model must be 1, not " + processes.Count);
            contract.Process = ProcessFactory.FromBPMN(processes.First());

            return contract; 
        }
    }
}
