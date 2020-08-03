

using System;
using System.ComponentModel;
using DasContract.Editor.Entities.Interfaces;
using DasContract.Editor.Migrator.Interfaces;

namespace DasContract.Editor.Entities.Processes.Process.Activities
{
    public class ContractScriptActivity : ContractActivity, IDataCopyable<ContractScriptActivity>, IMigratableComponent<ContractScriptActivity, IMigrator>
    {
        [DisplayName("Script code")]
        [Description("Source code for the script activity that will execute on activity start")]
        public string Script
        {
            get => script;
            set
            {
                if (value != script)
                    Migrator.Notify(() => script, e => script = e,
                            MigratorMode.Smart);
                script = value;
            }
        }
        string script;

        public void CopyDataFrom(ContractScriptActivity source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            script = source.Script;
        }

        public ContractScriptActivity WithMigrator(IMigrator parentMigrator)
        {
            Migrator = parentMigrator;
            return this;
        }
    }
}
