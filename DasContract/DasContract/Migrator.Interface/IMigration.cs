using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DasContract.Migrator.Interface
{
    public interface IMigration
    {
        /// <summary>
        /// Property expression of the property that is changed/tracked
        /// </summary>
        Expression PropertyExpression { get; set; }

        /// <summary>
        /// Reverts to the last known value and the last known value will be the current one
        /// </summary>
        void RevertLastKnownValue();
    }
}
