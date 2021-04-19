using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.BpmnEvents
{
    public interface IBpmnEventHandler
    {
        public event EventHandler<BpmnInternalEvent> ElementClick;
        public event EventHandler<BpmnInternalEvent> ElementChanged;
        public event EventHandler<BpmnInternalEvent> ShapeAdded;
        public event EventHandler<BpmnInternalEvent> ShapeRemoved;
        public event EventHandler<BpmnInternalEvent> ElementIdUpdated;
        public event EventHandler<BpmnInternalEvent> ConnectionAdded;
        public event EventHandler<BpmnInternalEvent> ConnectionRemoved;

        public Task InitializeHandler();
    }
}
