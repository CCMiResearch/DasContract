using DasContract.Blockchain.Solidity.Converters;
using DasContract.Editor.Web.Services.Converter;
using DasContract.Editor.Web.Services.Processes;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Shared
{
    public partial class NavMenu : ComponentBase
    {
        [Inject]
        protected IContractManager ContractManager { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected ElementReference NameInputReference { get; set; } 

        protected string ContractName { get { return ContractManager.GetContractName(); } set { ContractManager.SetContractName(value); } }

        [CascadingParameter]
        protected MainLayout Layout { get; set; }

        protected bool IsNameBeingEdited { get; set; }

        public void GenerateContract()
        {
            Console.WriteLine(ContractManager.ConvertToSolidity());
        }

        protected string BaseRelativePath()
        {
            return NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        }
    }
}
