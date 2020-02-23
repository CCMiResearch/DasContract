using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorComponents.Interfaces
{
    public interface IIdentifiableComponent
    {
        /// <summary>
        /// Id of this component
        /// </summary>
        string Id { get; set; }
    }
}
