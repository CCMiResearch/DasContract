using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
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

        public static string PrimitivePropertyTypeToString(PropertyDataType propertyType)
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
                case PropertyDataType.Byte:
                    return "uint8";
                default:
                    return "string";
            }
        }

        public static string PropertyTypeToString(Property property, ContractConverter contractConverter)
        {
            var type = property.DataType;

            //Get the datatype name if reference
            if (type == PropertyDataType.Reference)
                return contractConverter.GetDataType(property.ReferencedDataType).ToStructureName();
            else
                return PrimitivePropertyTypeToString(type);
        }

        public static string ToLowerCamelCase(this string name)
        {
            //remove spaces if any are present
            var trimmed = Regex.Replace(name, @"\s+", "");
            return trimmed.First().ToString().ToLower() + trimmed.Substring(1);
        }

        public static string ToUpperCamelCase(this string name)
        {
            //remove spaces if any are present
            var trimmed = Regex.Replace(name, @"\s+", "");
            return trimmed.First().ToString().ToUpper() + trimmed.Substring(1);
        }


        public static DateTime ParseISO8601String(this string str)
        {
            return DateTime.ParseExact(str, formats,
                CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public static string ToVariableName(this DataType dataType)
        {
            if(dataType.Name != null)
            {
                var lowerCaseAndTrimmed = dataType.Name.ToLowerCamelCase();
                if (lowerCaseAndTrimmed.Length > 0)
                    return lowerCaseAndTrimmed;
            }
            return dataType.Id.ToLowerCamelCase();
        }

        public static string ToStructureName(this DataType dataType)
        {
            if (dataType.Name != null)
            {
                var upperCaseAndTrimmed = dataType.Name.ToUpperCamelCase();
                if (upperCaseAndTrimmed.Length > 0)
                    return upperCaseAndTrimmed;
            }
            return dataType.Id.ToUpperCamelCase();
        }


    }
}
