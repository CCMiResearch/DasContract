using DasContract.Abstraction;

namespace DasContract.Editor.Web.Services.Converter
{
    public class ConverterService: IConverterService
    {
        public IConversionStrategy ConversionStrategy { get; set; }

        public ConverterService()
        {
        }

        public bool ConvertContract(Contract contract)
        {
            if (ConversionStrategy is null)
                throw new ConversionStrategyNotSetException();

            return ConversionStrategy.Convert(contract);
        }

        public string GetConvertedCode()
        {
            return ConversionStrategy.GetConvertedCode();
        }

        public string GetErrorMessage()
        {
            return ConversionStrategy.GetErrorMessage();
        }
    }
}
