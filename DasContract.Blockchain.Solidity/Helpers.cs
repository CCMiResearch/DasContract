using DasContract.Abstraction.Data;
using System.Linq;

namespace DasContract.Blockchain.Solidity
{
    class Helpers
    {


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
                case PropertyDataType.Reference:
                    return GetPropertyStructureName(property.ReferencedDataType); //TODO: needs an update
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
                case PropertyDataType.Reference:
                    /*
                    string s = GetPropertyStructureName(property.ReferencedDataType.Name) + "({";
                    foreach (var p in property.ReferencedDataType.Properties)
                    {
                        if (!p.IsCollection)
                        {
                            s += GetPropertyVariableName(p.Name) + ": " + GetDefaultValueString(p) + ", ";
                        }
                    }
                    
                    return s.Trim().Trim(',') + "})";
                    */
                    return "placedholder"; //TODO: needs an update
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
