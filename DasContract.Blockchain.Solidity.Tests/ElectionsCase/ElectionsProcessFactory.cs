using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DasContract.Abstraction.UserInterface;

namespace DasContract.Blockchain.Solidity.Tests.ElectionsCase
{
    public static class ElectionsProcessFactory
    {
        public static Process CreateElectionsProcess()
        {
            var processElements = new List<ProcessElement>
            {
                CreateStartEvent(),
                CreateInitiateElectionsTask(),
                CreateRegisterNewPartyTask(),
                CreateRegisterCandidateTask(),
                CreateApproveCandidatesTask(),
                CreateStartCountryElections(),
                CreateCountryElectionsCallActivity(),
                CreateEndEvent()
            };
            processElements.AddRange(CreateBoundaryEvents());

            return new Process
            {
                Id = "Process_1",
                ProcessElements = processElements.ToDictionary(e => e.Id, e => e),
                SequenceFlows = CreateSequenceFlows().ToDictionary(e => e.Id, e => e)
            }; 

        }

        private static IList<SequenceFlow> CreateSequenceFlows()
        {
            return new List<SequenceFlow> 
            {
                new SequenceFlow
                {
                    Id = "Sequence_Flow_1",
                    SourceId = "Start_Event_1",
                    TargetId = "Script_Task_1"
                },
                new SequenceFlow
                {
                    Id = "Sequence_Flow_2",
                    SourceId = "Script_Task_1",
                    TargetId = "User_Task_1"
                },
                new SequenceFlow
                {
                    Id = "Sequence_Flow_3",
                    SourceId = "User_Task_1",
                    TargetId = "User_Task_2"
                },
                new SequenceFlow
                {
                    Id = "Sequence_Flow_4",
                    SourceId = "Timer_Boundary_Event_1",
                    TargetId = "User_Task_2"
                },
                new SequenceFlow
                {
                    Id = "Sequence_Flow_5",
                    SourceId = "User_Task_2",
                    TargetId = "User_Task_3"
                },
                new SequenceFlow
                {
                    Id = "Sequence_Flow_6",
                    SourceId = "Timer_Boundary_Event_2",
                    TargetId = "User_Task_3"
                },
                new SequenceFlow
                {
                    Id = "Sequence_Flow_7",
                    SourceId = "User_Task_3",
                    TargetId = "Script_Task_2"
                },
                new SequenceFlow
                {
                    Id = "Sequence_Flow_8",
                    SourceId = "Timer_Boundary_Event_3",
                    TargetId = "Script_Task_2"
                },
                new SequenceFlow
                {
                    Id = "Sequence_Flow_9",
                    SourceId = "Script_Task_2",
                    TargetId = "Call_Activity_1"
                },
                new SequenceFlow
                {
                    Id = "Sequence_Flow_10",
                    SourceId = "Call_Activity_1",
                    TargetId = "End_Event_1"
                }
            };
        }

        private static IList<TimerBoundaryEvent> CreateBoundaryEvents()
        {
            return new List<TimerBoundaryEvent>
            {
                new TimerBoundaryEvent
                {
                    Id = "Timer_Boundary_Event_1",
                    Outgoing = new List<string> { "Sequence_Flow_4" },
                    AttachedTo = "User_Task_1",
                    TimerDefinition = "2020-10-20", //TODO
                    TimerDefinitionType = TimerDefinitionType.Date
                },
                new TimerBoundaryEvent
                {
                    Id = "Timer_Boundary_Event_2",
                    Outgoing = new List<string> { "Sequence_Flow_6" },
                    AttachedTo = "User_Task_2",
                    TimerDefinition = "2020-10-21", //TODO
                    TimerDefinitionType = TimerDefinitionType.Date
                },
                new TimerBoundaryEvent
                {
                    Id = "Timer_Boundary_Event_3",
                    Outgoing = new List<string> { "Sequence_Flow_8" },
                    AttachedTo = "User_Task_3",
                    TimerDefinition = "2020-10-22", //TODO
                    TimerDefinitionType = TimerDefinitionType.Date
                },
            };

        }

        private static StartEvent CreateStartEvent()
        {
            return new StartEvent
            {
                Id = "Start_Event_1",
                Outgoing = new List<string> { "Sequence_Flow_1" }
            };
        }

        private static EndEvent CreateEndEvent()
        {
            return new EndEvent
            {
                Id = "End_Event_1",
                Incoming = new List<string> { "Sequence_Flow_10 " }
            };
        }

        private static CallActivity CreateCountryElectionsCallActivity()
        {
            return new CallActivity
            {
                Id = "Call_Activity_1",
                CalledElement = "", //TODO called process ID
                InstanceType = InstanceType.Parallel,
                Incoming = new List<string> { "Sequence_Flow_9" },
                Outgoing = new List<string> { "Sequence_Flow_10" },
                Name = "Country Elections"
            };
        }


        private static ScriptTask CreateStartCountryElections()
        {
            return new ScriptTask
            {
                Id = "Script_Task_2",
                Name = "Start Country Elections",
                Incoming = new List<string> { "Sequence_Flow_7", "Sequence_Flow_8" },
                Outgoing = new List<string> { "Sequence_Flow_9" },
                InstanceType = InstanceType.Single,
                Script = "" // TODO
            };
        }

        private static UserTask CreateApproveCandidatesTask()
        {
            var form = new UserForm
            {
                Id = "Form_4",
                Fields = new List<FormField>
                {
                    new FormField
                    {
                        Id = "Form_4_Field_1",
                        DisplayName = "chosen candidates",
                        Type = FormFieldType.Property,
                        PropertyExpression = "Property_11"
                    }
                }
            };
            return new UserTask
            {
                Id = "User_Task_3",
                Incoming = new List<string> { "Sequence_Flow_5", "Sequence_Flow_6" },
                Outgoing = new List<string> { "Sequence_Flow_7" },
                InstanceType = InstanceType.Parallel,
                Name = "Approve and Order Candidates",
                Form = form
            };
        }

        private static UserTask CreateRegisterCandidateTask()
        {
            var form = new UserForm
            {
                Id = "Form_3",
                Fields = new List<FormField>
                {
                    new FormField
                    {
                        Id = "Form_3_Field_1",
                        DisplayName = "Party Id",
                        Type = FormFieldType.Property,
                        PropertyExpression = "Property_11",
                        IsReadOnly = true
                    },
                    new FormField
                    {
                        Id = "Form_3_Field_2",
                        DisplayName = "name",
                        Type = FormFieldType.Property,
                        PropertyExpression = "Property_19"
                    },
                    new FormField
                    {
                        Id = "Form_3_Field_3",
                        DisplayName = "website",
                        Type = FormFieldType.Property,
                        PropertyExpression = "Property_20"
                    }
                }
            };
            return new UserTask
            {
                Id = "User_Task_2",
                Incoming = new List<string> { "Sequence_Flow_3", "Sequence_Flow_4" },
                Outgoing = new List<string> { "Sequence_Flow_5" },
                InstanceType = InstanceType.Parallel,
                Name = "Register New Party",
                Form = form
            };
        }

        private static UserTask CreateRegisterNewPartyTask()
        {
            var form = new UserForm
            {
                Id = "Form_2",
                Fields = new List<FormField>
                {
                    new FormField
                    {
                        Id = "Form_2_Field_1",
                        DisplayName = "Name",
                        Type = FormFieldType.Property,
                        PropertyExpression = "Property_13"
                    },
                    new FormField
                    {
                        Id = "Form_2_Field_2",
                        DisplayName = "Code",
                        Type = FormFieldType.Property,
                        PropertyExpression = "Property_14"
                    },
                    new FormField
                    {
                        Id = "Form_2_Field_3",
                        DisplayName = "Website",
                        Type = FormFieldType.Property,
                        PropertyExpression = "Property_15"
                    }
                }
            };
            var incoming = new List<string>()
            {
                "Sequence_Flow_2"
            };

            var outgoing = new List<string>()
            {
                "Sequence_Flow_3"
            };

            return new UserTask
            {
                Id = "User_Task_1",
                Incoming = incoming,
                Outgoing = outgoing,
                InstanceType = InstanceType.Parallel,
                Name = "Register New Party",
                Form = form
            };
        }

        private static ScriptTask CreateInitiateElectionsTask()
        {
            var incoming = new List<string>()
            {
                "Sequence_Flow_1"
            };

            var outgoing = new List<string>()
            {
                "Sequence_Flow_2"
            };

            return new ScriptTask
            {
                Id = "Script_Task_1",
                Name = "Initiate Elections",
                Incoming = incoming,
                Outgoing = outgoing,
                InstanceType = InstanceType.Single,
                Script = "" //TODO
            };
        }

        
    }
}
