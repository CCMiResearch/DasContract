using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.DasContract.Abstraction.Interface
{
    public interface INamable
    {
        /// <summary>
        /// Display (reader friendly) name of this entity
        /// </summary>
        string Name { get; set; }
    }
}
