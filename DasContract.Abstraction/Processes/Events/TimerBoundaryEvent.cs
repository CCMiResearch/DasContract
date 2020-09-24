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
        /// TODO
        /// </summary>
        public string TimerDefinition { get; set; }
    }
}
