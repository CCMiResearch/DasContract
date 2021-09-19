using DasContract.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.Converter
{
    public interface IConverterService
    {
        string ConvertContract(Contract contract);
    }
}
