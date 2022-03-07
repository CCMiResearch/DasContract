using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Processes
{
    public static class ElementNames
    {
        public const string TASK = "Task";
        public const string USER_TASK = "UserTask";
        public const string SERVICE_TASK = "ServiceTask";
        public const string CALL_ACTIVITY = "CallActivity";
        public const string SCRIPT_TASK = "ScriptTask";
        public const string BUSINESS_RULE_TASK = "BusinessRuleTask";
        public const string EVENT = "Event";
        public const string END_EVENT = "EndEvent";
        public const string START_EVENT = "StartEvent";
        public const string TIMER_BOUNDARY_EVENT = "TimerBoundaryEvent";
        public const string BOUNDARY_EVENT = "BoundaryEvent";
        public const string EXCLUSIVE_GATEWAY = "EclusiveGateway";
        public const string PARALLEL_GATEWAY = "ParallelGateway";
        public const string GATEWAY = "Gateway";

        public const string ENUM = "Enum";
        public const string ENTITY = "Entity";
        public const string TOKEN = "Token";
    }
}
