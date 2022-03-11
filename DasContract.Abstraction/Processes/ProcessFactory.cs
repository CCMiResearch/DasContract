using DasContract.Abstraction.Exceptions;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Gateways;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Abstraction.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes
{
    public class ProcessFactory
    {
        public const string BPMNNS = "{http://www.omg.org/spec/BPMN/20100524/MODEL}";
        public const string CAMNS = "{http://camunda.org/schema/1.0/bpmn}";
        public const string XMLNS = "{http://www.w3.org/2001/XMLSchema-instance}";

        public static Process FromDasFile(string processXml)
        {
            return FromDasFile(XElement.Parse(processXml));
        }

        public static Process FromDasFile(XElement bpmnXElement)
        {
            Process process = new Process();
            var processElements = bpmnXElement.Descendants().ToList();

            foreach (var e in processElements)
            {
                if (e.Name == "ContractSequenceFlow")
                {
                    var sequenceFlow = CreateSequenceFlow(e);
                    process.SequenceFlows.Add(sequenceFlow.Id, sequenceFlow);
                }
                else if (e.Name == "ContractProcessElement")
                {
                    var processElement = CreateProcessElement(e);
                    if (processElement != null)
                    {
                        process.ProcessElements.Add(processElement.Id, processElement);
                    }
                }
            }

            return process;
        }

        static SequenceFlow CreateSequenceFlow(XElement sequenceFlowXElement)
        {
            var sequenceFlow = new SequenceFlow();

            var idAttr = sequenceFlowXElement.Descendants("Id").FirstOrDefault();
            var nameAtrr = sequenceFlowXElement.Descendants("Name").FirstOrDefault();
            var sourceIdAttr = sequenceFlowXElement.Descendants("SourceId").FirstOrDefault();
            var targetIdAttr = sequenceFlowXElement.Descendants("TargetId").FirstOrDefault();

            if (idAttr != null)
                sequenceFlow.Id = idAttr.Value;
            else
                throw new InvalidElementException("ID must be set on every element");

            if (sourceIdAttr != null)
                sequenceFlow.SourceId = sourceIdAttr.Value;
            else
                throw new InvalidElementException("Sequence " + idAttr + " does not have a source");

            if (targetIdAttr != null)
                sequenceFlow.TargetId = targetIdAttr.Value;

            else
                throw new InvalidElementException("Sequence " + idAttr + " does not have a target");

            if (nameAtrr != null)
            {
                sequenceFlow.Name = nameAtrr.Value;
                sequenceFlow.Condition = nameAtrr.Value;
            }

            return sequenceFlow;
        }


        static ProcessElement CreateProcessElement(XElement xElement)
        {
            ProcessElement processElement;

            switch (xElement.Attribute(XMLNS + "type").Value)
            {
                case "ContractBusinessRuleTask":
                    processElement = CreateBusinessRuleTask(xElement);
                    break;
                case "ContractScriptActivity":
                    processElement = CreateScriptTask(xElement);
                    break;
                case "ContractServiceActivity":
                    processElement = CreateServiceTask(xElement);
                    break;
                case "ContractUserActivity":
                    processElement = CreateUserTask(xElement);
                    break;
                case "ContractStartEvent":
                    processElement = CreateStartEvent(xElement);
                    break;
                case "ContractEndEvent":
                    processElement = CreateEndEvent(xElement);
                    break;
                case "ContractExclusiveGateway":
                    processElement = CreateExclusiveGateway(xElement);
                    break;
                case "ContractParallelGateway":
                    processElement = CreateParallelGateway(xElement);
                    break;
                default:
                    processElement = null;
                    break;
            }

            if (processElement != null)
            {
                processElement.Incoming = GetDescendantList(xElement, "Incoming");
                processElement.Outgoing = GetDescendantList(xElement, "Outgoing");
            }

            return processElement;
        }

        /*
        static TaskInstanceType GetTaskInstanceType(XElement xElement)
        {
            
        }
        */

        static IList<string> GetDescendantList(XElement xElement, string descendantName)
        {
            var descendants = xElement.Descendants(descendantName).First().Descendants("string");
            IList<string> descendantList = new List<string>();

            foreach (var e in descendants)
            {
                descendantList.Add(e.Value);
            }
            return descendantList;
        }

        static ScriptTask CreateScriptTask(XElement xElement)
        {
            var task = new ScriptTask();
            task.Id = GetProcessId(xElement);
            task.Name = RemoveWhitespaces(GetProcessName(xElement));

            var scriptList = xElement.Descendants("Script").ToList();
            if (scriptList.Count == 1)
                task.Script = scriptList.First().Value;
            else
                throw new InvalidElementException("script task " + task.Id + " must contain a script");

            return task;
        }

        static BusinessRuleTask CreateBusinessRuleTask(XElement xElement)
        {
            var task = new BusinessRuleTask();
            task.Id = GetProcessId(xElement);
            //TODO: Set definition
            return task;
        }

        static ServiceTask CreateServiceTask(XElement xElement)
        {
            var task = new ServiceTask();
            task.Id = GetProcessId(xElement);
            //TODO: Set implementation and configuration
            return task;
        }

        static UserTask CreateUserTask(XElement xElement)
        {
            var task = new UserTask();
            task.Id = GetProcessId(xElement);
            task.Name = RemoveWhitespaces(GetProcessName(xElement));
            task.Assignee = GetProcessAssignee(xElement);

            var formElement = xElement.Descendants("Form").FirstOrDefault();
            if (formElement != null)
            {
                /* TODO: apply for 
                UserForm form = new UserForm();
                form.Id = formElement.Descendants("Id").FirstOrDefault().Value;
                form.Fields = new List<FormField>();
                foreach (var f in formElement.Descendants("ContractFormField"))
                {
                    FormField field = new FormField();
                    field.Id = f.Descendants("Id").FirstOrDefault().Value;
                    field.DisplayName = RemoveWhitespaces(f.Descendants("Name").FirstOrDefault().Value);
                    var readOnly = f.Descendants("ReadOnly").FirstOrDefault().Value;
                    if (readOnly == "true") field.IsReadOnly = true;
                    else if (readOnly == "false") field.IsReadOnly = false;
                    field.PropertyExpression = f.Descendants("PropertyId").FirstOrDefault().Value;
                    form.Fields.Add(field);
                }
                task.Form = form;
                */
            }
            return task;
        }

        static ExclusiveGateway CreateExclusiveGateway(XElement xElement)
        {
            var gateway = new ExclusiveGateway();
            gateway.Id = GetProcessId(xElement);
            gateway.Name = RemoveWhitespaces(GetProcessName(xElement));
            return gateway;
        }

        static ParallelGateway CreateParallelGateway(XElement xElement)
        {
            var gateway = new ParallelGateway();
            gateway.Id = GetProcessId(xElement);
            gateway.Name = RemoveWhitespaces(GetProcessName(xElement));
            return gateway;
        }

        static EndEvent CreateEndEvent(XElement xElement)
        {
            var endEvent = new EndEvent();
            endEvent.Id = GetProcessId(xElement);

            return endEvent;
        }

        static StartEvent CreateStartEvent(XElement xElement)
        {
            var startEvent = new StartEvent();
            startEvent.Id = GetProcessId(xElement);
            return startEvent;
        }

        static string GetProcessId(XElement xElement)
        {
            if (xElement.Descendants("Id").ToList().Count == 0)
                throw new InvalidElementException("ID must be set on every element");

            return xElement.Descendants("Id").First().Value;
        }

        static string RemoveWhitespaces(string str)
        {
            return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }

        static string GetProcessName(XElement xElement, bool fullName=false)
        {
            var nameAttribute = xElement.Descendants("Name").FirstOrDefault();

            if (nameAttribute != null && (!nameAttribute.Value.Contains("]") || fullName))
                return nameAttribute.Value;
            else if (nameAttribute != null)
                return Regex.Replace(nameAttribute.Value.Split(']')[1], @"\s+", "");
            return "";
        }

        static ProcessUser GetProcessAssignee(XElement xElement)
        {
            ProcessUser user = new ProcessUser();
            var processName = GetProcessName(xElement, true);
            if (processName.Contains("[") && processName.Contains("]"))
            {
                string address = processName.Split('[')[1].Split(']')[0];
                if (address.StartsWith("0x"))
                    user.Address = address;
                else
                    user.Name = address;
            }
            return user;
        }

    }
}
