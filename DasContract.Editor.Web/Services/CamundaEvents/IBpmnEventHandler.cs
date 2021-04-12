using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.CamundaEvents
{
    public interface IBpmnEventHandler
    {
        public event EventHandler<BpmnInternalEvent> ElementClick;
        public event EventHandler<BpmnInternalEvent> ElementChanged;
        public event EventHandler<BpmnInternalEvent> ShapeAdded;
        public event EventHandler<BpmnInternalEvent> ShapeRemoved;
        public event EventHandler<BpmnInternalEvent> ElementIdUpdated;

        public Task InitializeHandler();
    }
}
