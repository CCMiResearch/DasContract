using DasContract.Abstraction.DataModel;
using DasContract.Abstraction.Processes;
using DasContract.DasContract.Abstraction.Interface;
using DasContract.Migrator;
using DasContract.Migrator.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Entity
{
    public class Contract : IIdentifiable, INamable, IMigratorProvider<IMigrator>
    {
        public string Id { get; set; }

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
            get => processes.WithMigrator(migrator);
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
            get => dataModel.WithMigrator(migrator);
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

        [XmlIgnore]
        readonly IMigrator migrator = new SimpleMigrator();

        public void StartTracingSteps()
        {
            migrator.StartTracingSteps();
        }

        public void StopTracingSteps()
        {
            migrator.StopTracingSteps();
        }
    }
}
