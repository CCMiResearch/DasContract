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
