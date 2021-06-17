using DasContract.Editor.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services
{
    public class SaveManager
    {
        public AsyncEvent<EventArgs> SaveRequested = new AsyncEvent<EventArgs>();

        public async Task RequestSave()
        {
            await SaveRequested.InvokeAsync(this, EventArgs.Empty);
        }
    }
}
