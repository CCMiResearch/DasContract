using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Bonsai.Utils.Property
{
    public static class PropertyGetSetExtensions
    {
        /// <summary>
        /// Sets the property value using LambdaExpression
        /// </summary>
        /// <typeparam name="TClass">Type of the target model/class</typeparam>
        /// <typeparam name="TProperty">Type of the modified property</typeparam>
        /// <param name="target">The target model/class with modified property</param>
        /// <param name="propertySelector">The property expression</param>
        /// <param name="newValue">New value</param>
        public static void SetPropertyValue<TClass, TProperty>(this TClass target, LambdaExpression propertySelector, TProperty newValue)
        {
            if (propertySelector == null)
                throw new ArgumentNullException(nameof(propertySelector));

            if (propertySelector.Body is MemberExpression memberSelectorExpression)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                    property.SetValue(target, newValue, null);
            }
        }

        /// <summary>
        /// Gets the property value using LambdaExpression
        /// </summary>
        /// <typeparam name="TClass">Type of the target model/class</typeparam>
        /// <typeparam name="TProperty">Type of the property</typeparam>
        /// <param name="target">The target model/class</param>
        /// <param name="propertySelector">The property expression</param>
        /// <returns>Property value</returns>
        public static TProperty GetPropertyValue<TClass, TProperty>(this TClass target, LambdaExpression propertySelector)
        {
            if (propertySelector == null)
                throw new ArgumentNullException(nameof(propertySelector));

            if (propertySelector.Body is MemberExpression memberSelectorExpression)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                    return (TProperty)property.GetValue(target, null);
            }

            return default;
        }
    }
}
