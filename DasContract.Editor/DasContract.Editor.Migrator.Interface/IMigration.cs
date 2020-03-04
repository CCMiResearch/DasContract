using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DasContract.Editor.Migrator.Interfaces
{
    public interface IMigration
    {
        /// <summary>
        /// Property expression of the property that is changed/tracked
        /// </summary>
        Expression PropertyExpression { get; set; }

        /// <summary>
        /// Bring the version up / forward
        /// </summary>
        void Up();

        /// <summary>
        /// Brings the version down / backward
        /// </summary>
        void Down();
    }
}
