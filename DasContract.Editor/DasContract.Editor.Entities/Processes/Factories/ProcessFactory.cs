using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DasContract.Editor.Entities.Processes.Factories.Exceptions;
using DasContract.Editor.Entities.Processes.Process;
using DasContract.Editor.Entities.Processes.Process.Activities;
using DasContract.Editor.Entities.Processes.Process.Events;
using DasContract.Editor.Entities.Processes.Process.Gateways;

namespace DasContract.Editor.Entities.Processes.Factories
{
    public static class ProcessFactory
    {
        public const string BPMNNS = "{http://www.omg.org/spec/BPMN/20100524/MODEL}";
        public const string CAMNS = "{http://camunda.org/schema/1.0/bpmn}";

        public static IEnumerable<ContractProcess> FromXML(string bpmnXml)
        {
            var xDoc = XDocument.Parse(bpmnXml);
            var processes = xDoc.Descendants(BPMNNS + "process").ToList();
            return processes.Select(p => FromProcessBPMN(p));
        }

        static ContractProcess FromProcessBPMN(XElement bpmnXElement)
        {
            var process = new ContractProcess();
            var processElements = bpmnXElement.Descendants().ToList();

            foreach (var element in processElements)
            {
                if (element.Name == BPMNNS + "sequenceFlow")
                {
                    var sequenceFlow = CreateSequenceFlow(element);
                    process.SequenceFlows.Add(sequenceFlow);
                }
                else
                {
                    var processElement = CreateProcessElement(element);
                    if (processElement != null)
                        process.ProcessElements.Add(processElement);
                }
            }
            return process;
        }

        static ContractSequenceFlow CreateSequenceFlow(XElement sequenceFlowXElement)
        {
            var sequenceFlow = new ContractSequenceFlow();

            var idAttr = sequenceFlowXElement.Attribute("id");
            var nameAtrr = sequenceFlowXElement.Attribute("name");
            var sourceIdAttr = sequenceFlowXElement.Attribute("sourceRef");
            var targetIdAttr = sequenceFlowXElement.Attribute("targetRef");

            if (idAttr != null)
                sequenceFlow.Id = idAttr.Value;
            else
                throw new InvalidContractProcessElementException("Id must be set on every element");

            if (sourceIdAttr != null)
                sequenceFlow.SourceId = sourceIdAttr.Value;
            else
                throw new InvalidContractProcessElementException("Sequence " + idAttr + " does not have a source");

            if (targetIdAttr != null)
                sequenceFlow.TargetId = targetIdAttr.Value;
            else
                throw new InvalidContractProcessElementException("Sequence " + idAttr + " does not have a target");

            if (nameAtrr != null)
                sequenceFlow.Name = nameAtrr.Value;

            sequenceFlow.Condition = GetSequenceFlowCondition(sequenceFlowXElement);
            return sequenceFlow;
        }

        static string GetSequenceFlowCondition(XElement sequenceFlowXElement)
        {
            var conditionDesc = sequenceFlowXElement.Descendants(BPMNNS + "conditionExpression").ToList();
            if (conditionDesc.Count == 0)
                return null;

            var conditionVal = conditionDesc.First().Value;
            return conditionVal;
        }

        static ContractProcessElement CreateProcessElement(XElement xElement)
        {
            ContractProcessElement processElement;

            switch (xElement.Name.ToString())
            {
                case BPMNNS + "businessRuleTask":
                    processElement = CreateBusinessRuleTask(xElement);
                    break;
                case BPMNNS + "scriptTask":
                    processElement = CreateScriptTask(xElement);
                    break;
                /*case BPMNNS + "serviceTask":
                    processElement = CreateServiceTask(xElement);
                    break;*/
                case BPMNNS + "userTask":
                    processElement = CreateUserTask(xElement);
                    break;
                case BPMNNS + "startEvent":
                    processElement = CreateStartEvent(xElement);
                    break;
                case BPMNNS + "endEvent":
                    processElement = CreateEndEvent(xElement);
                    break;
                case BPMNNS + "exclusiveGateway":
                    processElement = CreateExclusiveGateway(xElement);
                    break;
                case BPMNNS + "parallelGateway":
                    processElement = CreateParallelGateway(xElement);
                    break;
                default:
                    processElement = null;
                    break;
            }

            if (processElement != null)
            {
                processElement.Incoming = GetDescendantList(xElement, "incoming");
                processElement.Outgoing = GetDescendantList(xElement, "outgoing");
            }

            return processElement;
        }

        static List<string> GetDescendantList(XElement xElement, string descendantName)
        {
            var descendants = xElement.Descendants(BPMNNS + descendantName);
            var descendantList = new List<string>();

            foreach (var descendant in descendants)
                descendantList.Add(descendant.Value);
            return descendantList;
        }

        static ContractScriptActivity CreateScriptTask(XElement xElement)
        {
            var task = new ContractScriptActivity
            {
                Id = GetProcessId(xElement),
                Name = GetProcessName(xElement)
            };
            return task;
        }

        static ContractBusinessRuleActivity CreateBusinessRuleTask(XElement xElement)
        {
            var task = new ContractBusinessRuleActivity
            {
                Id = GetProcessId(xElement),
                Name = GetProcessName(xElement)
            };
            return task;
        }

        /*static ContractServiceTaskActivity CreateServiceTask(XElement xElement)
        {
            var task = new ServiceTask();
            task.Id = GetProcessId(xElement);
            task.Name = GetProcessName(xElement);
            return task;
        }*/

        static ContractUserActivity CreateUserTask(XElement xElement)
        {
            var task = new ContractUserActivity
            {
                Id = GetProcessId(xElement),
                Name = GetProcessName(xElement)
            };
            return task;
        }

        static ContractExclusiveGateway CreateExclusiveGateway(XElement xElement)
        {
            var gateway = new ContractExclusiveGateway
            {
                Id = GetProcessId(xElement),
                Name = GetProcessName(xElement)
            };
            return gateway;
        }

        static ContractParallelGateway CreateParallelGateway(XElement xElement)
        {
            var gateway = new ContractParallelGateway
            {
                Id = GetProcessId(xElement),
                Name = GetProcessName(xElement)
            };
            return gateway;
        }

        static ContractParallelGateway CreateParalellGateway(XElement xElement)
        {
            var gateway = new ContractParallelGateway
            {
                Id = GetProcessId(xElement),
                Name = GetProcessName(xElement)
            };

            return gateway;
        }

        static ContractEndEvent CreateEndEvent(XElement xElement)
        {
            var endEvent = new ContractEndEvent
            {
                Id = GetProcessId(xElement),
                Name = GetProcessName(xElement)
            };
            return endEvent;
        }

        static ContractStartEvent CreateStartEvent(XElement xElement)
        {
            var startEvent = new ContractStartEvent
            {
                Id = GetProcessId(xElement),
                Name = GetProcessName(xElement)
            };
            return startEvent;
        }

        static string GetProcessId(XElement xElement)
        {
            if (xElement.Attribute("id") == null)
                throw new InvalidContractProcessElementException("ID must be set on every element");

            return xElement.Attribute("id").Value;
        }

        static string GetProcessName(XElement xElement)
        {
            var nameAttribute = xElement.Attribute("name");
            if (nameAttribute != null)
                return nameAttribute.Value;
            return null;
        }
    }
}
