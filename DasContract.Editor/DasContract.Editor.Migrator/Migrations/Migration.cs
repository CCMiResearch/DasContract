using DasContract.Editor.Migrator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DasContract.Editor.Migrator
{
    public abstract class Migration : IMigration
    {
        public Expression PropertyExpression { get; set; }

        /// <inheritdoc/>
        public abstract void Down();

        /// <inheritdoc/>
        public abstract void Up();
    }
}
