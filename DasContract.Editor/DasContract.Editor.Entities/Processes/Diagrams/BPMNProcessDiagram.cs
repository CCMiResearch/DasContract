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
                    migrator.Notify(() => diagramXML, e => diagramXML = e);
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
