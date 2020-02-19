using System;
using System.Linq.Expressions;

namespace Bonsai.Utils.Property
{
    public class PropertyRecorder
    {
        /// <summary>
        /// Property expression of the tracked property
        /// </summary>
        private LambdaExpression PropertyExpression { get; set; }

        /// <summary>
        /// Current Model/Instance containing the property
        /// </summary>
        private object Model { get; set; }

        /// <summary>
        /// Sets/Gets recording status
        /// </summary>
        public bool Recording
        {
            get
            {
                return recording;
            }
            set
            {
                recording = value;
                firstRecording = true;
                if (value == true)
                    OriginalValue = CurrentValue;
            }
        }
        private bool recording = false;
        private bool firstRecording = false;

        /// <summary>
        /// Returns value saved on recording start
        /// </summary>
        private object OriginalValue { get; set; } = null;

        /// <summary>
        /// Returns the current model (live) value
        /// </summary>
        private object CurrentValue
        {
            get
            {
                return Model.GetPropertyValue<object, object>(PropertyExpression);
            }
        }

        /// <summary>
        /// Property recorder
        /// </summary>
        /// <param name="model">Current instance of the class containing tracked property</param>
        /// <param name="propertyExpression">Property expression</param>
        public PropertyRecorder(object model, LambdaExpression propertyExpression)
        {
            Model = model;
            PropertyExpression = propertyExpression;
        }

        /// <summary>
        /// Starts the recording
        /// </summary>
        public PropertyRecorder StartRecording()
        {
            Recording = true;
            return this;
        }

        /// <summary>
        /// Stops the recording
        /// </summary>
        public PropertyRecorder StopRecording()
        {
            Recording = false;
            return this;
        }

        /// <summary>
        /// Tells if the value has changed since the recording started. Returns false is recording has not ever started. 
        /// If recording started and is stopped now, the method compares value stored on the last recording beginning. 
        /// </summary>
        /// <returns></returns>
        public bool ValueChanged()
        {
            if (!firstRecording)
                return false;
            if (OriginalValue == null)
            {
                if (CurrentValue == null)
                    return false;
                return true;
            }
            return !OriginalValue.Equals(CurrentValue);
        }

    }
}
