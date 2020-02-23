using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Bonsai.Utils.Property
{
    public static class EnumAttributeGetterExtensions
    {
        /// <summary>
        /// Extracts Display attribute name from an enum
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns>Display attribute name or "in-code" name</returns>
        public static string GetDisplayName(this Enum enumValue)
        {
            if (enumValue == null)
                throw new ArgumentNullException(nameof(enumValue));

            var memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            if (memberInfo == null)
                return null;

            //Display attribute
            var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null)
                return displayAttribute.Name;

            //Enum value in string
            return enumValue.ToString();

        }
    }
}
