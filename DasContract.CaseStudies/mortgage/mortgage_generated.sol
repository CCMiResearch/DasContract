pragma solidity ^0.6.6;

import "https://github.com/OpenZeppelin/openzeppelin-contracts/blob/release-v3.1.0/contracts/token/ERC721/IERC721.sol";

contract GeneratedContract { 
	int Gateway_1whiz9yIncoming = 0;

	mapping (string => address) public addressMapping;

	mapping (string => bool) public ActiveStates;

	IERC721 houseTokenAddress = IERC721(address(0x93b6784CE509d1cEA255aE8269af4Ee258943Bd0));

	struct Mortgage{
		uint256 propertyAddress;
		uint256 propertyPrice;
		uint256 rate;
		uint256 downPaymentValue;
		uint256 mortgageDurationMonths;
		uint256 totalPaid;
		Escrow escrow;
		mapping (uint => Payment) payments;
		uint paymentsLength;
	}
	Mortgage mortgage = Mortgage({propertyAddress: 0, propertyPrice: 0, rate: 0, downPaymentValue: 0, mortgageDurationMonths: 0, totalPaid: 0, escrow: Escrow({identification: address(0x0)}), paymentsLength: 0});

	struct Insurance{
		bool isEnsured;
		string additionalInformation;
	}
	Insurance insurance = Insurance({isEnsured: false, additionalInformation: ""});

	struct Escrowagreement{
		bool agreeToEscrow;
		Escrow escrow;
	}
	Escrowagreement escrowagreement = Escrowagreement({agreeToEscrow: false, escrow: Escrow({identification: address(0x0)})});

	struct Escrow{
		address payable identification;
	}
	Escrow escrow = Escrow({identification: address(0x0)});

	struct Mortgagecancellation{
		string reason;
		bool confirmation;
	}
	Mortgagecancellation mortgagecancellation = Mortgagecancellation({reason: "", confirmation: false});

	struct Mortgagedefaultrequest{
		string reason;
	}
	Mortgagedefaultrequest mortgagedefaultrequest = Mortgagedefaultrequest({reason: ""});

	struct Payment{
		uint256 amount;
		bool onSchedule;
		address sender;
		address receiver;
	}
	Payment payment = Payment({amount: 0, onSchedule: false, sender: address(0x0), receiver: address(0x0)});

	struct Indemnity{
		bool valid;
	}
	Indemnity indemnity = Indemnity({valid: false});

	struct Contractvalidation{
		bool valid;
		bool cancelable;
	}
	Contractvalidation contractvalidation = Contractvalidation({valid: false, cancelable: false});

	function isStateActive(string memory state) public returns(bool){
		return ActiveStates[state];
	}

	constructor () public payable{
		ActiveStates["ApplyForaMortgagePayable"] = true;
	}

	modifier isApplyForaMortgagePayableAuthorized{
		if(addressMapping["Borrower"] == address(0x0)){
			addressMapping["Borrower"] = msg.sender;
		}
		require(msg.sender==addressMapping["Borrower"]);
		_;
	}

	modifier isApplyForaMortgagePayableState{
		require(isStateActive("ApplyForaMortgagePayable")==true);
		_;
	}

	function ApplyForaMortgagePayable(uint256 PropertyAddress, uint256 PropertyPrice, uint256 Rate, uint256 DownPayment, address payable EscrowID, uint256 MortgageDurationMonths) payable isApplyForaMortgagePayableState isApplyForaMortgagePayableAuthorized public {
		ActiveStates["ApplyForaMortgagePayable"] = false;
		mortgage.propertyAddress = PropertyAddress;
		mortgage.propertyPrice = PropertyPrice;
		mortgage.rate = Rate;
		mortgage.downPaymentValue = DownPayment;
		escrow.identification = EscrowID;
		mortgage.mortgageDurationMonths = MortgageDurationMonths;
		Gateway_0dznqqsLogic();
	}

	function Gateway_0dznqqsLogic() internal {
		Gateway_1ps7u6wLogic();
		ActiveStates["CancelApplication"] = true;
	}

	function Gateway_1ps7u6wLogic() internal {
		ActiveStates["EscrowPropertyRights"] = true;
		ActiveStates["AcceptInsurance"] = true;
		ActiveStates["EscrowMoneyPayable"] = true;
	}

	modifier isCancelApplicationAuthorized{
		if(addressMapping["Borrower"] == address(0x0)){
			addressMapping["Borrower"] = msg.sender;
		}
		require(msg.sender==addressMapping["Borrower"]);
		_;
	}

	modifier isCancelApplicationState{
		require(isStateActive("CancelApplication")==true);
		_;
	}

	function CancelApplication(string memory Reason, bool Confirmation) isCancelApplicationState isCancelApplicationAuthorized public {
		ActiveStates["CancelApplication"] = false;
		mortgagecancellation.reason = Reason;
		mortgagecancellation.confirmation = Confirmation;
		Gateway_1wbe662Logic();
	}

	modifier isEscrowPropertyRightsAuthorized{
		if(addressMapping["Property Owner"] == address(0x0)){
			addressMapping["Property Owner"] = msg.sender;
		}
		require(msg.sender==addressMapping["Property Owner"]);
		_;
	}

	modifier isEscrowPropertyRightsState{
		require(isStateActive("EscrowPropertyRights")==true);
		_;
	}

	function EscrowPropertyRights(bool Agree) isEscrowPropertyRightsState isEscrowPropertyRightsAuthorized public {
		ActiveStates["EscrowPropertyRights"] = false;
		escrowagreement.agreeToEscrow = Agree;
		ActiveStates["EscrowProperty"] = true;
		EscrowProperty();
	}

	modifier isAcceptInsuranceAuthorized{
		if(addressMapping["Insurer"] == address(0x0)){
			addressMapping["Insurer"] = msg.sender;
		}
		require(msg.sender==addressMapping["Insurer"]);
		_;
	}

	modifier isAcceptInsuranceState{
		require(isStateActive("AcceptInsurance")==true);
		_;
	}

	function AcceptInsurance(string memory Notes, bool IsEnsured) isAcceptInsuranceState isAcceptInsuranceAuthorized public {
		ActiveStates["AcceptInsurance"] = false;
		insurance.additionalInformation = Notes;
		insurance.isEnsured = IsEnsured;
		Gateway_1whiz9yIncoming += 1;
		Gateway_1whiz9yLogic();
	}

	modifier isEscrowMoneyPayableAuthorized{
		if(addressMapping["Lender"] == address(0x0)){
			addressMapping["Lender"] = msg.sender;
		}
		require(msg.sender==addressMapping["Lender"]);
		_;
	}

	modifier isEscrowMoneyPayableState{
		require(isStateActive("EscrowMoneyPayable")==true);
		_;
	}

	function EscrowMoneyPayable(bool Agree) payable isEscrowMoneyPayableState isEscrowMoneyPayableAuthorized public {
		ActiveStates["EscrowMoneyPayable"] = false;
		escrowagreement.agreeToEscrow = Agree;
		Gateway_1whiz9yIncoming += 1;
		Gateway_1whiz9yLogic();
	}

	function Gateway_1wbe662Logic() internal {
		if(contractvalidation.cancelable){
			ActiveStates["ReleaseEscrowsPayable"] = true;
			ReleaseEscrowsPayable();
		}
		else if(!contractvalidation.cancelable){
			ActiveStates["CancelApplication"] = true;
		}
	}

	modifier isEscrowPropertyState{
		require(isStateActive("EscrowProperty")==true);
		_;
	}

	function EscrowProperty() isEscrowPropertyState internal {
		ActiveStates["EscrowProperty"] = false;
		houseTokenAddress.transferFrom(addressMapping["Property Owner"], address(this), mortgage.propertyAddress);
		Gateway_1whiz9yIncoming += 1;
		Gateway_1whiz9yLogic();
	}

	function Gateway_1whiz9yLogic() internal {
		if(Gateway_1whiz9yIncoming==3){
			ActiveStates["ValidateContractPayable"] = true;
			ValidateContractPayable();
			Gateway_1whiz9yIncoming = 0;
		}
	}

	modifier isReleaseEscrowsPayableState{
		require(isStateActive("ReleaseEscrowsPayable")==true);
		_;
	}

	function ReleaseEscrowsPayable() payable isReleaseEscrowsPayableState public {
		ActiveStates["ReleaseEscrowsPayable"] = false;
		
            payable(addressMapping["Lender"]).transfer(mortgage.propertyPrice);
            houseTokenAddress.transferFrom(address(this),addressMapping["Property Owner"], mortgage.propertyAddress);
          
		ActiveStates["Event_1qywpw3"] = true;
	}

	modifier isValidateContractPayableState{
		require(isStateActive("ValidateContractPayable")==true);
		_;
	}

	function ValidateContractPayable() payable isValidateContractPayableState public {
		ActiveStates["ValidateContractPayable"] = false;
		if (payable(address(this)).balance >= mortgage.propertyPrice && insurance.isEnsured) { contractvalidation.valid = true; contractvalidation.cancelable = false; } else { contractvalidation.valid = false; }
		Gateway_1s86rvzLogic();
	}

	function Gateway_1s86rvzLogic() internal {
		if(contractvalidation.valid){
			ActiveStates["PayOwnerPaymentPayable"] = true;
			PayOwnerPaymentPayable();
		}
		else if(!contractvalidation.valid){
			ActiveStates["ReleaseEscrowsPayable"] = true;
			ReleaseEscrowsPayable();
		}
	}

	modifier isPayOwnerPaymentPayableState{
		require(isStateActive("PayOwnerPaymentPayable")==true);
		_;
	}

	function PayOwnerPaymentPayable() payable isPayOwnerPaymentPayableState public {
		ActiveStates["PayOwnerPaymentPayable"] = false;
		payable(addressMapping["Property Owner"]).transfer(mortgage.propertyPrice);
		Gateway_1b1r7rxLogic();
	}

	function Gateway_1b1r7rxLogic() internal {
		Gateway_1egt0pjLogic();
		Gateway_1rnm0w1Logic();
	}

	function Gateway_1egt0pjLogic() internal {
		ActiveStates["PayMortgageFeePayable"] = true;
	}

	function Gateway_1rnm0w1Logic() internal {
		ActiveStates["RequestDefault"] = true;
	}

	modifier isPayMortgageFeePayableAuthorized{
		if(addressMapping["Borrower"] == address(0x0)){
			addressMapping["Borrower"] = msg.sender;
		}
		require(msg.sender==addressMapping["Borrower"]);
		_;
	}

	modifier isPayMortgageFeePayableState{
		require(isStateActive("PayMortgageFeePayable")==true);
		_;
	}

	function PayMortgageFeePayable(uint256 Amount, address Sender, address Receiver) payable isPayMortgageFeePayableState isPayMortgageFeePayableAuthorized public {
		ActiveStates["PayMortgageFeePayable"] = false;
		payment.amount = Amount;
		payment.sender = Sender;
		payment.receiver = Receiver;
		ActiveStates["PaymenttotheInsurerandLenderPayable"] = true;
		PaymenttotheInsurerandLenderPayable();
	}

	modifier isRequestDefaultAuthorized{
		if(addressMapping["Lender"] == address(0x0)){
			addressMapping["Lender"] = msg.sender;
		}
		require(msg.sender==addressMapping["Lender"]);
		_;
	}

	modifier isRequestDefaultState{
		require(isStateActive("RequestDefault")==true);
		_;
	}

	function RequestDefault(string memory Reason) isRequestDefaultState isRequestDefaultAuthorized public {
		ActiveStates["RequestDefault"] = false;
		mortgagedefaultrequest.reason = Reason;
		ActiveStates["ValidateTermsViolation"] = true;
		ValidateTermsViolation();
	}

	modifier isPaymenttotheInsurerandLenderPayableState{
		require(isStateActive("PaymenttotheInsurerandLenderPayable")==true);
		_;
	}

	function PaymenttotheInsurerandLenderPayable() payable isPaymenttotheInsurerandLenderPayableState public {
		ActiveStates["PaymenttotheInsurerandLenderPayable"] = false;
		
            payable(addressMapping["Lender"]).transfer(address(this).balance*95/100);
            payable(addressMapping["Insurer"]).transfer(address(this).balance);
          
		ActiveStates["CheckPaymentSchedule"] = true;
		CheckPaymentSchedule();
	}

	modifier isValidateTermsViolationState{
		require(isStateActive("ValidateTermsViolation")==true);
		_;
	}

	function ValidateTermsViolation() isValidateTermsViolationState internal {
		ActiveStates["ValidateTermsViolation"] = false;
		// TODO
		Gateway_1c0jk30Logic();
	}

	modifier isCheckPaymentScheduleState{
		require(isStateActive("CheckPaymentSchedule")==true);
		_;
	}

	function CheckPaymentSchedule() isCheckPaymentScheduleState internal {
		ActiveStates["CheckPaymentSchedule"] = false;
		
            mortgage.totalPaid += payment.amount;
            mortgage.payments[mortgage.paymentsLength++] = payment;
            payment = Payment({amount: 0, onSchedule: false, sender: address(0x0), receiver: address(0x0)});
            uint256 monthlyFee = (mortgage.propertyPrice - mortgage.downPaymentValue)*(1+mortgage.rate/100)/mortgage.mortgageDurationMonths;
            if (mortgage.totalPaid < monthlyFee*mortgage.paymentsLength) {payment.onSchedule= false; } else {payment.onSchedule = true; }
          
		Gateway_04v6h62Logic();
	}

	function Gateway_1c0jk30Logic() internal {
		if(false){
			Gateway_1rnm0w1Logic();
		}
		else if(true){
			ActiveStates["TransferProportionMoneytotheBorrowerPayable"] = true;
		}
	}

	function Gateway_04v6h62Logic() internal {
		if(mortgage.totalPaid >=(mortgage.propertyPrice - mortgage.downPaymentValue)*(1+mortgage.rate/100)){
			ActiveStates["TransferthePropertytotheBorrower"] = true;
			TransferthePropertytotheBorrower();
		}
		else if(mortgage.totalPaid < (mortgage.propertyPrice - mortgage.downPaymentValue)*(1+mortgage.rate/100)){
			Gateway_1wj3ze2Logic();
		}
	}

	modifier isTransferProportionMoneytotheBorrowerPayableAuthorized{
		if(addressMapping["Lender"] == address(0x0)){
			addressMapping["Lender"] = msg.sender;
		}
		require(msg.sender==addressMapping["Lender"]);
		_;
	}

	modifier isTransferProportionMoneytotheBorrowerPayableState{
		require(isStateActive("TransferProportionMoneytotheBorrowerPayable")==true);
		_;
	}

	function TransferProportionMoneytotheBorrowerPayable(uint256 Amount, address Sender, address Receiver) payable isTransferProportionMoneytotheBorrowerPayableState isTransferProportionMoneytotheBorrowerPayableAuthorized public {
		ActiveStates["TransferProportionMoneytotheBorrowerPayable"] = false;
		payment.amount = Amount;
		payment.sender = Sender;
		payment.receiver = Receiver;
		ActiveStates["TransferthePropertytotheLender"] = true;
		TransferthePropertytotheLender();
	}

	modifier isTransferthePropertytotheBorrowerState{
		require(isStateActive("TransferthePropertytotheBorrower")==true);
		_;
	}

	function TransferthePropertytotheBorrower() isTransferthePropertytotheBorrowerState internal {
		ActiveStates["TransferthePropertytotheBorrower"] = false;
		houseTokenAddress.transferFrom(address(this),addressMapping["Borrower"], mortgage.propertyAddress);
		ActiveStates["Event_1iyrt67"] = true;
	}

	function Gateway_1wj3ze2Logic() internal {
		if(!payment.onSchedule){
			ActiveStates["CheckIndemnityTerms"] = true;
		}
		else if(payment.onSchedule){
			Gateway_1egt0pjLogic();
		}
	}

	modifier isTransferthePropertytotheLenderState{
		require(isStateActive("TransferthePropertytotheLender")==true);
		_;
	}

	function TransferthePropertytotheLender() isTransferthePropertytotheLenderState internal {
		ActiveStates["TransferthePropertytotheLender"] = false;
		houseTokenAddress.transferFrom(address(this),addressMapping["Lender"], mortgage.propertyAddress);
		ActiveStates["Event_1p3tfia"] = true;
	}

	modifier isCheckIndemnityTermsAuthorized{
		if(addressMapping["Insurer"] == address(0x0)){
			addressMapping["Insurer"] = msg.sender;
		}
		require(msg.sender==addressMapping["Insurer"]);
		_;
	}

	modifier isCheckIndemnityTermsState{
		require(isStateActive("CheckIndemnityTerms")==true);
		_;
	}

	function CheckIndemnityTerms(bool Valid) isCheckIndemnityTermsState isCheckIndemnityTermsAuthorized public {
		ActiveStates["CheckIndemnityTerms"] = false;
		indemnity.valid = Valid;
		Gateway_04bakm7Logic();
	}

	function Gateway_04bakm7Logic() internal {
		if(true){
			ActiveStates["PayfortheBorrowerPayable"] = true;
		}
		else if(false){
			Gateway_1egt0pjLogic();
		}
	}

	modifier isPayfortheBorrowerPayableAuthorized{
		if(addressMapping["Insurer"] == address(0x0)){
			addressMapping["Insurer"] = msg.sender;
		}
		require(msg.sender==addressMapping["Insurer"]);
		_;
	}

	modifier isPayfortheBorrowerPayableState{
		require(isStateActive("PayfortheBorrowerPayable")==true);
		_;
	}

	function PayfortheBorrowerPayable(uint256 Amount, address Sender, address Receiver) payable isPayfortheBorrowerPayableState isPayfortheBorrowerPayableAuthorized public {
		ActiveStates["PayfortheBorrowerPayable"] = false;
		payment.amount = Amount;
		payment.sender = Sender;
		payment.receiver = Receiver;
		ActiveStates["ProcessInsurerPaymentPayable"] = true;
		ProcessInsurerPaymentPayable();
	}

	modifier isProcessInsurerPaymentPayableState{
		require(isStateActive("ProcessInsurerPaymentPayable")==true);
		_;
	}

	function ProcessInsurerPaymentPayable() payable isProcessInsurerPaymentPayableState public {
		ActiveStates["ProcessInsurerPaymentPayable"] = false;
		
            mortgage.totalPaid += payment.amount;
            payment = Payment({amount: 0, onSchedule: false, sender: address(0x0), receiver: address(0x0)});

            payable(addressMapping["Lender"]).transfer(address(this).balance*95/100);
            payable(addressMapping["Insurer"]).transfer(address(this).balance);
          
		Gateway_1egt0pjLogic();
	}

 }