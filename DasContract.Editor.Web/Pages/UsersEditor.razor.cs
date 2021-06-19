using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Editor.Web.Services.Processes;
using DasContract.Editor.Web.Services.UndoRedo;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace DasContract.Editor.Web.Pages
{
    public partial class UsersEditor : ComponentBase, IDisposable
    {
        [Inject]
        protected IContractManager ContractManager { get; set; }

        [Inject]
        protected UsersRolesManager UsersRolesManager { get; set; }

        protected IJSRuntime JSRuntime { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
