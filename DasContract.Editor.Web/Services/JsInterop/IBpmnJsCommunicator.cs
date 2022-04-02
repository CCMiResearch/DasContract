using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.JsInterop
{
    interface IBpmnJsCommunicator
    {
        Task UpdateElementId(string oldElementId, string newElementId);
        Task UpdateElementName(string elementId, string newElementName);
    }
}
