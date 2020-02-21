using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DasContract.Editor.Entities;
using DasContract.Editor.Entities.DataModels;
using DasContract.Editor.Entities.DataModels.Entities;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Primitive;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Reference;
using DasContract.Editor.Entities.Forms;
using DasContract.Editor.Entities.Processes;
using DasContract.Editor.Entities.Processes.Diagrams;
using DasContract.Editor.Entities.Processes.Factories;
using DasContract.Editor.Entities.Processes.Process;
using DasContract.Editor.Entities.Serialization.XML;
using NUnit.Framework;

namespace DasContract.Editor.Tests.Entities.Serialization.XML
{
    public class Tests
    {
        ContractDataModel GetExampleDataModel()
        {
            var dataModel = new ContractDataModel();

            var entity2 = new ContractEntity()
            {
                Id = "entity-2",
                Name = "Entity 2",
            };

            var entity1 = new ContractEntity()
            {
                Id = "entity-1",
                Name = "Entity 1",
                PrimitiveProperties = new List<PrimitiveContractProperty>()
                {
                    new PrimitiveContractProperty()
                    {
                         Id = "property-1",
                         Name = "Property 1",
                         IsMandatory = true,
                         Type = PrimitiveContractPropertyType.Number
                    },
                    new PrimitiveContractProperty()
                    {
                         Id = "property-2",
                         Name = "Property 2",
                         IsMandatory = false,
                         Type = PrimitiveContractPropertyType.NumberCollection
                    },
                },
                ReferenceProperties = new List<ReferenceContractProperty>()
                {
                    new ReferenceContractProperty()
                    {
                         Id = "property-3",
                         Name = "Property 3",
                         IsMandatory = true,
                         Entity = entity2
                    }
                },
                CollectionReferenceProperties = new List<CollectionReferenceContractProperty>()
                {
                    new CollectionReferenceContractProperty()
                    {
                         Id = "property-4",
                         Name = "Property 4",
                         IsMandatory = false,
                         Entities = new ObservableCollection<ContractEntity>(){ entity2 }
                    }
                },
            };

            dataModel.Entities.Add(entity1);
            dataModel.Entities.Add(entity2);

            return dataModel;
        }

        void AssertExampleDataModel(ContractDataModel dataModel)
        {
            var entity1 = dataModel.Entities.Where(e => e.Id == "entity-1").Single();
            var entity2 = dataModel.Entities.Where(e => e.Id == "entity-2").Single();

            Assert.AreEqual("Entity 1", entity1.Name);
            Assert.AreEqual("property-1", entity1.PrimitiveProperties[0].Id);
            Assert.AreEqual("Property 1", entity1.PrimitiveProperties[0].Name);
            Assert.IsTrue(entity1.PrimitiveProperties[0].IsMandatory);
            Assert.AreEqual(PrimitiveContractPropertyType.Number, entity1.PrimitiveProperties[0].Type);

            Assert.AreEqual("property-2", entity1.PrimitiveProperties[1].Id);
            Assert.AreEqual("Property 2", entity1.PrimitiveProperties[1].Name);
            Assert.IsFalse(entity1.PrimitiveProperties[1].IsMandatory);
            Assert.AreEqual(PrimitiveContractPropertyType.NumberCollection, entity1.PrimitiveProperties[1].Type);

            Assert.AreEqual("property-3", entity1.ReferenceProperties[0].Id);
            Assert.AreEqual("Property 3", entity1.ReferenceProperties[0].Name);
            Assert.IsTrue(entity1.ReferenceProperties[0].IsMandatory);
            Assert.AreEqual(entity2, entity1.ReferenceProperties[0].Entity);
            Assert.AreEqual("entity-2", entity1.ReferenceProperties[0].EntityId);
            
            Assert.AreEqual("property-4", entity1.CollectionReferenceProperties[0].Id);
            Assert.AreEqual("Property 4", entity1.CollectionReferenceProperties[0].Name);
            Assert.IsFalse(entity1.CollectionReferenceProperties[0].IsMandatory);
            Assert.AreEqual(entity2, entity1.CollectionReferenceProperties[0].Entities[0]);
            Assert.AreEqual("entity-2", entity1.CollectionReferenceProperties[0].EntityIds[0]);
        }

        string GetExampleBPMN()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?> <bpmn:definitions xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:bpmn=\"http://www.omg.org/spec/BPMN/20100524/MODEL\" xmlns:bpmndi=\"http://www.omg.org/spec/BPMN/20100524/DI\" xmlns:dc=\"http://www.omg.org/spec/DD/20100524/DC\" xmlns:di=\"http://www.omg.org/spec/DD/20100524/DI\" id=\"Definitions_0p6wgsc\" targetNamespace=\"http://bpmn.io/schema/bpmn\" exporter=\"bpmn-js (https://demo.bpmn.io)\" exporterVersion=\"6.3.0\"> <bpmn:process id=\"Process_07u2mf6\" isExecutable=\"false\"> <bpmn:startEvent id=\"StartEvent_0q2d5wg\"> <bpmn:outgoing>Flow_0oz6h0h</bpmn:outgoing> </bpmn:startEvent> <bpmn:endEvent id=\"Event_13tfkn5\"> <bpmn:incoming>Flow_04oqalx</bpmn:incoming> <bpmn:incoming>Flow_1x5e5ef</bpmn:incoming> </bpmn:endEvent> <bpmn:exclusiveGateway id=\"Gateway_07a4al6\" name=\"Is it General Kenobi?\"> <bpmn:incoming>Flow_00sski2</bpmn:incoming> <bpmn:outgoing>Flow_04oqalx</bpmn:outgoing> <bpmn:outgoing>Flow_0m3jhsv</bpmn:outgoing> </bpmn:exclusiveGateway> <bpmn:sequenceFlow id=\"Flow_00sski2\" sourceRef=\"Activity_0tsw6mv\" targetRef=\"Gateway_07a4al6\" /> <bpmn:sequenceFlow id=\"Flow_04oqalx\" name=\"No\" sourceRef=\"Gateway_07a4al6\" targetRef=\"Event_13tfkn5\" /> <bpmn:sequenceFlow id=\"Flow_0m3jhsv\" name=\"Yes\" sourceRef=\"Gateway_07a4al6\" targetRef=\"Activity_0l230ae\" /> <bpmn:userTask id=\"Activity_0l230ae\" name=\"GENERAL KENOBI?\"> <bpmn:incoming>Flow_0m3jhsv</bpmn:incoming> <bpmn:outgoing>Flow_1p9g3p1</bpmn:outgoing> </bpmn:userTask> <bpmn:businessRuleTask id=\"Activity_0tsw6mv\" name=\"Hello there\"> <bpmn:incoming>Flow_0p2qori</bpmn:incoming> <bpmn:outgoing>Flow_00sski2</bpmn:outgoing> </bpmn:businessRuleTask> <bpmn:task id=\"Activity_1tpsax5\" name=\"A task\"> <bpmn:incoming>Flow_1p9g3p1</bpmn:incoming> <bpmn:outgoing>Flow_1x5e5ef</bpmn:outgoing> </bpmn:task> <bpmn:sequenceFlow id=\"Flow_1p9g3p1\" sourceRef=\"Activity_0l230ae\" targetRef=\"Activity_1tpsax5\" /> <bpmn:sequenceFlow id=\"Flow_1x5e5ef\" sourceRef=\"Activity_1tpsax5\" targetRef=\"Event_13tfkn5\" /> <bpmn:scriptTask id=\"Activity_1knqoaz\" name=\"Keep calm and refactor the whole goddamn shit goddamit dslkfmsldnfosindofni\"> <bpmn:incoming>Flow_0oz6h0h</bpmn:incoming> <bpmn:outgoing>Flow_0p2qori</bpmn:outgoing> </bpmn:scriptTask> <bpmn:sequenceFlow id=\"Flow_0oz6h0h\" sourceRef=\"StartEvent_0q2d5wg\" targetRef=\"Activity_1knqoaz\" /> <bpmn:sequenceFlow id=\"Flow_0p2qori\" sourceRef=\"Activity_1knqoaz\" targetRef=\"Activity_0tsw6mv\" /> </bpmn:process> <bpmndi:BPMNDiagram id=\"BPMNDiagram_1\"> <bpmndi:BPMNPlane id=\"BPMNPlane_1\" bpmnElement=\"Process_07u2mf6\"> <bpmndi:BPMNShape id=\"_BPMNShape_StartEvent_2\" bpmnElement=\"StartEvent_0q2d5wg\"> <dc:Bounds x=\"152\" y=\"222\" width=\"36\" height=\"36\" /> </bpmndi:BPMNShape> <bpmndi:BPMNShape id=\"Event_13tfkn5_di\" bpmnElement=\"Event_13tfkn5\"> <dc:Bounds x=\"692\" y=\"222\" width=\"36\" height=\"36\" /> </bpmndi:BPMNShape> <bpmndi:BPMNShape id=\"Gateway_07a4al6_di\" bpmnElement=\"Gateway_07a4al6\" isMarkerVisible=\"true\"> <dc:Bounds x=\"555\" y=\"215\" width=\"50\" height=\"50\" /> <bpmndi:BPMNLabel> <dc:Bounds x=\"552\" y=\"272\" width=\"58\" height=\"27\" /> </bpmndi:BPMNLabel> </bpmndi:BPMNShape> <bpmndi:BPMNEdge id=\"Flow_00sski2_di\" bpmnElement=\"Flow_00sski2\"> <di:waypoint x=\"510\" y=\"240\" /> <di:waypoint x=\"555\" y=\"240\" /> </bpmndi:BPMNEdge> <bpmndi:BPMNEdge id=\"Flow_04oqalx_di\" bpmnElement=\"Flow_04oqalx\"> <di:waypoint x=\"605\" y=\"240\" /> <di:waypoint x=\"692\" y=\"240\" /> <bpmndi:BPMNLabel> <dc:Bounds x=\"642\" y=\"222\" width=\"14\" height=\"14\" /> </bpmndi:BPMNLabel> </bpmndi:BPMNEdge> <bpmndi:BPMNEdge id=\"Flow_0m3jhsv_di\" bpmnElement=\"Flow_0m3jhsv\"> <di:waypoint x=\"580\" y=\"215\" /> <di:waypoint x=\"580\" y=\"160\" /> <bpmndi:BPMNLabel> <dc:Bounds x=\"585\" y=\"185\" width=\"20\" height=\"14\" /> </bpmndi:BPMNLabel> </bpmndi:BPMNEdge> <bpmndi:BPMNShape id=\"Activity_1edzm40_di\" bpmnElement=\"Activity_0l230ae\"> <dc:Bounds x=\"530\" y=\"80\" width=\"100\" height=\"80\" /> </bpmndi:BPMNShape> <bpmndi:BPMNShape id=\"Activity_0ak3qzi_di\" bpmnElement=\"Activity_0tsw6mv\"> <dc:Bounds x=\"410\" y=\"200\" width=\"100\" height=\"80\" /> </bpmndi:BPMNShape> <bpmndi:BPMNShape id=\"Activity_1tpsax5_di\" bpmnElement=\"Activity_1tpsax5\"> <dc:Bounds x=\"660\" y=\"80\" width=\"100\" height=\"80\" /> </bpmndi:BPMNShape> <bpmndi:BPMNEdge id=\"Flow_1p9g3p1_di\" bpmnElement=\"Flow_1p9g3p1\"> <di:waypoint x=\"630\" y=\"120\" /> <di:waypoint x=\"660\" y=\"120\" /> </bpmndi:BPMNEdge> <bpmndi:BPMNEdge id=\"Flow_1x5e5ef_di\" bpmnElement=\"Flow_1x5e5ef\"> <di:waypoint x=\"710\" y=\"160\" /> <di:waypoint x=\"710\" y=\"222\" /> </bpmndi:BPMNEdge> <bpmndi:BPMNShape id=\"Activity_0bj4485_di\" bpmnElement=\"Activity_1knqoaz\"> <dc:Bounds x=\"270\" y=\"200\" width=\"100\" height=\"80\" /> </bpmndi:BPMNShape> <bpmndi:BPMNEdge id=\"Flow_0oz6h0h_di\" bpmnElement=\"Flow_0oz6h0h\"> <di:waypoint x=\"188\" y=\"240\" /> <di:waypoint x=\"270\" y=\"240\" /> </bpmndi:BPMNEdge> <bpmndi:BPMNEdge id=\"Flow_0p2qori_di\" bpmnElement=\"Flow_0p2qori\"> <di:waypoint x=\"370\" y=\"240\" /> <di:waypoint x=\"410\" y=\"240\" /> </bpmndi:BPMNEdge> </bpmndi:BPMNPlane> </bpmndi:BPMNDiagram> </bpmn:definitions> ";
        }

        ContractProcess GetExampleProcess()
        {
            return ProcessFactory.FromXML(GetExampleBPMN()).First();
        }

        void AssertExampleProcess(ContractProcess process)
        {
            Assert.AreEqual(3, process.Activities.Count());
            Assert.AreEqual(1, process.Gateways.Count());
            Assert.AreEqual(6, process.ProcessElements.Count);
            Assert.AreEqual(7, process.SequenceFlows.Count);

            Assert.AreEqual("Flow_00sski2", process.SequenceFlows.First().Id);
            Assert.AreEqual("Activity_0tsw6mv", process.SequenceFlows.First().SourceId);
            Assert.AreEqual("Gateway_07a4al6", process.SequenceFlows.First().TargetId);
            Assert.AreEqual("Is it General Kenobi?", process.Gateways.First().Name);

            Assert.AreEqual("StartEvent_0q2d5wg", process.StartEvent.Id);
            Assert.AreEqual(1, process.StartEvent.Outgoing.Count);
            Assert.IsNull(process.StartEvent.Name);

            Assert.AreEqual("Keep calm and refactor the whole goddamn shit goddamit dslkfmsldnfosindofni", process.ScriptActivities.SingleOrDefault().Name);
            Assert.AreEqual("Hello there", process.BusinessActivities.SingleOrDefault().Name);
            Assert.AreEqual("GENERAL KENOBI?", process.UserActivities.SingleOrDefault().Name);
        }


        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ExampleDataModels()
        {
            AssertExampleDataModel(GetExampleDataModel());
        }

        [Test]
        public void ExampleProcess()
        {
            AssertExampleProcess(GetExampleProcess());
        }

        [Test]
        public void SerializationWithNoDiagram()
        {
            var contract = new EditorContract()
            {
                Id = "contract",
                Name = "Contract",
                DataModel = GetExampleDataModel()
            };

            //Serialize
            var serializedContract = EditorContractXML.To(contract);

            //Deserialize
            var deserializedContract = EditorContractXML.From(serializedContract);

            //Check
            Assert.AreEqual("contract", deserializedContract.Id);
            Assert.AreEqual("Contract", deserializedContract.Name);
            AssertExampleDataModel(deserializedContract.DataModel);
        }

        [Test]
        public void SerializationWithDiagram()
        {
            var contract = new EditorContract()
            {
                Id = "contract",
                Name = "Contract",
                DataModel = GetExampleDataModel(),
                Processes = new ContractProcesses()
                {
                    Diagram = BPMNProcessDiagram.FromXml(GetExampleBPMN())
                }
            };
            contract.Processes.Main.UserActivities.First().Form = new ContractForm() { Id = "XXX" };
            contract.Processes.Main.BusinessActivities.First().Diagram = DMNProcessDiagram.FromXML("YYY");
            contract.Processes.Main.ScriptActivities.First().Script = "ZZZ";
            contract.Processes.Main.StartEvent.StartForm = new ContractForm() { Id = "XXXX" };

            contract.Processes.Main.StartEvent.StartForm.Fields.Add(new ContractFormField()
            {
                 PropertyBinding = new ContractPropertyBinding() { Property = contract.DataModel.Entities[0].PrimitiveProperties[0] }
            });
            contract.Processes.Main.UserActivities.First().Form.Fields.Add(new ContractFormField()
            {
                PropertyBinding = new ContractPropertyBinding() { Property = contract.DataModel.Entities[0].PrimitiveProperties[0] }
            });

            //Serialize
            var serializedContract = EditorContractXML.To(contract);

            //Deserialize
            var deserializedContract = EditorContractXML.From(serializedContract);

            //Check
            Assert.AreEqual("XXXX", contract.Processes.Main.StartEvent.StartForm.Id);
            Assert.AreEqual("XXX", contract.Processes.Main.UserActivities.First().Form.Id);
            Assert.AreEqual("YYY", contract.Processes.Main.BusinessActivities.First().Diagram.DiagramXML);
            Assert.AreEqual("ZZZ", contract.Processes.Main.ScriptActivities.First().Script);

            Assert.AreEqual(contract.DataModel.Entities[0].PrimitiveProperties[0], 
                contract.Processes.Main.StartEvent.StartForm.Fields[0].PropertyBinding.Property);
            Assert.AreEqual(contract.DataModel.Entities[0].PrimitiveProperties[0],
                contract.Processes.Main.UserActivities.First().Form.Fields[0].PropertyBinding.Property);


            Assert.AreEqual("contract", deserializedContract.Id);
            Assert.AreEqual("Contract", deserializedContract.Name);
            AssertExampleDataModel(deserializedContract.DataModel);
            Assert.AreEqual(GetExampleBPMN(), deserializedContract.Processes.Diagram.DiagramXML);
            AssertExampleProcess(contract.Processes.Main);
        }
    }
}
