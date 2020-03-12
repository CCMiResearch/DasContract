using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Migrator;
using DasContract.Editor.Migrator.Interfaces;

namespace DasContract.Editor.Entities.Processes.Diagrams
{
    public class BPMNProcessDiagram: IMigratableComponent<BPMNProcessDiagram, IMigrator>
    {
        /// <summary>
        /// XML reprezentations of the BPMN diagram
        /// </summary>
        public string DiagramXML
        {
            get => diagramXML;
            set
            {
                if (value != diagramXML)
                    migrator.Notify(() => diagramXML, e => diagramXML = e,
                            MigratorMode.Smart);
                diagramXML = value;
            }
        }
        string diagramXML;


        public static BPMNProcessDiagram FromXml(string diagramXml)
        {
            return new BPMNProcessDiagram()
            {
                DiagramXML = diagramXml
            };
        }

        public static bool IsNullOrEmpty(BPMNProcessDiagram diagram) => diagram == null ? true : diagram.IsEmpty();

        public bool IsEmpty() => string.IsNullOrWhiteSpace(diagramXML);

        public static BPMNProcessDiagram Default() => BPMNProcessDiagram.FromXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?> <bpmn:definitions xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:bpmn=\"http://www.omg.org/spec/BPMN/20100524/MODEL\" xmlns:bpmndi=\"http://www.omg.org/spec/BPMN/20100524/DI\" xmlns:dc=\"http://www.omg.org/spec/DD/20100524/DC\" xmlns:di=\"http://www.omg.org/spec/DD/20100524/DI\" id=\"Definitions_0hiyvor\" targetNamespace=\"http://bpmn.io/schema/bpmn\" exporter=\"bpmn-js (https://demo.bpmn.io)\" exporterVersion=\"6.3.1\"> <bpmn:process id=\"Process_1v7jqr0\" isExecutable=\"false\"> <bpmn:startEvent id=\"StartEvent_1nygn6s\"> <bpmn:outgoing>Flow_1im4rte</bpmn:outgoing> </bpmn:startEvent> <bpmn:endEvent id=\"Event_1nfb3wk\"> <bpmn:incoming>Flow_1im4rte</bpmn:incoming> </bpmn:endEvent> <bpmn:sequenceFlow id=\"Flow_1im4rte\" sourceRef=\"StartEvent_1nygn6s\" targetRef=\"Event_1nfb3wk\" /> </bpmn:process> <bpmndi:BPMNDiagram id=\"BPMNDiagram_1\"> <bpmndi:BPMNPlane id=\"BPMNPlane_1\" bpmnElement=\"Process_1v7jqr0\"> <bpmndi:BPMNShape id=\"_BPMNShape_StartEvent_2\" bpmnElement=\"StartEvent_1nygn6s\"> <dc:Bounds x=\"156\" y=\"81\" width=\"36\" height=\"36\" /> </bpmndi:BPMNShape> <bpmndi:BPMNShape id=\"Event_1nfb3wk_di\" bpmnElement=\"Event_1nfb3wk\"> <dc:Bounds x=\"242\" y=\"81\" width=\"36\" height=\"36\" /> </bpmndi:BPMNShape> <bpmndi:BPMNEdge id=\"Flow_1im4rte_di\" bpmnElement=\"Flow_1im4rte\"> <di:waypoint x=\"192\" y=\"99\" /> <di:waypoint x=\"242\" y=\"99\" /> </bpmndi:BPMNEdge> </bpmndi:BPMNPlane> </bpmndi:BPMNDiagram> </bpmn:definitions> ");

        //--------------------------------------------------
        //                  MIGRATOR
        //--------------------------------------------------
        IMigrator migrator = new SimpleMigrator();

        public BPMNProcessDiagram WithMigrator(IMigrator parentMigrator)
        {
            migrator = parentMigrator;
            return this;
        }
    }
}
