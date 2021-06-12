using DasContract.Blockchain.Solidity.Converters;
using DasContract.Editor.Web.Services.Converter;
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
        private IConverterService ConverterService { get; set; }

        [CascadingParameter]
        protected MainLayout Layout { get; set; }

        public void GenerateContract()
        {
            ConverterService.ConvertContract();
        }
    }
}
