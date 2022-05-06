using DasContract.Abstraction;
using DasContract.Editor.Web.Services.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Tests.Unit.Converter
{
    public class MockConversionStrategy : ConversionStrategy
    {
        public override bool Convert(Contract contract)
        {
            if (contract.Id is null)
            {
                ErrorMessage = "Id is not defined";
                return false;
            }
            else
            {
                ConvertedCode = contract.Name;
                return true;
            }
        }
    }
}
