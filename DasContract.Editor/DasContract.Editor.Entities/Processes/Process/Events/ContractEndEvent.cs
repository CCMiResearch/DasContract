using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Migrator.Interfaces;

namespace DasContract.Editor.Entities.Processes.Process.Events
{
    public class ContractEndEvent : ContractEvent, IMigratableComponent<ContractEndEvent, IMigrator>
    {
        public ContractEndEvent WithMigrator(IMigrator parentMigrator)
        {
            Migrator = parentMigrator;
            return this;
        }
    }
}
