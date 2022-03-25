using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.BpmnEvents
{
    public interface IBpmnSynchronizer
    {
        public void InitializeOrRestoreBpmnEditor(string canvasElementId);
    }
}
