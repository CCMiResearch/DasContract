using System;
using DasContract.Editor.Entities.Interfaces;
using DasContract.Editor.Entities.Processes.Diagrams;
using DasContract.Editor.Migrator.Interfaces;

namespace DasContract.Editor.Entities.Processes.Process.Activities
{
    public class ContractBusinessRuleActivity : ContractActivity, IDataCopyable<ContractBusinessRuleActivity>, IMigratableComponent<ContractBusinessRuleActivity, IMigrator>
    {
        /// <summary>
        /// A definition of a business rule in DMN diagram
        /// </summary>
        public DMNProcessDiagram Diagram { get; set; } = DMNProcessDiagram.Default();

        public void CopyDataFrom(ContractBusinessRuleActivity source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            Diagram = source.Diagram;
        }

        public ContractBusinessRuleActivity WithMigrator(IMigrator parentMigrator)
        {
            Migrator = parentMigrator;
            return this;
        }
    }
}
