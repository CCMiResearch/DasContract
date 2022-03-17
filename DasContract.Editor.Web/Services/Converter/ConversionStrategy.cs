using DasContract.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.Converter
{
    public abstract class ConversionStrategy : IConversionStrategy
    {
        protected string ConvertedCode { get; set; }
        protected string ErrorMessage { get; set; }

        public string GetConvertedCode()
        {
            return ConvertedCode;
        }

        public string GetErrorMessage()
        {
            return ErrorMessage;
        }

        public abstract bool Convert(Contract contract);
    }
}
