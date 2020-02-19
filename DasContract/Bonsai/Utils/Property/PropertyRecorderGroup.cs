using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bonsai.Utils.Property
{
    public class PropertyRecorderGroup
    {
        private List<PropertyRecorder> Recorders { get; } = new List<PropertyRecorder>();

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
                Recorders.ForEach(e =>
                {
                    e.Recording = value;
                });

            }
        }
        private bool recording = false;

        /// <summary>
        /// Starts the recording
        /// </summary>
        public PropertyRecorderGroup StartRecording()
        {
            Recording = true;
            return this;
        }

        /// <summary>
        /// Stops the recording
        /// </summary>
        public PropertyRecorderGroup StopRecording()
        {
            Recording = false;
            return this;
        }

        /// <summary>
        /// Inserts new recorder into the group
        /// </summary>
        /// <param name="recorder">The recorder</param>
        /// <returns>This</returns>
        public PropertyRecorderGroup AddRecorder(PropertyRecorder recorder)
        {
            Recorders.Add(recorder);
            return this;
        }

        /// <summary>
        /// Creates and inserts new recorder into the group
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="propertyExpression">The property expression</param>
        /// <returns>This</returns>
        public PropertyRecorderGroup AddRecorder(object model, LambdaExpression propertyExpression)
        {
            return AddRecorder(new PropertyRecorder(model, propertyExpression));
        }

        /// <summary>
        /// Tells if some recordes ValueChanged returns true 
        /// </summary>
        /// <returns></returns>
        public bool SomeValueChanged()
        {
            return Recorders.Any(e => e.ValueChanged());
        }

    }
}
