using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.BpmnEvents
{
    public interface IBpmnEventListener
    {
        public event EventHandler<BpmnElementEvent> ElementClick;
        public event EventHandler<BpmnElementEvent> ElementChanged;
        public event EventHandler<BpmnElementEvent> ShapeAdded;
        public event EventHandler<BpmnElementEvent> ShapeRemoved;
        public event EventHandler<BpmnElementEvent> ElementIdUpdated;
        public event EventHandler<BpmnElementEvent> ConnectionAdded;
        public event EventHandler<BpmnElementEvent> ConnectionRemoved;
        public event EventHandler<BpmnElementEvent> RootAdded;
        public event EventHandler<BpmnElementEvent> RootRemoved;

        public Task InitializeHandler();
    }
}
