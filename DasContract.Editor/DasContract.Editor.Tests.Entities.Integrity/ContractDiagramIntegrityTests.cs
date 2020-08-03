using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DasContract.Editor.Entities;
using DasContract.Editor.Entities.DataModels;
using DasContract.Editor.Entities.DataModels.Entities;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Primitive;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Reference;
using DasContract.Editor.Entities.Exceptions;
using DasContract.Editor.Entities.Forms;
using DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities;
using DasContract.Editor.Entities.Integrity.Contract.Processes;
using DasContract.Editor.Entities.Integrity.Extensions;
using DasContract.Editor.Entities.Processes;
using DasContract.Editor.Entities.Processes.Diagrams;
using DasContract.Editor.Entities.Processes.Process;
using DasContract.Editor.Entities.Processes.Process.Activities;
using NUnit.Framework;

namespace DasContract.Editor.Tests.Entities.Integrity
{
    public class ContractDiagramIntegrityTests : BaseTest
    {
        string GetExampleBPMN()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?> <bpmn:definitions xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:bpmn=\"http://www.omg.org/spec/BPMN/20100524/MODEL\" xmlns:bpmndi=\"http://www.omg.org/spec/BPMN/20100524/DI\" xmlns:dc=\"http://www.omg.org/spec/DD/20100524/DC\" xmlns:di=\"http://www.omg.org/spec/DD/20100524/DI\" id=\"Definitions_0p6wgsc\" targetNamespace=\"http://bpmn.io/schema/bpmn\" exporter=\"bpmn-js (https://demo.bpmn.io)\" exporterVersion=\"6.3.0\"> <bpmn:process id=\"Process_07u2mf6\" isExecutable=\"false\"> <bpmn:startEvent id=\"StartEvent_0q2d5wg\"> <bpmn:outgoing>Flow_0oz6h0h</bpmn:outgoing> </bpmn:startEvent> <bpmn:endEvent id=\"Event_13tfkn5\"> <bpmn:incoming>Flow_04oqalx</bpmn:incoming> <bpmn:incoming>Flow_1x5e5ef</bpmn:incoming> </bpmn:endEvent> <bpmn:exclusiveGateway id=\"Gateway_07a4al6\" name=\"Is it General Kenobi?\"> <bpmn:incoming>Flow_00sski2</bpmn:incoming> <bpmn:outgoing>Flow_04oqalx</bpmn:outgoing> <bpmn:outgoing>Flow_0m3jhsv</bpmn:outgoing> </bpmn:exclusiveGateway> <bpmn:sequenceFlow id=\"Flow_00sski2\" sourceRef=\"Activity_0tsw6mv\" targetRef=\"Gateway_07a4al6\" /> <bpmn:sequenceFlow id=\"Flow_04oqalx\" name=\"No\" sourceRef=\"Gateway_07a4al6\" targetRef=\"Event_13tfkn5\" /> <bpmn:sequenceFlow id=\"Flow_0m3jhsv\" name=\"Yes\" sourceRef=\"Gateway_07a4al6\" targetRef=\"Activity_0l230ae\" /> <bpmn:userTask id=\"Activity_0l230ae\" name=\"GENERAL KENOBI?\"> <bpmn:incoming>Flow_0m3jhsv</bpmn:incoming> <bpmn:outgoing>Flow_1p9g3p1</bpmn:outgoing> </bpmn:userTask> <bpmn:businessRuleTask id=\"Activity_0tsw6mv\" name=\"Hello there\"> <bpmn:incoming>Flow_0p2qori</bpmn:incoming> <bpmn:outgoing>Flow_00sski2</bpmn:outgoing> </bpmn:businessRuleTask> <bpmn:task id=\"Activity_1tpsax5\" name=\"A task\"> <bpmn:incoming>Flow_1p9g3p1</bpmn:incoming> <bpmn:outgoing>Flow_1x5e5ef</bpmn:outgoing> </bpmn:task> <bpmn:sequenceFlow id=\"Flow_1p9g3p1\" sourceRef=\"Activity_0l230ae\" targetRef=\"Activity_1tpsax5\" /> <bpmn:sequenceFlow id=\"Flow_1x5e5ef\" sourceRef=\"Activity_1tpsax5\" targetRef=\"Event_13tfkn5\" /> <bpmn:scriptTask id=\"Activity_1knqoaz\" name=\"Keep calm and refactor the whole goddamn shit goddamit dslkfmsldnfosindofni\"> <bpmn:incoming>Flow_0oz6h0h</bpmn:incoming> <bpmn:outgoing>Flow_0p2qori</bpmn:outgoing> </bpmn:scriptTask> <bpmn:sequenceFlow id=\"Flow_0oz6h0h\" sourceRef=\"StartEvent_0q2d5wg\" targetRef=\"Activity_1knqoaz\" /> <bpmn:sequenceFlow id=\"Flow_0p2qori\" sourceRef=\"Activity_1knqoaz\" targetRef=\"Activity_0tsw6mv\" /> </bpmn:process> <bpmndi:BPMNDiagram id=\"BPMNDiagram_1\"> <bpmndi:BPMNPlane id=\"BPMNPlane_1\" bpmnElement=\"Process_07u2mf6\"> <bpmndi:BPMNShape id=\"_BPMNShape_StartEvent_2\" bpmnElement=\"StartEvent_0q2d5wg\"> <dc:Bounds x=\"152\" y=\"222\" width=\"36\" height=\"36\" /> </bpmndi:BPMNShape> <bpmndi:BPMNShape id=\"Event_13tfkn5_di\" bpmnElement=\"Event_13tfkn5\"> <dc:Bounds x=\"692\" y=\"222\" width=\"36\" height=\"36\" /> </bpmndi:BPMNShape> <bpmndi:BPMNShape id=\"Gateway_07a4al6_di\" bpmnElement=\"Gateway_07a4al6\" isMarkerVisible=\"true\"> <dc:Bounds x=\"555\" y=\"215\" width=\"50\" height=\"50\" /> <bpmndi:BPMNLabel> <dc:Bounds x=\"552\" y=\"272\" width=\"58\" height=\"27\" /> </bpmndi:BPMNLabel> </bpmndi:BPMNShape> <bpmndi:BPMNEdge id=\"Flow_00sski2_di\" bpmnElement=\"Flow_00sski2\"> <di:waypoint x=\"510\" y=\"240\" /> <di:waypoint x=\"555\" y=\"240\" /> </bpmndi:BPMNEdge> <bpmndi:BPMNEdge id=\"Flow_04oqalx_di\" bpmnElement=\"Flow_04oqalx\"> <di:waypoint x=\"605\" y=\"240\" /> <di:waypoint x=\"692\" y=\"240\" /> <bpmndi:BPMNLabel> <dc:Bounds x=\"642\" y=\"222\" width=\"14\" height=\"14\" /> </bpmndi:BPMNLabel> </bpmndi:BPMNEdge> <bpmndi:BPMNEdge id=\"Flow_0m3jhsv_di\" bpmnElement=\"Flow_0m3jhsv\"> <di:waypoint x=\"580\" y=\"215\" /> <di:waypoint x=\"580\" y=\"160\" /> <bpmndi:BPMNLabel> <dc:Bounds x=\"585\" y=\"185\" width=\"20\" height=\"14\" /> </bpmndi:BPMNLabel> </bpmndi:BPMNEdge> <bpmndi:BPMNShape id=\"Activity_1edzm40_di\" bpmnElement=\"Activity_0l230ae\"> <dc:Bounds x=\"530\" y=\"80\" width=\"100\" height=\"80\" /> </bpmndi:BPMNShape> <bpmndi:BPMNShape id=\"Activity_0ak3qzi_di\" bpmnElement=\"Activity_0tsw6mv\"> <dc:Bounds x=\"410\" y=\"200\" width=\"100\" height=\"80\" /> </bpmndi:BPMNShape> <bpmndi:BPMNShape id=\"Activity_1tpsax5_di\" bpmnElement=\"Activity_1tpsax5\"> <dc:Bounds x=\"660\" y=\"80\" width=\"100\" height=\"80\" /> </bpmndi:BPMNShape> <bpmndi:BPMNEdge id=\"Flow_1p9g3p1_di\" bpmnElement=\"Flow_1p9g3p1\"> <di:waypoint x=\"630\" y=\"120\" /> <di:waypoint x=\"660\" y=\"120\" /> </bpmndi:BPMNEdge> <bpmndi:BPMNEdge id=\"Flow_1x5e5ef_di\" bpmnElement=\"Flow_1x5e5ef\"> <di:waypoint x=\"710\" y=\"160\" /> <di:waypoint x=\"710\" y=\"222\" /> </bpmndi:BPMNEdge> <bpmndi:BPMNShape id=\"Activity_0bj4485_di\" bpmnElement=\"Activity_1knqoaz\"> <dc:Bounds x=\"270\" y=\"200\" width=\"100\" height=\"80\" /> </bpmndi:BPMNShape> <bpmndi:BPMNEdge id=\"Flow_0oz6h0h_di\" bpmnElement=\"Flow_0oz6h0h\"> <di:waypoint x=\"188\" y=\"240\" /> <di:waypoint x=\"270\" y=\"240\" /> </bpmndi:BPMNEdge> <bpmndi:BPMNEdge id=\"Flow_0p2qori_di\" bpmnElement=\"Flow_0p2qori\"> <di:waypoint x=\"370\" y=\"240\" /> <di:waypoint x=\"410\" y=\"240\" /> </bpmndi:BPMNEdge> </bpmndi:BPMNPlane> </bpmndi:BPMNDiagram> </bpmn:definitions> ";
        }

        [Test]
        public void ValidatePotencialDiagram_OK()
        {
            contract.ValidatePotentialDiagram(BPMNProcessDiagram.Default());
            contract.ValidatePotentialDiagram(BPMNProcessDiagram.FromXml(GetExampleBPMN()));
        }

        [Test]
        public void ValidatePotencialDiagram_NoStartEvents()
        {
            try
            {
                contract.ValidatePotentialDiagram(BPMNProcessDiagram.FromXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?> <bpmn:definitions xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:bpmn=\"http://www.omg.org/spec/BPMN/20100524/MODEL\" xmlns:bpmndi=\"http://www.omg.org/spec/BPMN/20100524/DI\" xmlns:dc=\"http://www.omg.org/spec/DD/20100524/DC\" id=\"Definitions_0eb01sb\" targetNamespace=\"http://bpmn.io/schema/bpmn\" exporter=\"bpmn-js (https://demo.bpmn.io)\" exporterVersion=\"6.3.0\"> <bpmn:process id=\"Process_1sge6c9\" isExecutable=\"false\"> <bpmn:endEvent id=\"Event_0n2mpqp\" /> </bpmn:process> <bpmndi:BPMNDiagram id=\"BPMNDiagram_1\"> <bpmndi:BPMNPlane id=\"BPMNPlane_1\" bpmnElement=\"Process_1sge6c9\"> <bpmndi:BPMNShape id=\"_BPMNShape_StartEvent_2\" bpmnElement=\"StartEvent_0p6bth7\"> <dc:Bounds x=\"156\" y=\"81\" width=\"36\" height=\"36\" /> </bpmndi:BPMNShape> <bpmndi:BPMNShape id=\"Event_0n2mpqp_di\" bpmnElement=\"Event_0n2mpqp\"> <dc:Bounds x=\"272\" y=\"81\" width=\"36\" height=\"36\" /> </bpmndi:BPMNShape> </bpmndi:BPMNPlane> </bpmndi:BPMNDiagram> </bpmn:definitions> "));
                Assert.Fail();
            }
            catch (AtLeastOneStartEventRequiredException)
            {

            }
        }

        [Test]
        public void ValidatePotencialDiagram_NoEndEvents()
        {
            try
            {
                contract.ValidatePotentialDiagram(BPMNProcessDiagram.FromXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?> <bpmn:definitions xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:bpmn=\"http://www.omg.org/spec/BPMN/20100524/MODEL\" xmlns:bpmndi=\"http://www.omg.org/spec/BPMN/20100524/DI\" xmlns:dc=\"http://www.omg.org/spec/DD/20100524/DC\" id=\"Definitions_0eb01sb\" targetNamespace=\"http://bpmn.io/schema/bpmn\" exporter=\"bpmn-js (https://demo.bpmn.io)\" exporterVersion=\"6.3.0\"> <bpmn:process id=\"Process_1sge6c9\" isExecutable=\"false\"> <bpmn:startEvent id=\"StartEvent_0p6bth7\" /> </bpmn:process> <bpmndi:BPMNDiagram id=\"BPMNDiagram_1\"> <bpmndi:BPMNPlane id=\"BPMNPlane_1\" bpmnElement=\"Process_1sge6c9\"> <bpmndi:BPMNShape id=\"_BPMNShape_StartEvent_2\" bpmnElement=\"StartEvent_0p6bth7\"> <dc:Bounds x=\"156\" y=\"81\" width=\"36\" height=\"36\" /> </bpmndi:BPMNShape> <bpmndi:BPMNShape id=\"Event_0n2mpqp_di\" bpmnElement=\"Event_0n2mpqp\"> <dc:Bounds x=\"272\" y=\"81\" width=\"36\" height=\"36\" /> </bpmndi:BPMNShape> </bpmndi:BPMNPlane> </bpmndi:BPMNDiagram> </bpmn:definitions> "));
                Assert.Fail();
            }
            catch (AtLeastOneEndEventRequiredException)
            {

            }
        }

        [Test]
        public void BPMNProcessReplaceSafely()
        {
            var contract = new EditorContract()
            {
                Processes = new ContractProcesses()
                {

                }
            };
            contract.ReplaceSafely(BPMNProcessDiagram.Default());

            Assert.AreEqual(0, contract.AnalyzeIntegrityWhenReplacedWith(BPMNProcessDiagram.FromXml(GetExampleBPMN())).DeleteRisks.Count());

            contract.ReplaceSafely(BPMNProcessDiagram.FromXml(GetExampleBPMN()));

            Assert.AreEqual(3, contract.AnalyzeIntegrityWhenReplacedWith(BPMNProcessDiagram.Default()).DeleteRisks.Count());
        }

        [Test]
        public void BPMNProcessReplaceSafely_NoRisks()
        {
            var contract = new EditorContract()
            {
                Processes = new ContractProcesses()
                {

                }
            };
            contract.ReplaceSafely(BPMNProcessDiagram.Default());

            Assert.IsFalse(contract.AnalyzeIntegrityWhenReplacedWith(BPMNProcessDiagram.Default()).HasDeleteRisks());
        }
    }
}
