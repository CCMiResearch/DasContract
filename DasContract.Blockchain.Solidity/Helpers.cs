using DasContract.Abstraction.Data;
using System.Linq;

namespace DasContract.Blockchain.Solidity
{
    class Helpers
    {
        public static readonly string ADDRESS_MAPPING_VAR_NAME = "addressMapping";

        public static string GetSolidityStringType(Property property)
        {
            var type = property.DataType;
            switch (type)
            {
                case PropertyDataType.Bool:
                    return "bool";
                case PropertyDataType.String:
                    return "string";
                case PropertyDataType.Int:
                    return "int";
                case PropertyDataType.Address:
                    return "address";
                case PropertyDataType.AddressPayable:
                    return "address payable";
                case PropertyDataType.Data:
                    return "string";
                case PropertyDataType.DateTime:
                    return "string";
                case PropertyDataType.Uint:
                    return "uint256";
                case PropertyDataType.Entity:
                    return GetPropertyStructureName(property.Entity.Name);
                default:
                    return "string";
            }
        }

        public static string GetDefaultValueString(Property property)
        {
            var type = property.DataType;
            switch (type)
            {
                case PropertyDataType.Bool:
                    return "false";
                case PropertyDataType.String:
                    return "\"\"";
                case PropertyDataType.Int:
                    return "0";
                case PropertyDataType.Address:
                    return "address(0x0)";
                case PropertyDataType.AddressPayable:
                    return "address(0x0)";
                case PropertyDataType.Data:
                    return "\"\"";
                case PropertyDataType.DateTime:
                    return "\"\"";
                case PropertyDataType.Uint:
                    return "0";
                case PropertyDataType.Entity:
                    string s = GetPropertyStructureName(property.Entity.Name) + "({";
                    foreach (var p in property.Entity.Properties)
                    {
                        if (!p.IsCollection)
                        {
                            s += GetPropertyVariableName(p.Name) + ": " + GetDefaultValueString(p) + ", ";
                        }
                    }
                    return s.Trim().Trim(',') + "})";
                default:
                    return "\"\"";
            }
        }

        public static string GetPropertyVariableName(string name)
        {
            return name.First().ToString().ToLower() + name.Substring(1);
        }

        public static string GetPropertyStructureName(string name)
        {
            return name.First().ToString().ToUpper() + name.Substring(1);
        }

    }
}
