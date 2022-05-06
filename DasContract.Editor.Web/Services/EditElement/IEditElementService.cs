using DasContract.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.EditElement
{
    public interface IEditElementService
    {
        /// <summary>
        /// Gets called whenever a new element is assigned
        /// </summary>
        public event EventHandler<EditElementEventArgs> EditElementAssigned;
        /// <summary>
        /// Gets called whenever the assigned element is modified in any way
        /// </summary>
        public event EventHandler EditElementModified;

        public IContractElement EditElement {get; set;}

        public void EditedElementModified();
    }
}
