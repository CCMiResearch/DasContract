using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DasContract.Editor.Migrator.Migrations.GetSet
{
    public class GetSetValueMigration<TProperty>: Migration
    {

        public Func<TProperty> PropertyGetter { get; set; }

        public Action<TProperty> PropertySetter { get; set; }

        public TProperty LastKnownValue { get; set; }

        protected bool IsUp { get; set; } = true;

        public GetSetValueMigration(Expression propertyExpression, 
            Func<TProperty> propertyGetter, 
            Action<TProperty> propertySetter)
        {
            PropertyExpression = propertyExpression;
            PropertyGetter = propertyGetter;
            PropertySetter = propertySetter;
            UpdateLastKnownValue();
        }

        /// <summary>
        /// Tells if the last known value is different from the current value
        /// </summary>
        /// <returns></returns>
        public bool HasValueChanged()
        {
            if (Equals(GetCurrentValue(), LastKnownValue))
                return false;
            return true;
        }

        /// <summary>
        /// Updates the last known value to the current value
        /// </summary>
        public void UpdateLastKnownValue()
        {
            LastKnownValue = GetCurrentValue();
        }

        public override void Down()
        {
            if (!IsUp)
                return;

            IsUp = false;
            RevertLastKnownValue();
        }

        public override void Up()
        {
            if (IsUp)
                return;

            IsUp = true;
            RevertLastKnownValue();
        }

        public void RevertLastKnownValue()
        {
            var temp = LastKnownValue;
            LastKnownValue = GetCurrentValue();
            SetNewValue(temp);
        }

        /// <summary>
        /// Compiles the getter expression and returns the current value
        /// </summary>
        /// <returns>The current value</returns>
        TProperty GetCurrentValue()
        {
            return PropertyGetter();
        }

        /// <summary>
        /// Compiles the setter expression and sets a new value
        /// </summary>
        /// <param name="newValue">The new value</param>
        void SetNewValue(TProperty newValue)
        {
            PropertySetter(newValue);
        }
    }
}
