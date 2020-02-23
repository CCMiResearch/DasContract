using System;

namespace Bonsai.RazorComponents.Interfaces
{
    public interface ILoadableComponent
    {
        /// <summary>
        /// Property telling if the component is in the process of loading or not
        /// </summary>
        bool Loading { get; set; }
    }
}
