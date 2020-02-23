using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Bonsai.Utils.String;
using Bonsai.Utils.Property;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CValueInput
{
    public class ValueInput<TProperty> : LoadableComponent
    {
        //--------------------------------------------------
        //             PROPERTY EXPRESSION
        //--------------------------------------------------
        [Parameter]
        public Expression<Func<TProperty>> PropertyExpression { get; set; }

        //--------------------------------------------------
        //                    NAME
        //--------------------------------------------------
        /// <summary>
        /// Name of this inputs property
        /// </summary>
        protected string Name
        {
            get
            {
                return PropertyExpression.GetPropertyName();
            }
        }

        //--------------------------------------------------
        //                     ID
        //--------------------------------------------------
        /// <summary>
        /// Inputs Id
        /// </summary>
        [Parameter]
        public string Id
        {
            get
            {
                if (id == null)
                    return Name.ToIdFriendly();
                return id;
            }
            set
            {
                id = value;
            }
        }
        private string id = null;


        //--------------------------------------------------
        //                    LABEL
        //--------------------------------------------------
        /// <summary>
        /// Inputs label text
        /// </summary>
        [Parameter]
        public string Label
        {
            get
            {
                if (label == null)
                    return PropertyExpression.GetDisplayName();
                return label;
            }
            set
            {
                label = value;
            }
        }
        string label = null;

        //--------------------------------------------------
        //                 PLACEHOLDER
        //--------------------------------------------------
        [Parameter]
        public string Placeholder
        {
            get
            {
                if (string.IsNullOrEmpty(placeholder))
                    return PropertyExpression.GetDisplayName();
                return placeholder;
            }
            set
            {
                placeholder = value;
            }
        }
        private string placeholder = "";

        //--------------------------------------------------
        //                  DESCRIPTION
        //--------------------------------------------------
        [Parameter]
        public string Description
        {
            get
            {
                if (string.IsNullOrEmpty(description))
                    return PropertyExpression.GetDescription();
                return description;
            }
            set
            {
                description = value;
            }
        }
        private string description = "";

        //--------------------------------------------------
        //                   REQUIRED
        //--------------------------------------------------
        /// <summary>
        /// Tells if this input is required
        /// </summary>
        [Parameter]
        public bool Required
        {
            get
            {
                if (required == null)
                    return PropertyExpression.HasRequiredAttribute();
                return (bool)required;
            }
            set
            {
                required = value;
            }
        }
        private bool? required = null;

        //--------------------------------------------------
        //                    READONLY
        //--------------------------------------------------

        /// <summary>
        /// Cascading parameter for ReadOnly
        /// </summary>
        [CascadingParameter(Name = "ReadOnly")]
        public bool ReadOnlyCascade { get; set; } = false;

        /// <summary>
        /// Parameter that tells if this input is read only
        /// </summary>
        [Parameter]
        public bool ReadOnly
        {
            get
            {
                return ReadOnlyCascade || readOnly;
            }
            set
            {
                readOnly = value;
            }
        }
        private bool readOnly = false;

        //--------------------------------------------------
        //                  VALIDATION
        //--------------------------------------------------
        /// <summary>
        /// Cascading parameter for EditContext
        /// </summary>
        [CascadingParameter(Name = "FormEditContext")]
        protected EditContext EditContextCascade { get; set; }

        /// <summary>
        /// Tells if this input is valid
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return GetState() != ValueInputState.Invalid;
        }

        /// <summary>
        /// Returns current input state (validation)
        /// </summary>
        /// <returns></returns>
        public ValueInputState GetState()
        {
            var currentField = EditContextCascade.Field(Name);
            //EditContextCascade.Validate();

            //The input is not modified
            if (!valueModified)
                return ValueInputState.NotModified;

            //There are some errro messages
            foreach (string validationMessage in EditContextCascade.GetValidationMessages(currentField))
                return ValueInputState.Invalid;

            //All is good
            return ValueInputState.Valid;
        }
        private bool valueModified = false;

        /// <summary>
        /// Returns inputs state class
        /// </summary>
        public string InputStateClass
        {
            get
            {
                return ValueInputStateHelper.ToClass(GetState());
            }
        }

        //--------------------------------------------------
        //               VALUE AND BINDING
        //--------------------------------------------------

        ///// <summary>
        ///// Direct connection to the models property
        ///// </summary>
        //private TProperty PropertyValue
        //{
        //    get
        //    {
        //        return EditContextCascade.Model.GetPropertyValue<object, TProperty>(PropertyExpression);
        //    }
        //    set
        //    {
        //        if (EditContextCascade != null)
        //            EditContextCascade.Model.SetPropertyValue(PropertyExpression, value);
        //        StateHasChanged();
        //    }
        //}

        /// <summary>
        /// Binding property for current input value value
        /// </summary>
        [Parameter]
        public TProperty Value { get; set; }

        /// <summary>
        /// Invoked on value change (primarily for binding)
        /// </summary>
        [Parameter]
        public EventCallback<TProperty> ValueChanged { get; set; }

        /// <summary>
        /// Properly changes the current value and invokes change callback
        /// </summary>
        /// <param name="change">Change event</param>
        /// <returns>Task</returns>
        protected async Task ChangeValueAsync(ChangeEventArgs change)
        {
            if (change == null)
                throw new ArgumentNullException(nameof(change));

            await ChangeValueAsync(Parse(change.Value));
        }

        /// <summary>
        /// Properly changes the current value and invokes change callback
        /// </summary>
        /// <param name="newValue">New value</param>
        /// <returns>Task</returns>
        protected async Task ChangeValueAsync(object newValue)
        {
            await ChangeValueAsync(Parse(newValue));
        }

        /// <summary>
        /// Properly changes the current value and invokes change callback
        /// </summary>
        /// <param name="newValue">New value</param>
        /// <returns>Task</returns>
        protected async Task ChangeValueAsync(TProperty newValue)
        {
            //Check for read only
            if (ReadOnly)
                return;

            //Set the new value
            Value = newValue;

            //Invoke change callback
            await ValueChanged.InvokeAsync(Value);

            //Validate
            EditContextCascade.Validate();

            //Changed flag
            valueModified = true;
        }

        /// <summary>
        /// Parses recieved object into the TProperty object
        /// </summary>
        /// <param name="value">Input value</param>
        /// <returns>Parsed TProperty</returns>
        protected virtual TProperty Parse(object value)
        {
            return (TProperty)value;
        }
    }
}
