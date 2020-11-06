using DasContract.Abstraction.Data;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.Converters
{
    public class TokenConverter
    {
        Token token;

        DataModelConverter dataModelConverter;

        SolidityContract tokenContract;

        public TokenConverter(Token token, DataModelConverter dataModelConverter)
        {
            this.token = token;
            this.dataModelConverter = dataModelConverter;
        }

        public void ConvertLogic()
        {
            tokenContract = new SolidityContract(token.Name);
            tokenContract.AddInheritance(ConverterConfig.OWNABLE_NAME);
            dataModelConverter.AddDependency(ConverterConfig.OWNABLE_IMPORT);
            if (token.IsFungible)
            {
                tokenContract.AddInheritance(ConverterConfig.FUNGIBLE_TOKEN_NAME);
                dataModelConverter.AddDependency(ConverterConfig.FUNGIBLE_TOKEN_IMPORT);
            }
            else
            {
                tokenContract.AddInheritance(ConverterConfig.NON_FUNGIBLE_TOKEN_NAME);
                dataModelConverter.AddDependency(ConverterConfig.NON_FUNGIBLE_TOKEN_IMPORT);
            }

            foreach (var property in token.Properties)
            {
                tokenContract.AddComponent(dataModelConverter.ConvertProperty(property));
            }
        }

        public SolidityContract GetTokenContract()
        {
            return tokenContract;
        }
    }
}
