using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.UserForm
{
    public class UserFormService
    {
        private bool _isPreviewOpen = false;
        public bool IsPreviewOpen { 
            get { return _isPreviewOpen; } 
            set 
            {
                if(_isPreviewOpen != value)
                {
                    _isPreviewOpen = value;
                    IsPreviewOpenChanged?.Invoke();
                }
            } 
        }

        public event Action IsPreviewOpenChanged;
    }
}
