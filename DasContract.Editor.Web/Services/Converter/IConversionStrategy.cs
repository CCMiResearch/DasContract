using DasContract.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.Converter
{
    public interface IConversionStrategy
    {
        bool Convert(Contract contract);
        string GetConvertedCode();
        string GetErrorMessage();
    }
}
