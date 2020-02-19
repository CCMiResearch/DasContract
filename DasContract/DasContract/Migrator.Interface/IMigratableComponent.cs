using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Migrator.Interface
{
    public interface IMigratableComponent<TClass, TMigrator>
        where TMigrator : IMigrator
    {
        /// <summary>
        /// Returns "this" but with its migrator set to the input migrator
        /// </summary>
        /// <param name="parentMigrator">The input migrator to be set</param>
        /// <returns>Current class with the new migrator</returns>
        TClass WithMigrator(TMigrator parentMigrator);
    }
}
