using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Migrator.Interfaces
{
    public interface IMigratorProvider<TMigrator>
        where TMigrator : IMigrator
    {
        /// <summary>
        /// Returns a migrator
        /// </summary>
        /// <returns>The migrator</returns>
        TMigrator GetMigrator();
    }
}
