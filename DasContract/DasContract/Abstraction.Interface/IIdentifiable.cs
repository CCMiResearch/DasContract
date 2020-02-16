using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.DasContract.Abstraction.Interface
{
    public interface IIdentifiable
    {
        /// <summary>
        /// Identification if this entity
        /// </summary>
        string Id { get; set; }
    }
}
