using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Entities.Interfaces
{
    public interface IIdentifiable
    {
        /// <summary>
        /// Identification if this entity
        /// </summary>
        string Id { get; set; }
    }
}
