using DasContract.Abstraction;
using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.BpmnEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.EditElement
{
    public class EditElementService: IDisposable, IEditElementService
    {
        /// <summary>
        /// Gets called whenever a new element is assigned
        /// </summary>
        public event EventHandler<EditElementEventArgs> EditElementAssigned;
        /// <summary>
        /// Gets called whenever the assigned element is modified in any way
        /// </summary>
        public event EventHandler EditElementModified;

        private IContractElement _editElement;

        public IContractElement EditElement
        {
            get { return _editElement; }
            set
            {
                if (_editElement == value)
                {
                    return;
                }

                _editElement = value;
                var args = new EditElementEventArgs { processElement = _editElement };
                OnEditElementAssigned(args);
            }
        }

        public void EditedElementModified()
        {
            EditElementModified?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnEditElementAssigned(EditElementEventArgs args)
        {
            EditElementAssigned?.Invoke(this, args);
        }

        public void Dispose()
        {
        }
    }
}
