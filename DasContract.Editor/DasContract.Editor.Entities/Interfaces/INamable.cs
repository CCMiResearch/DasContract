using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Entities.Interfaces
{
    public interface INamable
    {
        /// <summary>
        /// Display (reader friendly) name of this entity
        /// </summary>
        string Name { get; set; }
    }
}
