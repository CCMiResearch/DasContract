using DasContract.Abstraction;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.BpmnEvents.Exceptions;
using DasContract.Editor.Web.Services.Converter;
using DasContract.Editor.Web.Services.LocalStorage;
using DasContract.Editor.Web.Services.Save;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DasContract.Editor.Web.Services.ContractManagement
{
    public class ContractManager : IContractManager
    {
        private readonly IDataModelManager _dataModelManager;
        private readonly IUserModelManager _userModelManager;
        private readonly IProcessModelManager _processModelManager;

        protected Contract Contract { get; set; }

        private IJSRuntime _jsRuntime;

        private HttpClient _httpClient;

        private SaveManager _saveManager;

        private IContractStorage _contractStorage;

        private IConverterService _converterService;

        public string GeneratedContract { get; private set; }
        public string SerializedContract { get; private set; }

        public ContractManager(IJSRuntime jsRuntime, HttpClient httpClient,
            IConverterService converterService, IContractStorage contractStorage, 
            SaveManager saveManager, IDataModelManager dataModelManager, IUserModelManager userModelManager, 
            IProcessModelManager processModelManager)
        {
            _jsRuntime = jsRuntime;
            _converterService = converterService;
            _httpClient = httpClient;
            _contractStorage = contractStorage;
            _saveManager = saveManager;
            _dataModelManager = dataModelManager;
            _userModelManager = userModelManager;
            _processModelManager = processModelManager;
        }

        public async Task InitAsync()
        {
            await _jsRuntime.InvokeVoidAsync("exitGuardLib.setContractManagerInstance", DotNetObjectReference.Create(this));
            _saveManager.ContractSaveRequested += SaveContract;
        }

        public bool IsContractInitialized()
        {
            return Contract != null;
        }

        public async Task InitializeNewContract()
        {
            Contract = new Contract
            {
                Id = Guid.NewGuid().ToString()
            };
            ContractChanged();
            try
            {
                _dataModelManager.SetDataModelXml(await _httpClient.GetStringAsync("dist/examples/example-datatypes.xml"));
            }
            catch (Exception) { }
        }

        public string SerializeContract()
        {
            return Contract.ToXElement().ToString();
        }

        public void RestoreContract(string contractXML)
        {
            var xElement = XElement.Parse(contractXML);
            Contract = new Contract(xElement);
            ContractChanged();
        }

        public bool ConvertContract(out string data)
        {
            if (_converterService.ConvertContract(Contract))
            {
                data = _converterService.GetConvertedCode();
                return true;
            }
            else
            {
                data = _converterService.GetErrorMessage();
                return false;
            }
        }

        public string GetContractName()
        {
            return string.IsNullOrWhiteSpace(Contract?.Name) ? "Unnamed contract" : Contract.Name;
        }

        public string GetContractId()
        {
            return Contract.Id;
        }

        public void SetContractName(string name)
        {
            Contract.Name = name;
        }

        [JSInvokable]
        public bool CanSafelyExit()
        {
            if (Contract == null)
                return true;

            return SerializeContract() == SerializedContract;
        }

        private async Task SaveContract(object sender, EventArgs args)
        {
            var currentlySerialized = SerializeContract();
            if (currentlySerialized != SerializedContract)
            {
                SerializedContract = currentlySerialized;
                await _contractStorage.StoreContract(Contract.Id, GetContractName(), SerializedContract);
            }
        }

        private void ContractChanged()
        {
            SerializedContract = SerializeContract();
            _dataModelManager.SetContract(Contract);
            _userModelManager.SetContract(Contract);
            _processModelManager.SetContract(Contract);
        }
    }
}
