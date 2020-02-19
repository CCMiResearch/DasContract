using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Bonsai.Utils.Property
{
    public static class PropertyAttributeGetter
    {

        //--------------------------------------------------
        //                     NAME
        //--------------------------------------------------

        /// <summary>
        /// Gets property Name in string
        /// </summary>
        /// <typeparam name="C">Type of the class</typeparam>
        /// <typeparam name="P">Type of the property</typeparam>
        /// <param name="propertyExpression">Property expression (f.e. (user => user.name))</param>
        /// <returns>Property "in-code" name</returns>
        public static string GetPropertyName<C, P>(this Expression<Func<C, P>> propertyExpression)
        {
            return ExtractName(GetMemberInfo(propertyExpression.Body));
        }

        /// <summary>
        /// Gets property Name in string
        /// </summary>
        /// <typeparam name="C">Type of the property</typeparam>
        /// <param name="propertyExpression">Property expression (f.e. (() => user.name))</param>
        /// <returns>Property "in-code" name</returns>
        public static string GetPropertyName<C>(this Expression<Func<C>> propertyExpression)
        {
            return ExtractName(GetMemberInfo(propertyExpression.Body));
        }

        /// <summary>
        /// Extracts "in-code" name from MemberInfo
        /// </summary>
        /// <param name="memberInfo">Member info</param>
        /// <returns>Attribute "in-code" Name</returns>
        private static string ExtractName(MemberInfo memberInfo)
        {
            //Return a value
            return memberInfo.Name;
        }

        //--------------------------------------------------
        //            [DISPLAYNAME] & [DISPLAY]
        //--------------------------------------------------

        /// <summary>
        /// Gets DisplayName or Display attribute in string
        /// </summary>
        /// <typeparam name="C">Type of the class</typeparam>
        /// <typeparam name="P">Type of the property</typeparam>
        /// <param name="propertyExpression">Property expression (f.e. (user => user.name))</param>
        /// <returns>DisplayName or Display attribut value, else property "in-code" name</returns>
        public static string GetDisplayName<C, P>(this Expression<Func<C, P>> propertyExpression)
        {
            return ExtractDisplayName(GetMemberInfo(propertyExpression.Body));
        }

        /// <summary>
        /// Gets DisplayName or Display attribute in string
        /// </summary>
        /// <typeparam name="C">Type of the property</typeparam>
        /// <param name="propertyExpression">Property expression (f.e. (() => user.name))</param>
        /// <returns>DisplayName or Display attribut value, else property "in-code" name</returns>
        public static string GetDisplayName<C>(this Expression<Func<C>> propertyExpression)
        {
            return ExtractDisplayName(GetMemberInfo(propertyExpression.Body));
        }

        /// <summary>
        /// Extracts Display or DisplayName attribute from the member info
        /// </summary>
        /// <param name="memberInfo">Member info</param>
        /// <returns>DisplayName or Display attribut value, else property "in-code" name</returns>
        private static string ExtractDisplayName(MemberInfo memberInfo)
        {
            //Get attribute
            var displayNameAttribute = memberInfo.GetAttribute<DisplayNameAttribute>();

            //Return a value
            if (displayNameAttribute != null)
                return displayNameAttribute.DisplayName;
            else
            {
                //Get alternative display
                var displayAttribute = memberInfo.GetAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                    return displayAttribute.Name;

                //Return property name
                else
                    return memberInfo.Name;
            }
        }

        //--------------------------------------------------
        //                 [DESCRIPTION]
        //--------------------------------------------------

        /// <summary>
        /// Gets Desctription attribute in string
        /// </summary>
        /// <typeparam name="C">Type of the class</typeparam>
        /// <typeparam name="P">Type of the property</typeparam>
        /// <param name="propertyExpression">Property expression (f.e. (user => user.name))</param>
        /// <returns>Desctription attribut value, else empty string</returns>
        public static string GetDescription<C, P>(this Expression<Func<C, P>> propertyExpression)
        {
            return ExtractDescription(GetMemberInfo(propertyExpression.Body));
        }

        /// <summary>
        /// Gets Desctription attribute in string
        /// </summary>
        /// <typeparam name="C">Type of the property</typeparam>
        /// <param name="propertyExpression">Property expression (f.e. (() => user.name))</param>
        /// <returns>Desctription attribut value, else empty string</returns>
        public static string GetDescription<C>(this Expression<Func<C>> propertyExpression)
        {
            return ExtractDescription(GetMemberInfo(propertyExpression.Body));
        }

        /// <summary>
        /// Extracts description attribute from the member info
        /// </summary>
        /// <param name="memberInfo">Member info</param>
        /// <returns>Description attribut value, else empty string</returns>
        private static string ExtractDescription(MemberInfo memberInfo)
        {
            //Get attribute
            var descriptionAttribute = memberInfo.GetAttribute<DescriptionAttribute>();

            //Return a value
            if (descriptionAttribute != null)
                return descriptionAttribute.Description;
            return "";
        }

        //--------------------------------------------------
        //                  [REQUIRED]
        //--------------------------------------------------
        /// <summary>
        /// Tells if a property has required attribute
        /// </summary>
        /// <typeparam name="C">Type of the class</typeparam>
        /// /// <typeparam name="C">Type of the property</typeparam>
        /// <param name="propertyExpression">Property expression (f.e. (user => user.name))</param>
        /// <returns>True if the property has Required attribute, else false</returns>
        public static bool HasRequiredAttribute<C, P>(this Expression<Func<C, P>> propertyExpression)
        {
            return ExtractRequired(GetMemberInfo(propertyExpression.Body));
        }

        /// <summary>
        /// Tells if a property has required attribute
        /// </summary>
        /// <typeparam name="C">Type of the property</typeparam>
        /// <param name="propertyExpression">Property expression (f.e. (() => user.name))</param>
        /// <returns>True if the property has Required attribute, else false</returns>
        public static bool HasRequiredAttribute<C>(this Expression<Func<C>> propertyExpression)
        {
            return ExtractRequired(GetMemberInfo(propertyExpression.Body));
        }

        /// <summary>
        /// Extracts Required attribute from MemberInfo
        /// </summary>
        /// <param name="memberInfo">Member info</param>
        /// <returns>True if member has required attribute, else false</returns>
        private static bool ExtractRequired(MemberInfo memberInfo)
        {
            //Get attribute
            var requiredAttribute = memberInfo.GetAttribute<RequiredAttribute>();

            //Return a value
            if (requiredAttribute == null)
                return false;
            return true;
        }

        //--------------------------------------------------
        //                 HELPER METHODS
        //--------------------------------------------------
        /// <summary>
        /// Returns member info from property expression
        /// </summary>
        /// <param name="propertyExpression">Property expression body (f.e. (user => user.name).Body)</param>
        /// <returns>Member info</returns>
        public static MemberInfo GetMemberInfo(Expression propertyExpression)
        {
            var memberExpr = propertyExpression as MemberExpression;
            if (memberExpr == null)
            {
                if (propertyExpression is UnaryExpression unaryExpr 
                    && unaryExpr.NodeType == ExpressionType.Convert)
                    memberExpr = unaryExpr.Operand as MemberExpression;
            }

            if (memberExpr != null)
                return memberExpr.Member;

            return null;
        }

        /// <summary>
        /// Getter for attribute of a MemberInfo
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="member">MemberInfo</param>
        /// <returns>Null if attribute not found, else attribute</returns>
        public static T GetAttribute<T>(this MemberInfo member)
            where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null)
                return null;

            return (T)attribute;
        }
    }
}
