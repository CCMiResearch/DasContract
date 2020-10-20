using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.Converters
{
    public static class ConverterConfig
    {
        public static readonly string SOLIDITY_VERSION = "^0.6.6";

        public static readonly string IS_STATE_ACTIVE_FUNCTION_NAME = "isStateActive";
        public static readonly string ACTIVE_STATES_NAME = "ActiveStates";
        public static readonly string STATE_PARAMETER_NAME = "state";

        public static readonly string ADDRESS_MAPPING_VAR_NAME = "addressMapping";

        public static readonly string FUNGIBLE_TOKEN_IMPORT = "https://github.com/OpenZeppelin/openzeppelin-contracts/blob/release-v3.1.0/contracts/token/ERC20/ERC20.sol";
        public static readonly string NON_FUNGIBLE_TOKEN_IMPORT = "import https://github.com/OpenZeppelin/openzeppelin-contracts/blob/release-v3.1.0/contracts/token/ERC721/ERC721.sol";

        public static readonly string FUNGIBLE_TOKEN_NAME = "ERC20";
        public static readonly string NON_FUNGIBLE_TOKEN_NAME = "ERC721";
    }
}
