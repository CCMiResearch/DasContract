using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Processes.Events
{
    public enum TimerDefinitionType
    {
        Date,
        Duration
    }

    public class TimerBoundaryEvent: BoundaryEvent
    {
        public TimerDefinitionType TimerDefinitionType { get; set; }
        /// <summary>
        /// Contains timer definition data based on the TimerDefinitionType
        /// For duration, the definition is in ISO 8601 Durations format.
        /// For date, the definition is in ISO 8601 Dates format.
        /// </summary>
        public string TimerDefinition { get; set; }
    }
}
