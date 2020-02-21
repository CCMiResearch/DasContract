using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DasContract.Editor.Entities.Exceptions;
using DasContract.Editor.Entities.Interfaces;
using DasContract.Editor.Entities.Processes.Diagrams;
using DasContract.Editor.Entities.Processes.Factories;
using DasContract.Editor.Entities.Processes.Process;
using DasContract.Editor.Entities.Processes.Process.Activities;
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

                if (value == null)
                    return;

                var processes = ProcessFactory.FromXML(value.DiagramXML);
                if (processes.Count() != 1)
                    throw new InvalidProcessCountException("The diagram must contain exactly one process");

                UpdateMainProcess(processes.First());
            }
        }
        BPMNProcessDiagram diagram;

        /// <summary>
        /// Updates the main process and copies custom data from the old one
        /// </summary>
        /// <param name="value">The new main process</param>
        public void UpdateMainProcess(ContractProcess value)
        {
            var oldProcess = Main;
            var newProcess = value;
            Main = newProcess;

            if (newProcess == null || oldProcess == null)
                return;

            //Update activities
            UpdateMainProcessActivities(oldProcess.ScriptActivities, newProcess.ScriptActivities);
            UpdateMainProcessActivities(oldProcess.BusinessActivities, newProcess.BusinessActivities);
            UpdateMainProcessActivities(oldProcess.UserActivities, newProcess.UserActivities);

            //Update start event
            newProcess.StartEvent.CopyDataFrom(oldProcess.StartEvent);
        }

        /// <summary>
        /// Updates ienumerable of new activities with custom data from the old activities
        /// </summary>
        /// <typeparam name="TActivity"></typeparam>
        /// <param name="oldActivities"></param>
        /// <param name="newActivities"></param>
        void UpdateMainProcessActivities<TActivity>(IEnumerable<TActivity> oldActivities, IEnumerable<TActivity> newActivities)
            where TActivity : ContractActivity, IDataCopyable<TActivity>
        {
            foreach (var item in oldActivities)
            {
                var res = newActivities.Where(e => e.Id == item.Id).SingleOrDefault();
                if (res != null)
                    res.CopyDataFrom(item);
            }
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
