using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.CamundaEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.EditElement
{
    public class EditElementService: IDisposable
    {
        public event EventHandler<EditElementEventArgs> EditElementChanged;

        private ProcessElement _editElement;

        public ProcessElement EditElement
        {
            get { return _editElement; }
            set
            {
                if(_editElement == value)
                {
                    return;
                }

                _editElement = value;
                var args = new EditElementEventArgs { processElement = _editElement };
                OnEditElementChanged(args);
            }
        }

        protected virtual void OnEditElementChanged(EditElementEventArgs args)
        {
            EditElementChanged?.Invoke(this, args);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
