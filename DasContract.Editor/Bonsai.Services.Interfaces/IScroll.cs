using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Services.Interfaces
{
    public interface IScroll
    {
        /// <summary>
        /// Initializes slow scroll on all <a> elements with a class and anchor
        /// </summary>
        /// <param name="className">The name of the class to bind with link anchor slow scroll</param>
        Task AnchorScrollAsync(string className);

        /// <summary>
        /// Scrolls page to a specific X axis position from top
        /// </summary>
        /// <param name="top">X axis position from top of the page</param>
        Task ToAsync(uint top);

        /// <summary>
        /// Scrolls to the fist element that matches the selector.
        /// Does not scroll if no such element exists. 
        /// </summary>
        /// <param name="selector">Selector for the element to scroll to</param>
        Task ToAsync(string selector);

        /// <summary>
        /// Scrolls to the top of the page
        /// </summary>
        Task ToTopAsync();
    }
}
