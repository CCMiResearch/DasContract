pragma solidity ^0.6.6;

import "https://github.com/OpenZeppelin/openzeppelin-contracts/blob/release-v3.1.0/contracts/access/Ownable.sol";
import "https://github.com/OpenZeppelin/openzeppelin-contracts/blob/release-v3.1.0/contracts/token/ERC721/ERC721.sol";


contract VotingToken is Ownable, ERC721{ 
	constructor(string memory name, string memory symbol) ERC721(name, symbol) Ownable() public payable{
	}

	function mint(address receiver) onlyOwner public {
		_safeMint(receiver, uint256(receiver));
	}

	function transfer(address from, address to) onlyOwner public {
		_transfer(from, to, uint256(from));
	}

 }

contract Contract { 
	

	uint256 call_Activity_1_VoteCounter;

	mapping(uint256 => mapping(string => bool)) Process_2_Call_Activity_1ActiveStates;

	function incrementPartyVote(address partyAddress) internal {
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
            require(candidates[votingChoices[i]].approved==true, "Candidate address is invalid");
            address currCandidateParty = candidates[votingChoices[i]].partyId;
            if(first) {
                firstCandidateParty = currCandidateParty;
                require(politicalParties[firstCandidateParty].countryId == countryId);
                first = false;
            }
            else {
                require(currCandidateParty == firstCandidateParty, "Candidates must be from the same party");
            }
            
        }
    }
    
    function checkClosedListVotingChoices(address[] memory votingChoices, uint256 countryId) internal view {
        require(votingChoices.length == 1, "Only a party vote is allowed");
        //Check whether the party is registered and is from the correct country
        PoliticalParty storage party = politicalParties[votingChoices[0]];
        require(party.id != address(0), "Party address is invalid");
        require(party.countryId == countryId);
    }
    
    function checkSingleTransferrableVotingChoices(address[] memory votingChoices, uint256 countryId) internal view {
        //Check whether the candidates are approved
        for (uint i = 0; i<votingChoices.length; i++){
            Candidate storage candidate = candidates[votingChoices[i]];
            require(candidate.approved == true, "Candidate address is invalid");
            require(politicalParties[candidate.partyId].countryId == countryId);
        }
    }

	uint256 call_Activity_1Counter;

	uint256 approveCandidatesCounter;

	uint256 user_Task_2Counter;

	uint256 registerNewPartyCounter;

	mapping(string => bool) Process_1ActiveStates;

	VotingToken votingToken;

	address[] addressHelper;

	uint candidateApprovalEnd;

	uint candidateRegistrationEnd;

	uint partyRegistrationEnd;

	uint startDate;

	mapping (address => Candidate) candidates;
	address[] candidatesKeys;

	mapping (address => PoliticalParty) politicalParties;
	address[] politicalPartiesKeys;

	CountryElections[] countries;

	enum VotingSystem{
		ClosedList,
		OpenList,
		SingleTransferable
	}

	struct CountryElections{
		uint256 id;
		string countryName;
		uint electionBeginDate;
		uint electionEndDate;
		address[] voters;
		VotingSystem votingSystem;
		int availableSeats;
		uint8 electoralTreshold;
		uint8 minimumAge;
	}

	struct PoliticalParty{
		address id;
		string name;
		string code;
		string website;
		int voteCount;
		int allocatedSeats;
		uint256 countryId;
	}

	struct Candidate{
		address id;
		string name;
		string website;
		int voteCount;
		bool hasSeat;
		bool approved;
		address partyId;
	}

	function isStateActiveProcess_1(string memory state) public view returns(bool){
		return Process_1ActiveStates[state];
	}

	constructor() public payable{
		votingToken = new VotingToken("VotingToken","VOTE");
		InitiateElections();
	}

	function InitiateElections() internal {
		candidateApprovalEnd = 9598986017;
	    candidateRegistrationEnd = 9598986017;
	    partyRegistrationEnd = 9598986017;
	    
		CountryElections memory czechElections = CountryElections(0,"Czech Republic", 9598986017, 959994017, new address[](0), VotingSystem.OpenList, 10, 20, 18);
        countries.push(czechElections);
        CountryElections memory germanElections = CountryElections(1,"Germany", 9598986017, 959994017, new address[](0), VotingSystem.ClosedList, 20, 40, 18);
        countries.push(germanElections);
		Process_1ActiveStates["RegisterNewParty"] = true;
	}

	modifier isRegisterNewPartyState{
		require(isStateActiveProcess_1("RegisterNewParty") == true);
		_;
	}

	function RegisterNewParty(string memory name, string memory code, string memory website, uint256 countryId) isRegisterNewPartyState() public {
		require(touchTimer_Boundary_Event_1());
		require(politicalParties[msg.sender].id == address(0));

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
        politicalPartiesKeys.push(msg.sender);
	}

	modifier isUser_Task_2State{
		require(isStateActiveProcess_1("User_Task_2") == true);
		_;
	}

	function User_Task_2(address partyId, string memory name, string memory website) isUser_Task_2State() public {
		require(touchTimer_Boundary_Event_2());
		PoliticalParty storage party = politicalParties[partyId];
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
        candidates[msg.sender] = candidate;
	}

	modifier isApproveCandidatesState{
		require(isStateActiveProcess_1("ApproveCandidates") == true);
		_;
	}

	function ApproveCandidates(address[] memory chosencandidates) isApproveCandidatesState() public {
		require(touchTimer_Boundary_Event_3());
		for(uint i = 0; i < chosencandidates.length; i++){
            Candidate storage c = candidates[chosencandidates[i]];
            //check if candidate exists and whether he is assigned to the political party
            if(bytes(c.name).length > 0 && c.partyId == msg.sender){
                c.approved = true;
            }
        }
		approveCandidatesCounter++;
		if(approveCandidatesCounter >= politicalPartiesKeys.length){
			Process_1ActiveStates["ApproveCandidates"] = false;
			Script_Task_2();
		}
	}

	function Script_Task_2() internal {
		CountryElections storage cz = countries[0];

        cz.voters.push(0x5B38Da6a701c568545dCfcB03FcB875f56beddC4);
        CountryElections storage ge = countries[1];
        ge.voters.push(0xAb8483F64d9C6d1EcF9b849Ae677dD3315835cb2); 
		Process_1ActiveStates["Call_Activity_1"] = true;
		Call_Activity_1();
	}

	function Call_Activity_1() internal {
		call_Activity_1Counter = 0;
		for(uint256 call_Activity_1Identifier = 0; call_Activity_1Identifier < countries.length; call_Activity_1Identifier++){ 
			Call_Activity_1_Script_Task_3(call_Activity_1Identifier);
		}
	}

	function Call_Activity_1ReturnLogic() internal {
		call_Activity_1Counter++;
		if(call_Activity_1Counter >= countries.length){
			Process_1ActiveStates["Call_Activity_1"] = false;
			Process_1ActiveStates["End_Event_1"] = true;
		}
	}

	function touchTimer_Boundary_Event_1() isRegisterNewPartyState() public returns(bool){
		if(now > partyRegistrationEnd){
			Process_1ActiveStates["RegisterNewParty"] = false;
			Process_1ActiveStates["User_Task_2"] = true;
			return false;
		}
		return true;
	}

	function touchTimer_Boundary_Event_2() isUser_Task_2State() public returns(bool){
		if(now > candidateRegistrationEnd){
			Process_1ActiveStates["User_Task_2"] = false;
			approveCandidatesCounter = 0;
			Process_1ActiveStates["ApproveCandidates"] = true;
			return false;
		}
		return true;
	}

	function touchTimer_Boundary_Event_3() isApproveCandidatesState() public returns(bool){
		if(now > candidateApprovalEnd){
			Process_1ActiveStates["ApproveCandidates"] = false;
			Script_Task_2();
			return false;
		}
		return true;
	}

	function isStateActiveProcess_2_Call_Activity_1(uint256 call_Activity_1Identifier, string memory state) public view returns(bool){
		return Process_2_Call_Activity_1ActiveStates[call_Activity_1Identifier][state];
	}

	function Call_Activity_1_Script_Task_3(uint256 call_Activity_1Identifier) internal {
		CountryElections storage country = countries[call_Activity_1Identifier];

        for (uint256 i = 0; i < country.voters.length; i++)
        {
           votingToken.mint(country.voters[i]);
        }
		Process_2_Call_Activity_1ActiveStates[call_Activity_1Identifier]["Call_Activity_1_Vote"] = true;
	}

	modifier isCall_Activity_1_VoteState(uint256 call_Activity_1Identifier){
		require(isStateActiveProcess_2_Call_Activity_1(call_Activity_1Identifier,"Call_Activity_1_Vote") == true);
		_;
	}

	function Call_Activity_1_Vote(uint256 call_Activity_1Identifier, address[] memory votingchoices) isCall_Activity_1_VoteState(call_Activity_1Identifier) public {
		require(touchCall_Activity_1_Timer_Boundary_Event_4(call_Activity_1Identifier));
		require(votingchoices.length > 0, "At least one cadidate must be chosen");
        //Check whether voting is open
        CountryElections storage country = countries[call_Activity_1Identifier];
        require(now > country.electionBeginDate, "Voting is currently not allowed");

        //Check the voting choices according to the given voting system
        if (country.votingSystem == VotingSystem.ClosedList)
        {
            checkClosedListVotingChoices(votingchoices, country.id);
        }
        else if (country.votingSystem == VotingSystem.OpenList)
        {
            checkOpenListVotingChoices(votingchoices, country.id);
            // incrementCandidatesVotes(votingChoices, country.id);
        }
        else if (country.votingSystem == VotingSystem.SingleTransferable)
        {
            checkSingleTransferrableVotingChoices(votingchoices, country.id);
            //incrementCandidatesVotes(votingChoices);
        }
        votingToken.transfer(msg.sender, address(this));

        if (country.votingSystem == VotingSystem.ClosedList)
            incrementPartyVote(votingchoices[0]);
        else
            incrementCandidatesVotes(votingchoices); 
	}

	function touchCall_Activity_1_Timer_Boundary_Event_4(uint256 call_Activity_1Identifier) isCall_Activity_1_VoteState(call_Activity_1Identifier) public returns(bool){
		if(now > countries[call_Activity_1Identifier].electionEndDate){
			Process_2_Call_Activity_1ActiveStates[call_Activity_1Identifier]["Call_Activity_1_Vote"] = false;
			Call_Activity_1_CountVotes(call_Activity_1Identifier);
			return false;
		}
		return true;
	}

	function Call_Activity_1_CountVotes(uint256 call_Activity_1Identifier) internal {
		
		Call_Activity_1_DistributeSeats(call_Activity_1Identifier);
	}

	function Call_Activity_1_DistributeSeats(uint256 call_Activity_1Identifier) internal {
		
		Call_Activity_1_Script_Task_6(call_Activity_1Identifier);
	}

	function Call_Activity_1_Script_Task_6(uint256 call_Activity_1Identifier) internal {
		
		Process_2_Call_Activity_1ActiveStates[call_Activity_1Identifier]["Call_Activity_1_End_Event_2"] = true;
		Call_Activity_1ReturnLogic();
	}

 }