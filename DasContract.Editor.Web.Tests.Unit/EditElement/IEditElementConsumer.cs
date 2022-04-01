using DasContract.Editor.Web.Services.EditElement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Tests.Unit.EditElement
{
    public interface IEditElementConsumer
    {
        void ConsumeEditElementAssigned(object sender, EditElementEventArgs args);
        void ConsumeEditElementChanged(object sender, EventArgs args);
    }
}
