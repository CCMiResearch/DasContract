using DasContract.Abstraction.Data;
using DasContract.Blockchain.Solidity.Converters;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace DasContract.Blockchain.Solidity
{
    public static class Helpers
    {

        static readonly string[] formats = { 
            // Basic formats
            "yyyyMMddTHHmmsszzz",
            "yyyyMMddTHHmmsszz",
            "yyyyMMddTHHmmssZ",
            "yyyyMMdd",
            "yyyy-MM-dd",
            // Extended formats
            "yyyy-MM-ddTHH:mm:sszzz",
            "yyyy-MM-ddTHH:mm:sszz",
            "yyyy-MM-ddTHH:mm:ssZ",
            // All of the above with reduced accuracy
            "yyyyMMddTHHmmzzz",
            "yyyyMMddTHHmmzz",
            "yyyyMMddTHHmmZ",
            "yyyy-MM-ddTHH:mmzzz",
            "yyyy-MM-ddTHH:mmzz",
            "yyyy-MM-ddTHH:mmZ",
            // Accuracy reduced to hours
            "yyyyMMddTHHzzz",
            "yyyyMMddTHHzz",
            "yyyyMMddTHHZ",
            "yyyy-MM-ddTHHzzz",
            "yyyy-MM-ddTHHzz",
            "yyyy-MM-ddTHHZ"
        };

        public static string PropertyTypeToString(PropertyDataType propertyType)
        {
            switch (propertyType)
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
                    return "uint";
                case PropertyDataType.Uint:
                    return "uint256";
                default:
                    return "string";
            }
        }

        public static string PropertyTypeToString(Property property, ContractConverter contractConverter)
        {
            var type = property.DataType;

            //Get the datatype name if reference
            if (type == PropertyDataType.Reference)
                return ToUpperCamelCase(contractConverter.GetDataType(property.ReferencedDataType).Name);
            else
                return PropertyTypeToString(type);
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
                    return "0";
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

        public static string ToLowerCamelCase(string name)
        {
            //remove spaces if any are present
            var trimmed = Regex.Replace(name, @"\s+", "");
            return trimmed.First().ToString().ToLower() + trimmed.Substring(1);
        }

        public static string ToUpperCamelCase(string name)
        {
            //remove spaces if any are present
            var trimmed = Regex.Replace(name, @"\s+", "");
            return trimmed.First().ToString().ToUpper() + trimmed.Substring(1);
        }


        public static DateTime ParseISO8601String(string str)
        {
            return DateTime.ParseExact(str, formats,
                CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

    }
}
