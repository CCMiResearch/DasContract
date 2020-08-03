using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Pages.Main.Pages
{
    public partial class Index: PageBase
    {

        protected override void OnInitialized()
        {
            

            base.OnInitialized();
            Breadcrumbs
                .AddHome("DasContract")
                .Finish();
        }
    }
}
