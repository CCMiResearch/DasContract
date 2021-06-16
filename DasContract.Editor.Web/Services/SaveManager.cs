using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services
{
    public class SaveManager
    {
        public event EventHandler SaveRequested;

        public void RequestSave()
        {
            SaveRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
