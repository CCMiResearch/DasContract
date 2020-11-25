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
            tokenContract = new SolidityContract(token.ToStructureName());
            tokenContract.AddInheritance(ConverterConfig.OWNABLE_NAME);
            //constructor
            tokenContract.AddComponent(CreateConstructor());
            //mint
            tokenContract.AddComponent(CreateMintFunction());
            //transfer
            tokenContract.AddComponent(CreateTransferFunction());
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

        SolidityConstructor CreateConstructor()
        {
            SolidityConstructor constructor = new SolidityConstructor();
            constructor.AddParameter(new SolidityParameter("string memory", "name"));
            constructor.AddParameter(new SolidityParameter("string memory", "symbol"));
            if (token.IsFungible)
                constructor.AddParentCall(ConverterConfig.FUNGIBLE_TOKEN_NAME, new List<string> { "name", "symbol" });
            else
                constructor.AddParentCall(ConverterConfig.NON_FUNGIBLE_TOKEN_NAME, new List<string> { "name", "symbol" });
            constructor.AddParentCall(ConverterConfig.OWNABLE_NAME, new List<string>());

            return constructor;
        }

        SolidityFunction CreateMintFunction()
        {
            SolidityFunction function = new SolidityFunction("mint", SolidityVisibility.Public);

            function.AddParameter(new SolidityParameter("address", "receiver"));
            function.AddModifier(ConverterConfig.OWNABLE_MODIFIER);
            function.AddToBody(new SolidityStatement(token.MintScript, false));
            //function.AddToBody(new SolidityStatement("_safeMint(receiver, uint256(receiver))"));
            return function;
        }

        SolidityFunction CreateTransferFunction()
        {
            SolidityFunction function = new SolidityFunction("transfer", SolidityVisibility.Public);

            function.AddParameter(new SolidityParameter("address", "from"));
            function.AddParameter(new SolidityParameter("address", "to"));
            function.AddModifier(ConverterConfig.OWNABLE_MODIFIER);
            function.AddToBody(new SolidityStatement(token.TransferScript, false));

            return function;
        }

        /// <summary>
        /// Provides the statement necessary to initialize the token's contract in the main contract.
        /// </summary>
        /// <returns></returns>
        public SolidityStatement GetConstructorStatement()
        {
            return new SolidityStatement($"{token.ToVariableName()} = " +
                $"new {token.ToStructureName()}(\"{token.Name}\",\"{token.Symbol}\")");
        }

        public SolidityStatement GetTokenVariableStatement()
        {
            return new SolidityStatement($"{token.ToStructureName()} {token.ToVariableName()}");
        }

        public SolidityContract GetTokenContract()
        {
            return tokenContract;
        }
    }
}
