using DasContract.Abstraction;

namespace DasContract.Editor.Web.Services.Converter
{
    public class ConverterService: IConverterService
    {
        private IConversionStrategy ConversionStrategy { get; set; }
        private ConversionTarget CurrentTarget { get; set; }

        public ConverterService()
        {
            ConversionStrategy = new SolidityConversionStrategy();
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

        public void SetConversionTarget(ConversionTarget conversionTarget)
        {
            if (conversionTarget == CurrentTarget)
                return;

            switch (conversionTarget)
            {
                case ConversionTarget.PLUTUS:
                    ConversionStrategy = new PlutusConversionStrategy();
                    break;
                case ConversionTarget.SOLIDITY:
                    ConversionStrategy = new SolidityConversionStrategy();
                    break;
            }

            CurrentTarget = conversionTarget;
        }
    }
}
