using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Migrator;
using DasContract.Editor.Migrator.Interfaces;

namespace DasContract.Editor.Entities.Processes.Diagrams
{
    public class DMNProcessDiagram : IMigratableComponent<DMNProcessDiagram, IMigrator>
    {
        /// <summary>
        /// XML reprezentations of the DMN diagram
        /// </summary>
        public string DiagramXML
        {
            get => diagramXML;
            set
            {
                if (value != diagramXML)
                    migrator.Notify(() => diagramXML, e => diagramXML = e);
                diagramXML = value;
            }
        }
        string diagramXML;

        public static DMNProcessDiagram FromXml(string diagramXml)
        {
            return new DMNProcessDiagram()
            {
                DiagramXML = diagramXml
            };
        }

        public bool IsEmpty() => string.IsNullOrEmpty(diagramXML);

        public static bool IsNullOrEmpty(DMNProcessDiagram diagram) => diagram == null ? true : diagram.IsEmpty();

        public static DMNProcessDiagram Default()
        {
            return FromXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?> <definitions xmlns=\"https://www.omg.org/spec/DMN/20191111/MODEL/\" id=\"definitions_0uuo4c9\" name=\"definitions\" namespace=\"http://camunda.org/schema/1.0/dmn\" exporter=\"dmn-js (https://demo.bpmn.io/dmn)\" exporterVersion=\"8.0.1\"> <decision id=\"decision_0w1gd1h\" name=\"\"> <decisionTable id=\"decisionTable_0p21n5y\"> <input id=\"input1\" label=\"\"> <inputExpression id=\"inputExpression1\" typeRef=\"string\"> <text></text> </inputExpression> </input> <output id=\"output1\" label=\"\" name=\"\" typeRef=\"string\" /> <rule id=\"DecisionRule_0lbi946\"> <inputEntry id=\"UnaryTests_03llcno\"> <text>FooInput</text> </inputEntry> <outputEntry id=\"LiteralExpression_0utk97l\"> <text>FooResponse</text> </outputEntry> </rule> <rule id=\"DecisionRule_096k27d\"> <inputEntry id=\"UnaryTests_0ca3uln\"> <text></text> </inputEntry> <outputEntry id=\"LiteralExpression_1punxos\"> <text></text> </outputEntry> </rule> </decisionTable> </decision> </definitions> ");
        }

        //--------------------------------------------------
        //                  MIGRATOR
        //--------------------------------------------------
        IMigrator migrator = new SimpleMigrator();

        public DMNProcessDiagram WithMigrator(IMigrator parentMigrator)
        {
            migrator = parentMigrator;
            return this;
        }
    }
}
