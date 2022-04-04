using DasContract.Abstraction;
using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.EditElement;
using DasContract.Editor.Web.Services.JsInterop;
using DasContract.Editor.Web.Services.ContractManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.ProcessDetail.GeneralTabs
{
    public partial class ElementGeneralTab : ComponentBase
    {
        [Parameter]
        public IContractElement ContractElement { get; set; }

        [Inject]
        private IJSRuntime JSRunTime { get; set; }

        [Inject]
        private IContractManager ContractManager { get; set; }

        [Inject]
        private IBpmnJsCommunicator BpmnJsCommunicator { get; set; }

        [Inject]
        private EditElementService EditElementService { get; set; }

        protected string _idInputClassDecorator;
        protected string _idInputErrorMessage;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected async void IdInput(ChangeEventArgs args)
        {
            var id = args.Value as string;
            _idInputErrorMessage = string.Empty;
            _idInputClassDecorator = "";

            if (id == ContractElement.Id)
                return;

            if (string.IsNullOrEmpty(id))
            {
                _idInputClassDecorator = "is-invalid";
                _idInputErrorMessage += "The id cannot be empty\n";
                return;
            }
            if (id.Length > 50)
            {
                _idInputClassDecorator = "is-invalid";
                _idInputErrorMessage += "The id can't be longer than 50 characters\n";
                return;
            }
            if (id != ContractElement.Id && !ContractManager.IsElementIdAvailable(id))
            {
                _idInputClassDecorator = "is-invalid";
                _idInputErrorMessage += "The id must be unique\n";
                return;
            }
            if (!Regex.IsMatch(id, "^[a-zA-Z0-9_]*$"))
            {
                _idInputClassDecorator = "is-invalid";
                _idInputErrorMessage += "Id must consist of alphanumerical characters and underscores\n";
                return;
            }
            

            if (ContractElement is Process)
            {
                ContractManager.UpdateProcessId(ContractElement as Process, id);
                EditElementService.EditedElementModified();
            }
            else
            {
                await BpmnJsCommunicator.UpdateElementId(ContractElement.Id, id);
            }
        }

        protected async void NameInput(ChangeEventArgs args)
        {
            var name = args.Value as string;
            string elementId;
            if (ContractElement is Process)
            {
                var process = ContractElement as Process;
                elementId = string.IsNullOrEmpty(process.ParticipantId) ? process.BpmnId : process.ParticipantId;
            }
            else
                elementId = ContractElement.Id;

            if (elementId != null)
                await JSRunTime.InvokeVoidAsync("modellerLib.updateElementName", elementId, name);
        }
    }
}
