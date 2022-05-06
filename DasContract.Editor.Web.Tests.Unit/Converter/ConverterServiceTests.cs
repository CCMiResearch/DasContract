using DasContract.Abstraction;
using DasContract.Editor.Web.Services.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DasContract.Editor.Web.Tests.Unit.Converter
{
    public class ConverterServiceTests
    {
        private readonly IConverterService _converterService;
        public ConverterServiceTests()
        {
            _converterService = new ConverterService();
        }

        [Fact]
        public void SetConversionStrategyAndConvert_ShouldConvert()
        {
            var contract = new Contract { Name = "Hello world", Id = "Contract1"};
            var conversionStrategy = new MockConversionStrategy();
            _converterService.ConversionStrategy = conversionStrategy;

            var conversionResult = _converterService.ConvertContract(contract);

            Assert.True(conversionResult);
            Assert.Equal(contract.Name, _converterService.GetConvertedCode());
        }

        [Fact]
        public void SetConversionStrategyAndConvertWithoutId_ShouldError()
        {
            var contract = new Contract { Name = "Hello world"};
            var conversionStrategy = new MockConversionStrategy();
            _converterService.ConversionStrategy = conversionStrategy;

            var conversionResult = _converterService.ConvertContract(contract);

            Assert.False(conversionResult);
            Assert.Equal("Id is not defined", _converterService.GetErrorMessage());
        }
    }
}
