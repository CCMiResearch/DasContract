using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DasContract.Abstraction.BPMN.Factory;
using DasContract.Abstraction.Entity;
using DasContract.Abstraction.Exceptions.Specific;
using DasContract.Migrator;
using DasContract.Migrator.Interface;

namespace DasContract.Abstraction.Processes
{
    public class ContractProcesses: IMigratableComponent<ContractProcesses, IMigrator>
    {
        /// <summary>
        /// BPMN 2.0 XML with process description and a visual process information for the main process
        /// </summary>
        public string Diagram
        {
            get => diagram;
            set
            {
                if (value != diagram)
                    migrator.Notify(() => diagram, d => diagram = d);
                diagram = value;

                var processes = ProcessFactory.FromBPMN(value);
                if (processes.Count() != 1)
                    throw new InvalidProcessCountException("The diagram must contain exactly one process");

                UpdateMainProcess(processes.First());
            }
        }
        string diagram;

        public void UpdateMainProcess(Process value)
        {
            if (value == null)
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

            Main = newProcess;
        }

        /// <summary>
        /// Contract main process. Currently only one process is allowed. 
        /// </summary>
        public Process Main { get; set; }

        //--------------------------------------------------
        //                  MIGRATOR
        //--------------------------------------------------
        [XmlIgnore]
        IMigrator migrator = new SimpleMigrator();

        public ContractProcesses WithMigrator(IMigrator parentMigrator)
        {
            migrator = parentMigrator;
            return this;
        }
    }
}
