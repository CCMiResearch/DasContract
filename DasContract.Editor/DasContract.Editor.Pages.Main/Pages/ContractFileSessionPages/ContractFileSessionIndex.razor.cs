using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bonsai.RazorComponents.MaterialBootstrap.Components.CBreadcrumbs;

namespace DasContract.Editor.Pages.Main.Pages.ContractFileSessionPages
{
    public partial class ContractFileSessionIndex: PageBase
    {
        public static Breadcrumb Breadcrumb { get; } = new Breadcrumb("File session", "/ContractFileSession");

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Breadcrumbs
                .AddHome("DasContract")
                .AddLastCrumb(Breadcrumb);
        }
    }
}
