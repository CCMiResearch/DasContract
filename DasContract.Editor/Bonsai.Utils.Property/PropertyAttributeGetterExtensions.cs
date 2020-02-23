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
        /// <typeparam name="TClass">Type of the class</typeparam>
        /// <typeparam name="TProperty">Type of the property</typeparam>
        /// <param name="propertyExpression">Property expression (f.e. (user => user.name))</param>
        /// <returns>Property "in-code" name</returns>
        public static string GetPropertyName<TClass, TProperty>(this Expression<Func<TClass, TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            return ExtractName(GetMemberInfo(propertyExpression.Body));
        }

        /// <summary>
        /// Gets property Name in string
        /// </summary>
        /// <typeparam name="TProperty">Type of the property</typeparam>
        /// <param name="propertyExpression">Property expression (f.e. (() => user.name))</param>
        /// <returns>Property "in-code" name</returns>
        public static string GetPropertyName<TProperty>(this Expression<Func<TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

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
        /// <typeparam name="TClass">Type of the class</typeparam>
        /// <typeparam name="TProperty">Type of the property</typeparam>
        /// <param name="propertyExpression">Property expression (f.e. (user => user.name))</param>
        /// <returns>DisplayName or Display attribut value, else property "in-code" name</returns>
        public static string GetDisplayName<TClass, TProperty>(this Expression<Func<TClass, TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            return ExtractDisplayName(GetMemberInfo(propertyExpression.Body));
        }

        /// <summary>
        /// Gets DisplayName or Display attribute in string
        /// </summary>
        /// <typeparam name="TProperty">Type of the property</typeparam>
        /// <param name="propertyExpression">Property expression (f.e. (() => user.name))</param>
        /// <returns>DisplayName or Display attribut value, else property "in-code" name</returns>
        public static string GetDisplayName<TProperty>(this Expression<Func<TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

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
        /// <typeparam name="TClass">Type of the class</typeparam>
        /// <typeparam name="TProperty">Type of the property</typeparam>
        /// <param name="propertyExpression">Property expression (f.e. (user => user.name))</param>
        /// <returns>Desctription attribut value, else empty string</returns>
        public static string GetDescription<TClass, TProperty>(this Expression<Func<TClass, TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            return ExtractDescription(GetMemberInfo(propertyExpression.Body));
        }

        /// <summary>
        /// Gets Desctription attribute in string
        /// </summary>
        /// <typeparam name="TProperty">Type of the property</typeparam>
        /// <param name="propertyExpression">Property expression (f.e. (() => user.name))</param>
        /// <returns>Desctription attribut value, else empty string</returns>
        public static string GetDescription<TProperty>(this Expression<Func<TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

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
        /// <typeparam name="TClass">Type of the class</typeparam>
        /// /// <typeparam name="TClass">Type of the property</typeparam>
        /// <param name="propertyExpression">Property expression (f.e. (user => user.name))</param>
        /// <returns>True if the property has Required attribute, else false</returns>
        public static bool HasRequiredAttribute<TClass, TProperty>(this Expression<Func<TClass, TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            return ExtractRequired(GetMemberInfo(propertyExpression.Body));
        }

        /// <summary>
        /// Tells if a property has required attribute
        /// </summary>
        /// <typeparam name="TProperty">Type of the property</typeparam>
        /// <param name="propertyExpression">Property expression (f.e. (() => user.name))</param>
        /// <returns>True if the property has Required attribute, else false</returns>
        public static bool HasRequiredAttribute<TProperty>(this Expression<Func<TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

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
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null)
                return null;

            return (T)attribute;
        }
    }
}
