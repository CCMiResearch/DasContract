using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.UserInput
{
    public class KeyEvent
    {
        public bool AltKey { get; set; }
        public bool CtrlKey { get; set; }
        public string Type { get; set; }
        public string Key { get; set; }
    }
}
