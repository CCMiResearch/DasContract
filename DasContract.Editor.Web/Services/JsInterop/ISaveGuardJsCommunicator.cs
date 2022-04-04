using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.JsInterop
{
    public interface ISaveGuardJsCommunicator
    {
        Task<bool> DisplayAndCollectConfirmation(string message = null);
    }
}
