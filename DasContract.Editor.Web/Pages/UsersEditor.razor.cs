using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Editor.Web.Services.Processes;
using DasContract.Editor.Web.Services.UndoRedo;
using DasContract.Editor.Web.Services.UserInput;
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

        [Inject]
        protected UserInputHandler UserInputHandler { get; set; }

        protected IJSRuntime JSRuntime { get; set; }

        protected override void OnInitialized()
        {
            UserInputHandler.KeyDown += HandleKeyDown;
        }

        public void Dispose()
        {
            UserInputHandler.KeyDown -= HandleKeyDown;
        }

        public void HandleKeyDown(object sender, KeyEvent e)
        {
            if (e.CtrlKey && e.Key == "z")
            {
                UsersRolesManager.Undo();
                StateHasChanged();
            }
            else if (e.CtrlKey && e.Key == "y")
            {
                UsersRolesManager.Redo();
                StateHasChanged();
            }
        }
    }
}
