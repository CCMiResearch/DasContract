using DasContract.Abstraction.Data;
using System.Linq;

namespace DasContract.Blockchain.Solidity
{
    class Helpers
    {
        public static readonly string ADDRESS_MAPPING_VAR_NAME = "addressMapping";

        public static string GetSolidityStringType(Property property)
        {
            var type = property.Type;
            switch (type)
            {
                case PropertyType.Bool:
                    return "bool";
                case PropertyType.String:
                    return "string";
                case PropertyType.Int:
                    return "int";
                case PropertyType.Address:
                    return "address";
                case PropertyType.AddressPayable:
                    return "address payable";
                case PropertyType.Data:
                    return "string";
                case PropertyType.DateTime:
                    return "string";
                case PropertyType.Uint:
                    return "uint256";
                case PropertyType.Entity:
                    return GetPropertyStructureName(property.Entity.Name);
                default:
                    return "string";
            }
        }

        public static string GetDefaultValueString(Property property)
        {
            var type = property.Type;
            switch (type)
            {
                case PropertyType.Bool:
                    return "false";
                case PropertyType.String:
                    return "\"\"";
                case PropertyType.Int:
                    return "0";
                case PropertyType.Address:
                    return "address(0x0)";
                case PropertyType.AddressPayable:
                    return "address(0x0)";
                case PropertyType.Data:
                    return "\"\"";
                case PropertyType.DateTime:
                    return "\"\"";
                case PropertyType.Uint:
                    return "0";
                case PropertyType.Entity:
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
