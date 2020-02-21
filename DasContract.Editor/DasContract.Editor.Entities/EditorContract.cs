using DasContract.Editor.Entities.DataModels;
using DasContract.Editor.Entities.Interfaces;
using DasContract.Editor.Entities.Processes;
using DasContract.Editor.Migrator;
using DasContract.Editor.Migrator.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Entities
{
    public class EditorContract: IIdentifiable, INamable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name
        {
            get => name;
            set
            {
                if (value != name)
                    migrator.Notify(() => name, p => name = p);
                name = value;
            }
        }
        string name;

        /// <summary>
        /// Contract processes
        /// </summary>
        public ContractProcesses Processes
        {
            get => processes?.WithMigrator(migrator);
            set
            {
                if (value != processes)
                    migrator.Notify(() => processes, p => processes = p);
                processes = value;
            }
        }
        ContractProcesses processes = new ContractProcesses();

        /// <summary>
        /// Contract data model
        /// </summary>
        public ContractDataModel DataModel
        {
            get => dataModel?.WithMigrator(migrator);
            set
            {
                if (value != dataModel)
                    migrator.Notify(() => dataModel, d => dataModel = d);
                dataModel = value;
            }
        }
        ContractDataModel dataModel = new ContractDataModel();

        //--------------------------------------------------
        //                  MIGRATOR
        //--------------------------------------------------
        public IMigrator GetMigrator()
        {
            return migrator;
        }

        readonly IMigrator migrator = new SimpleMigrator();

        public EditorContract StartTracingSteps()
        {
            migrator.StartTracingSteps();
            return this;
        }

        public EditorContract StopTracingSteps()
        {
            migrator.StopTracingSteps();
            return this;
        }
    }
}
