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
        public static string solidityValidationScript = @"function incrementPartyVote(address partyAddress) internal {
        PoliticalParty storage party = politicalParties[partyAddress];
        party.voteCount += 1;
    }

    function incrementCandidatesVotes(address[] memory candidateAddresses) internal {
        
        for(uint i = 0; i<candidateAddresses.length; i++){
            Candidate storage candidate = candidates[candidateAddresses[i]];
            candidate.voteCount += 1;
        }
        
    }
    
    function checkOpenListVotingChoices(address[] memory votingChoices, uint256 countryId) internal view {
        address firstCandidateParty;
        bool first = true;
        
        //Check whether the candidates are approved AND are from the same party AND represent the correct country
        for (uint i = 0; i<votingChoices.length; i++){
            require(candidates[votingChoices[i]].approved==true, ""Candidate address is invalid"");
            address currCandidateParty = candidates[votingChoices[i]].partyId;
            if(first) {
                firstCandidateParty = currCandidateParty;
                require(politicalParties[firstCandidateParty].countryId == countryId);
                first = false;
            }
            else {
                require(currCandidateParty == firstCandidateParty, ""Candidates must be from the same party"");
            }
            
        }
    }
    
    function checkClosedListVotingChoices(address[] memory votingChoices, uint256 countryId) internal view {
        require(votingChoices.length == 1, ""Only a party vote is allowed"");
        //Check whether the party is registered and is from the correct country
        PoliticalParty storage party = politicalParties[votingChoices[0]];
        require(party.id != address(0), ""Party address is invalid"");
        require(party.countryId == countryId);
    }
    
    function checkSingleTransferrableVotingChoices(address[] memory votingChoices, uint256 countryId) internal view {
        //Check whether the candidates are approved
        for (uint i = 0; i<votingChoices.length; i++){
            Candidate storage candidate = candidates[votingChoices[i]];
            require(candidate.approved == true, ""Candidate address is invalid"");
            require(politicalParties[candidate.partyId].countryId == countryId);
        }
    }";

        public static Process CreateElectionsProcess()
        {
            var processElements = new List<ProcessElement>
            {
                CreateStartEvent(),
                CreateInitiateElectionsTask(),
                //CreateRegisterNewPartyTask(),
                //CreateRegisterCandidateTask(),
                //CreateApproveCandidatesTask(),
                CreateStartCountryElections(),
                CreateCountryElectionsCallActivity(),
                CreateEndEvent()
            };
            processElements.AddRange(CreateBoundaryEvents());

            return new Process
            {
                Id = "Process_1",
                IsExecutable = true,
                ProcessElements = processElements.ToDictionary(e => e.Id, e => e),
                SequenceFlows = CreateSequenceFlows().ToDictionary(e => e.Id, e => e),
                CustomScript = solidityValidationScript
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
                    TimerDefinition = "${partyRegistrationEnd}", 
                    TimerDefinitionType = TimerDefinitionType.Date
                },
                new TimerBoundaryEvent
                {
                    Id = "Timer_Boundary_Event_2",
                    Outgoing = new List<string> { "Sequence_Flow_6" },
                    AttachedTo = "User_Task_2",
                    TimerDefinition = "${candidateRegistrationEnd}",
                    TimerDefinitionType = TimerDefinitionType.Date
                },
                new TimerBoundaryEvent
                {
                    Id = "Timer_Boundary_Event_3",
                    Outgoing = new List<string> { "Sequence_Flow_8" },
                    AttachedTo = "User_Task_3",
                    TimerDefinition = "${candidateApprovalEnd}", 
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
                CalledElement = "Process_2",
                InstanceType = InstanceType.Parallel,
                LoopCollection = "Property_27",
                Incoming = new List<string> { "Sequence_Flow_9" },
                Outgoing = new List<string> { "Sequence_Flow_10" },
                Name = "Country Elections CallActivity"
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
                Script = @"CountryElections storage cz = countries[0];

        cz.voters.push(0x5B38Da6a701c568545dCfcB03FcB875f56beddC4);
        CountryElections storage ge = countries[1];
        ge.voters.push(0xAb8483F64d9C6d1EcF9b849Ae677dD3315835cb2); "
            };
        }
        /*
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
                        PropertyExpression = "Property_100"
                    }
                }
            };
            return new UserTask
            {
                Id = "User_Task_3",
                Incoming = new List<string> { "Sequence_Flow_5", "Sequence_Flow_6" },
                Outgoing = new List<string> { "Sequence_Flow_7" },
                InstanceType = InstanceType.Parallel,
                LoopCollection = "Property_25",
                Name = "Approve Candidates",
                Form = form,
                ValidationScript = @"for(uint i = 0; i < chosencandidates.length; i++){
            Candidate storage c = candidates[chosencandidates[i]];
            //check if candidate exists and whether he is assigned to the political party
            if(bytes(c.name).length > 0 && c.partyId == msg.sender){
                c.approved = true;
            }
        }"
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
                        PropertyExpression = "Property_12",
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
                LoopCardinality = -1,
                Name = "Register New Candidate",
                Form = form,
                ValidationScript = @"PoliticalParty storage party = politicalParties[partyId];
        //check whether a party exists
        require(bytes(party.name).length > 0);
        //check whether a candidate is not already registered to this address
        require(candidates[msg.sender].id == address(0));
        
        Candidate memory candidate = Candidate({
            id: msg.sender,
            name: name,
            website: website, 
            voteCount: 0, 
            hasSeat: false,
            approved: false,
            partyId: party.id
        });
        
        candidatesKeys.push(msg.sender);
        candidates[msg.sender] = candidate;"
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
                    },
                    new FormField
                    {
                        Id = "Form_2_Field_4",
                        DisplayName = "CountryId",
                        Type = FormFieldType.Property,
                        PropertyExpression = "Property_51"
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
                LoopCardinality = -1,
                Name = "Register New Party",
                Form = form,
                ValidationScript = @"require(politicalParties[msg.sender].id == address(0));

        PoliticalParty memory party = PoliticalParty({
            id: msg.sender,
            name: name,
            code: code,
            website: website,
            voteCount: 0,
            allocatedSeats: 0,
            countryId: countryId
        });
        politicalParties[msg.sender] = party;
        politicalPartiesKeys.push(msg.sender);"
            };
        }
        */

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
                Script = @"candidateApprovalEnd = 9598986017;
	    candidateRegistrationEnd = 9598986017;
	    partyRegistrationEnd = 9598986017;
	    
		CountryElections memory czechElections = CountryElections(0,""Czech Republic"", 9598986017, 959994017, new address[](0), VotingSystem.OpenList, 10, 20, 18);
        countries.push(czechElections);
        CountryElections memory germanElections = CountryElections(1,""Germany"", 9598986017, 959994017, new address[](0), VotingSystem.ClosedList, 20, 40, 18);
        countries.push(germanElections);"
            };
        }

        
    }
}
