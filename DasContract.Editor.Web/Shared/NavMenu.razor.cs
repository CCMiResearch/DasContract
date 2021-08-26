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
    public partial class NavMenu: ComponentBase
    {
        [Inject]
        private IContractManager ContractManager { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        protected MainLayout Layout { get; set; }

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
