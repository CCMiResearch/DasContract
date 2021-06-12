using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Shared
{
    public class ToolBarItem
    {
        public event EventHandler<MouseEventArgs> OnClick;
        public string Description { get; set; }
        public string Name { get; set; }
        public string IconPath { get; set; }

        public void OnToolBarItemClick(object sender, MouseEventArgs args)
        {
            OnClick?.Invoke(sender, args);
        }
    }
}
