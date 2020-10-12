using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace DasContract.Blockchain.Solidity.Tests.ElectionsCase
{
    public static class CountryElectionsProcessFactory
    {
        public static Process CreateCountryElectionsProcess()
        {
            return new Process
            {
                Id = "Process_2",
                ProcessElements = new List<ProcessElement>
                {
                    CreateStartEvent(),
                    CreateSendBallotsTask(),
                    CreateVoteTask(),
                    CreateTimerBoundaryEvent(),
                    CreateCountVotesTask(),
                    CreateDistributeSeatsTask(),
                    CreateAssignPrivileguesTask(),
                    CreateEndEvent()
                }.ToDictionary(e => e.Id, e => e),
                SequenceFlows = CreateSequenceFlows().ToDictionary(e => e.Id, e => e)
            };
        }

        private static IList<SequenceFlow> CreateSequenceFlows()
        {
            return new List<SequenceFlow>
            {
                new SequenceFlow
                {
                    Id = "Sequence_Flow_11",
                    SourceId = "Start_Event_2",
                    TargetId = "Script_Task_3"
                },
                new SequenceFlow
                {
                    Id = "Sequence_Flow_12",
                    SourceId = "Script_Task_3",
                    TargetId = "User_Task_4"
                },
                new SequenceFlow
                {
                    Id = "Sequence_Flow_13",
                    SourceId = "User_Task_4",
                    TargetId = "Script_Task_4"
                },
                new SequenceFlow
                {
                    Id = "Sequence_Flow_14",
                    SourceId = "Timer_Boundary_Event_4",
                    TargetId = "Script_Task_4"
                },
                new SequenceFlow
                {
                    Id = "Sequence_Flow_15",
                    SourceId = "Script_Task_4",
                    TargetId = "Script_Task_5"
                },
                new SequenceFlow
                {
                    Id = "Sequence_Flow_16",
                    SourceId = "Script_Task_5",
                    TargetId = "Script_Task_6"
                },
                new SequenceFlow
                {
                    Id = "Sequence_Flow_17",
                    SourceId = "Script_Task_6",
                    TargetId = "End_Event_2"
                },
            };
        }

        private static TimerBoundaryEvent CreateTimerBoundaryEvent()
        {
            return new TimerBoundaryEvent
            {
                Id = "Timer_Boundary_Event_4",
                Outgoing = new List<string> { "Sequence_Flow_14" },
                TimerDefinitionType = TimerDefinitionType.Date,
                AttachedTo = "User_Task_4"
            };
        }

        private static EndEvent CreateEndEvent()
        {
            return new EndEvent
            {
                Id = "End_Event_2",
                Incoming = new List<string> { "Sequence_Flow_17" }
            };
        }

        private static StartEvent CreateStartEvent()
        {
            return new StartEvent
            {
                Id = "Start_Event_2",
                Outgoing = new List<string> { "Sequence_Flow_11" }
            };
        }

        private static ScriptTask CreateAssignPrivileguesTask()
        {
            return new ScriptTask
            {
                Id = "Script_Task_6",
                Incoming = new List<string> { "Sequence_Flow_16 " },
                Outgoing = new List<string> { "Sequence_Flow_17 " },
                InstanceType = InstanceType.Single,
                Name = "Assign Privilegues to the Elected Candidates",
                Script = "" //TODO
            };
        }

        private static ScriptTask CreateDistributeSeatsTask()
        {
            return new ScriptTask
            {
                Id = "Script_Task_5",
                Incoming = new List<string> { "Sequence_Flow_15 " },
                Outgoing = new List<string> { "Sequence_Flow_16 " },
                InstanceType = InstanceType.Single,
                Name = "Distribute Seats",
                Script = "" //TODO
            };
        }

        private static ScriptTask CreateCountVotesTask()
        {
            return new ScriptTask
            {
                Id = "Script_Task_4",
                Incoming = new List<string> { "Sequence_Flow_13 ", "Sequence_Flow_14" },
                Outgoing = new List<string> { "Sequence_Flow_15 " },
                InstanceType = InstanceType.Single,
                Name = "Count Votes",
                Script = "" //TODO
            };
        }

        private static UserTask CreateVoteTask()
        {
            return new UserTask
            {
                Id = "User_Task_4",
                Incoming = new List<string> { "Sequence_Flow_12 " },
                Outgoing = new List<string> { "Sequence_Flow_13 " },
                InstanceType = InstanceType.Parallel,
                Name = "Vote",
                OperationType = TokenOperationType.Send,
                Form = new Abstraction.UserInterface.UserForm()
            };
        }

        private static ScriptTask CreateSendBallotsTask()
        {
            return new ScriptTask
            {
                Id = "Script_Task_3",
                Incoming = new List<string> { "Sequence_Flow_11 " },
                Outgoing = new List<string> { "Sequence_Flow_12 " },
                InstanceType = InstanceType.Single,
                Name = "Send ballots to the elegible EU Citizens",
                OperationType = TokenOperationType.Create,
                Script = "", //TODO
            };
        }
    }
}
