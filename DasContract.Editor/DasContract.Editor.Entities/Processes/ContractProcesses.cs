using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DasContract.Editor.Entities.Processes.Diagrams;
using DasContract.Editor.Entities.Processes.Process;
using DasContract.Editor.Migrator;
using DasContract.Editor.Migrator.Interfaces;

namespace DasContract.Editor.Entities.Processes
{
    public class ContractProcesses : IMigratableComponent<ContractProcesses, IMigrator>
    {
        /// <summary>
        /// BPMN 2.0 XML with process description and a visual process information for the main process
        /// </summary>
        public BPMNProcessDiagram Diagram
        {
            get => diagram.WithMigrator(migrator);
            set
            {
                if (value != diagram)
                    migrator.Notify(() => diagram, d => diagram = d);
                diagram = value;

                throw new NotImplementedException();
                /*var processes = ProcessFactory.FromBPMN(value);
                if (processes.Count() != 1)
                    throw new InvalidProcessCountException("The diagram must contain exactly one process");

                UpdateMainProcess(processes.First());*/
            }
        }
        BPMNProcessDiagram diagram;

        public void UpdateMainProcess(ContractProcess value)
        {
            throw new NotImplementedException();
            /*if (value == null)
            {
                Main = null;
                return;
            }

            if (Main == null)
            {
                Main = value;
                return;
            }

            if (value == Main)
                return;

            var oldProcess = Main;
            var newProcess = value;

            foreach (var item in oldProcess.ScriptActivities)
            {
                var res = newProcess.ScriptActivities.Where(e => e.Id == item.Id).SingleOrDefault();
                if (res != null)
                    res.CopyCustomDataFrom(item);
            }

            foreach (var item in oldProcess.BusinessActivities)
            {
                var res = newProcess.BusinessActivities.Where(e => e.Id == item.Id).SingleOrDefault();
                if (res != null)
                    res.CopyCustomDataFrom(item);
            }

            foreach (var item in oldProcess.UserActivities)
            {
                var res = newProcess.UserActivities.Where(e => e.Id == item.Id).SingleOrDefault();
                if (res != null)
                    res.CopyCustomDataFrom(item);
            }

            Main = newProcess;*/
        }

        /// <summary>
        /// Contract main process. Currently only one process is allowed. 
        /// </summary>
        public ContractProcess Main { get; set; }

        //--------------------------------------------------
        //                  MIGRATOR
        //--------------------------------------------------
        IMigrator migrator = new SimpleMigrator();

        public ContractProcesses WithMigrator(IMigrator parentMigrator)
        {
            migrator = parentMigrator;
            return this;
        }
    }
}
