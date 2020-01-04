{-# LANGUAGE MultiParamTypeClasses #-}

module LandTitleTransferRecording where

import Playground.Contract
import Language.Plutus.Contract
import Language.PlutusTx.Eq as E
import Ledger
import Ledger.Value                     (Value, geq)
import Ledger.Ada                     as ADA
import Control.Applicative              (pure, (<|>))
import Control.Monad                    (void)
import Control.Monad.Error.Lens
import Prelude
import Language.PlutusTx.List(uniqueElement, find)
import Ledger.Validation(findContinuingOutputs)
import Data.Text(Text)
import Wallet.Emulator
import Data.ByteString.Lazy.UTF8 as BLU
import Wallet.Emulator.Wallet
import Ledger.Typed.Scripts
import Language.PlutusTx
import Language.PlutusTx.Numeric(zero)
import Ledger.AddressMap
import Language.Plutus.Contract.StateMachine
import Control.Monad.Freer
import Language.PlutusTx.AssocMap
import qualified Data.Map as Map
import Data.Map(Map)
import qualified Data.Set as Set
import Data.Maybe
import Ledger.Interval(always)
import Control.Lens(makeClassyPrisms)
--
--
data LandTitleTransferRecording = LandTitleTransferRecording
    { idLandTitleTransferRecording :: ByteString
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, IotsType, ToSchema)

makeLift ''LandTitleTransferRecording


data FactLandTitleTransfer = FactLandTitleTransfer
    { attrlandtitle :: ByteString
    , attrprice :: Integer
    , attrcurrentowner :: PubKey
    , attrnewowner :: PubKey
    }
    deriving (E.Eq, Prelude.Eq, Prelude.Show, Prelude.Ord, Generic, IotsType, ToSchema)

makeLift ''FactLandTitleTransfer
makeIsData ''FactLandTitleTransfer


data FactPayment = FactPayment
    { attramountpaid :: Integer
    , attrpayer :: PubKey
    , attrreceiver :: PubKey
    }
    deriving (E.Eq, Prelude.Eq, Prelude.Show, Prelude.Ord, Generic, IotsType, ToSchema)

makeLift ''FactPayment
makeIsData ''FactPayment


data Roles = Roles
    { initiator :: PubKey
    , executor :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Prelude.Ord, Generic, IotsType, ToSchema)

makeLift ''Roles
makeIsData ''Roles

data TransactionKindLandTitleTransferState =
      InitialLandTitleTransfer   Roles
    | RequestedLandTitleTransfer Roles FactLandTitleTransfer
    | PromisedLandTitleTransfer  Roles FactLandTitleTransfer
    | StatedLandTitleTransfer    Roles FactLandTitleTransfer
    | AcceptedLandTitleTransfer  Roles FactLandTitleTransfer
    | DeclinedLandTitleTransfer  Roles FactLandTitleTransfer
    | QuittedLandTitleTransfer   Roles FactLandTitleTransfer
    | RejectedLandTitleTransfer  Roles FactLandTitleTransfer
    | StoppedLandTitleTransfer   Roles FactLandTitleTransfer
    deriving (Prelude.Eq, Prelude.Ord, Prelude.Show, Generic, IotsType)

makeLift ''TransactionKindLandTitleTransferState
makeIsData ''TransactionKindLandTitleTransferState

instance E.Eq TransactionKindLandTitleTransferState where
    (InitialLandTitleTransfer _)          ==  (InitialLandTitleTransfer _)       =   True
    (RequestedLandTitleTransfer _ _)   ==  (RequestedLandTitleTransfer _ _)   =   True
    (PromisedLandTitleTransfer _ _)    ==  (PromisedLandTitleTransfer _ _)    =   True
    (StatedLandTitleTransfer _ _)      ==  (StatedLandTitleTransfer _ _)      =   True
    (AcceptedLandTitleTransfer _ _)    ==  (AcceptedLandTitleTransfer _ _)    =   True
    (DeclinedLandTitleTransfer _ _)    ==  (DeclinedLandTitleTransfer _ _)    =   True
    (QuittedLandTitleTransfer _ _)     ==  (QuittedLandTitleTransfer _ _)     =   True
    (RejectedLandTitleTransfer _ _)    ==  (RejectedLandTitleTransfer _ _)    =   True
    (StoppedLandTitleTransfer _ _)     ==  (StoppedLandTitleTransfer _ _)     =   True
    _                             ==  _                                =   False

data TransactionKindLandTitleTransferAction =
      InitiateLandTitleTransfer Roles
    | RequestLandTitleTransfer FactLandTitleTransfer
    | PromiseLandTitleTransfer
    | StateLandTitleTransfer
    | AcceptLandTitleTransfer
    | DeclineLandTitleTransfer
    | QuitLandTitleTransfer
    | RejectLandTitleTransfer
    | StopLandTitleTransfer
    deriving (Prelude.Eq, Prelude.Ord, Prelude.Show, Generic, IotsType)

makeLift ''TransactionKindLandTitleTransferAction
makeIsData ''TransactionKindLandTitleTransferAction

data TransactionKindPaymentState =
      InitialPayment   Roles
    | RequestedPayment Roles FactPayment
    | PromisedPayment  Roles FactPayment
    | StatedPayment    Roles FactPayment
    | AcceptedPayment  Roles FactPayment
    | DeclinedPayment  Roles FactPayment
    | QuittedPayment   Roles FactPayment
    | RejectedPayment  Roles FactPayment
    | StoppedPayment   Roles FactPayment
    deriving (Prelude.Eq, Prelude.Ord, Prelude.Show, Generic, IotsType)

makeLift ''TransactionKindPaymentState
makeIsData ''TransactionKindPaymentState

instance E.Eq TransactionKindPaymentState where
    (InitialPayment _)          ==  (InitialPayment _)       =   True
    (RequestedPayment _ _)   ==  (RequestedPayment _ _)   =   True
    (PromisedPayment _ _)    ==  (PromisedPayment _ _)    =   True
    (StatedPayment _ _)      ==  (StatedPayment _ _)      =   True
    (AcceptedPayment _ _)    ==  (AcceptedPayment _ _)    =   True
    (DeclinedPayment _ _)    ==  (DeclinedPayment _ _)    =   True
    (QuittedPayment _ _)     ==  (QuittedPayment _ _)     =   True
    (RejectedPayment _ _)    ==  (RejectedPayment _ _)    =   True
    (StoppedPayment _ _)     ==  (StoppedPayment _ _)     =   True
    _                             ==  _                                =   False

data TransactionKindPaymentAction =
      InitiatePayment Roles
    | RequestPayment FactPayment
    | PromisePayment
    | StatePayment
    | AcceptPayment
    | DeclinePayment
    | QuitPayment
    | RejectPayment
    | StopPayment
    deriving (Prelude.Eq, Prelude.Ord, Prelude.Show, Generic, IotsType)

makeLift ''TransactionKindPaymentAction
makeIsData ''TransactionKindPaymentAction

transitionLandTitleTransferCompletion :: LandTitleTransferRecording -> TransactionKindLandTitleTransferState -> TransactionKindLandTitleTransferAction -> Maybe TransactionKindLandTitleTransferState
transitionLandTitleTransferCompletion _ (InitialLandTitleTransfer ie) (RequestLandTitleTransfer fct)
    = Just (RequestedLandTitleTransfer ie fct)
transitionLandTitleTransferCompletion _ (RequestedLandTitleTransfer ie fct) DeclineLandTitleTransfer
    = Just (DeclinedLandTitleTransfer ie fct)
transitionLandTitleTransferCompletion _ (RequestedLandTitleTransfer ie fct) PromiseLandTitleTransfer
    = Just (PromisedLandTitleTransfer ie fct)
transitionLandTitleTransferCompletion _ (DeclinedLandTitleTransfer ie fct) (RequestLandTitleTransfer fct1)
    = Just (RequestedLandTitleTransfer ie fct1)
transitionLandTitleTransferCompletion _ (DeclinedLandTitleTransfer ie fct) QuitLandTitleTransfer
    = Just (QuittedLandTitleTransfer ie fct)
transitionLandTitleTransferCompletion _ (PromisedLandTitleTransfer ie fct) StateLandTitleTransfer
    = Just (StatedLandTitleTransfer ie fct)
transitionLandTitleTransferCompletion _ (StatedLandTitleTransfer ie fct) RejectLandTitleTransfer
    = Just (RejectedLandTitleTransfer ie fct)
transitionLandTitleTransferCompletion _ (StatedLandTitleTransfer ie fct) AcceptLandTitleTransfer
    = Just (AcceptedLandTitleTransfer ie fct)
transitionLandTitleTransferCompletion _ (RejectedLandTitleTransfer ie fct) StateLandTitleTransfer
    = Just (StatedLandTitleTransfer ie fct)
transitionLandTitleTransferCompletion _ (RejectedLandTitleTransfer ie fct) StopLandTitleTransfer
    = Just (StoppedLandTitleTransfer ie fct)
transitionLandTitleTransferCompletion _ _ _
    = Nothing

checkLandTitleTransferCompletion :: LandTitleTransferRecording -> TransactionKindLandTitleTransferState -> TransactionKindLandTitleTransferAction -> PendingTx -> Bool
checkLandTitleTransferCompletion _ _ _ _ = True
checkLandTitleTransferCompletion _ (InitialLandTitleTransfer roles) (RequestLandTitleTransfer _) penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferCompletion _ (DeclinedLandTitleTransfer roles _) (RequestLandTitleTransfer _) penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferCompletion _ (DeclinedLandTitleTransfer roles _) QuitLandTitleTransfer penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferCompletion _ (StatedLandTitleTransfer roles _) AcceptLandTitleTransfer penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferCompletion _ (StatedLandTitleTransfer roles _) RejectLandTitleTransfer penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferCompletion _ (RequestedLandTitleTransfer roles _) PromiseLandTitleTransfer penTx = txSignedBy penTx (executor roles)
checkLandTitleTransferCompletion _ (RequestedLandTitleTransfer roles _) DeclineLandTitleTransfer penTx = txSignedBy penTx (executor roles)
checkLandTitleTransferCompletion _ (PromisedLandTitleTransfer roles _) StateLandTitleTransfer penTx = txSignedBy penTx (executor roles)
checkLandTitleTransferCompletion _ (RejectedLandTitleTransfer roles _) StateLandTitleTransfer penTx = txSignedBy penTx (executor roles)
checkLandTitleTransferCompletion _ (RejectedLandTitleTransfer roles _) StopLandTitleTransfer penTx = txSignedBy penTx (executor roles)


isFinalLandTitleTransferCompletion :: TransactionKindLandTitleTransferState -> Bool
isFinalLandTitleTransferCompletion (QuittedLandTitleTransfer _ _)    =   True
isFinalLandTitleTransferCompletion (StoppedLandTitleTransfer _ _)    =   True
isFinalLandTitleTransferCompletion (AcceptedLandTitleTransfer _ _)   =   True
isFinalLandTitleTransferCompletion _                            =   False


machineLandTitleTransferCompletion :: LandTitleTransferRecording -> StateMachine TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction
machineLandTitleTransferCompletion pn = StateMachine
    { smTransition  =   transitionLandTitleTransferCompletion pn
    , smCheck       =   checkLandTitleTransferCompletion pn
    , smFinal       =   isFinalLandTitleTransferCompletion
    }

validatorLandTitleTransferCompletion :: LandTitleTransferRecording -> ValidatorType (StateMachine TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction)
validatorLandTitleTransferCompletion pn = mkValidator (machineLandTitleTransferCompletion pn)

scriptLandTitleTransferCompletion :: LandTitleTransferRecording -> ScriptInstance (StateMachine TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction)
scriptLandTitleTransferCompletion pn = 
    let val = $$(compile [|| validatorLandTitleTransferCompletion ||])
            `applyCode`
                liftCode pn
        wrap = wrapValidator @TransactionKindLandTitleTransferState @TransactionKindLandTitleTransferAction
    in validator @(StateMachine TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction)
        val $$(compile [|| wrap ||])

instanceLandTitleTransferCompletion :: LandTitleTransferRecording -> StateMachineInstance TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction
instanceLandTitleTransferCompletion pn = StateMachineInstance
    { stateMachine = machineLandTitleTransferCompletion pn
    , validatorInstance = scriptLandTitleTransferCompletion pn
    }

allocateLandTitleTransferCompletion :: TransactionKindLandTitleTransferState -> TransactionKindLandTitleTransferAction -> Value -> ValueAllocation
allocateLandTitleTransferCompletion _ _ currentValue =
    ValueAllocation
        { vaOwnAddress = currentValue
        , vaOtherPayments = mempty
        }

clientLandTitleTransferCompletion :: LandTitleTransferRecording -> StateMachineClient TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction
clientLandTitleTransferCompletion pn = mkStateMachineClient (instanceLandTitleTransferCompletion pn) allocateLandTitleTransferCompletion

transitionLandTitleTransferSending :: LandTitleTransferRecording -> TransactionKindLandTitleTransferState -> TransactionKindLandTitleTransferAction -> Maybe TransactionKindLandTitleTransferState
transitionLandTitleTransferSending _ (InitialLandTitleTransfer ie) (RequestLandTitleTransfer fct)
    = Just (RequestedLandTitleTransfer ie fct)
transitionLandTitleTransferSending _ (RequestedLandTitleTransfer ie fct) DeclineLandTitleTransfer
    = Just (DeclinedLandTitleTransfer ie fct)
transitionLandTitleTransferSending _ (RequestedLandTitleTransfer ie fct) PromiseLandTitleTransfer
    = Just (PromisedLandTitleTransfer ie fct)
transitionLandTitleTransferSending _ (DeclinedLandTitleTransfer ie fct) (RequestLandTitleTransfer fct1)
    = Just (RequestedLandTitleTransfer ie fct1)
transitionLandTitleTransferSending _ (DeclinedLandTitleTransfer ie fct) QuitLandTitleTransfer
    = Just (QuittedLandTitleTransfer ie fct)
transitionLandTitleTransferSending _ (PromisedLandTitleTransfer ie fct) StateLandTitleTransfer
    = Just (StatedLandTitleTransfer ie fct)
transitionLandTitleTransferSending _ (StatedLandTitleTransfer ie fct) RejectLandTitleTransfer
    = Just (RejectedLandTitleTransfer ie fct)
transitionLandTitleTransferSending _ (StatedLandTitleTransfer ie fct) AcceptLandTitleTransfer
    = Just (AcceptedLandTitleTransfer ie fct)
transitionLandTitleTransferSending _ (RejectedLandTitleTransfer ie fct) StateLandTitleTransfer
    = Just (StatedLandTitleTransfer ie fct)
transitionLandTitleTransferSending _ (RejectedLandTitleTransfer ie fct) StopLandTitleTransfer
    = Just (StoppedLandTitleTransfer ie fct)
transitionLandTitleTransferSending _ _ _
    = Nothing

checkLandTitleTransferSending :: LandTitleTransferRecording -> TransactionKindLandTitleTransferState -> TransactionKindLandTitleTransferAction -> PendingTx -> Bool
checkLandTitleTransferSending _ _ _ _ = True
checkLandTitleTransferSending _ (InitialLandTitleTransfer roles) (RequestLandTitleTransfer _) penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferSending _ (DeclinedLandTitleTransfer roles _) (RequestLandTitleTransfer _) penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferSending _ (DeclinedLandTitleTransfer roles _) QuitLandTitleTransfer penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferSending _ (StatedLandTitleTransfer roles _) AcceptLandTitleTransfer penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferSending _ (StatedLandTitleTransfer roles _) RejectLandTitleTransfer penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferSending _ (RequestedLandTitleTransfer roles _) PromiseLandTitleTransfer penTx = txSignedBy penTx (executor roles)
checkLandTitleTransferSending _ (RequestedLandTitleTransfer roles _) DeclineLandTitleTransfer penTx = txSignedBy penTx (executor roles)
checkLandTitleTransferSending _ (PromisedLandTitleTransfer roles _) StateLandTitleTransfer penTx = txSignedBy penTx (executor roles)
checkLandTitleTransferSending _ (RejectedLandTitleTransfer roles _) StateLandTitleTransfer penTx = txSignedBy penTx (executor roles)
checkLandTitleTransferSending _ (RejectedLandTitleTransfer roles _) StopLandTitleTransfer penTx = txSignedBy penTx (executor roles)


isFinalLandTitleTransferSending :: TransactionKindLandTitleTransferState -> Bool
isFinalLandTitleTransferSending (QuittedLandTitleTransfer _ _)    =   True
isFinalLandTitleTransferSending (StoppedLandTitleTransfer _ _)    =   True
isFinalLandTitleTransferSending (AcceptedLandTitleTransfer _ _)   =   True
isFinalLandTitleTransferSending _                            =   False


machineLandTitleTransferSending :: LandTitleTransferRecording -> StateMachine TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction
machineLandTitleTransferSending pn = StateMachine
    { smTransition  =   transitionLandTitleTransferSending pn
    , smCheck       =   checkLandTitleTransferSending pn
    , smFinal       =   isFinalLandTitleTransferSending
    }

validatorLandTitleTransferSending :: LandTitleTransferRecording -> ValidatorType (StateMachine TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction)
validatorLandTitleTransferSending pn = mkValidator (machineLandTitleTransferSending pn)

scriptLandTitleTransferSending :: LandTitleTransferRecording -> ScriptInstance (StateMachine TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction)
scriptLandTitleTransferSending pn = 
    let val = $$(compile [|| validatorLandTitleTransferSending ||])
            `applyCode`
                liftCode pn
        wrap = wrapValidator @TransactionKindLandTitleTransferState @TransactionKindLandTitleTransferAction
    in validator @(StateMachine TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction)
        val $$(compile [|| wrap ||])

instanceLandTitleTransferSending :: LandTitleTransferRecording -> StateMachineInstance TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction
instanceLandTitleTransferSending pn = StateMachineInstance
    { stateMachine = machineLandTitleTransferSending pn
    , validatorInstance = scriptLandTitleTransferSending pn
    }

allocateLandTitleTransferSending :: TransactionKindLandTitleTransferState -> TransactionKindLandTitleTransferAction -> Value -> ValueAllocation
allocateLandTitleTransferSending _ _ currentValue =
    ValueAllocation
        { vaOwnAddress = currentValue
        , vaOtherPayments = mempty
        }

clientLandTitleTransferSending :: LandTitleTransferRecording -> StateMachineClient TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction
clientLandTitleTransferSending pn = mkStateMachineClient (instanceLandTitleTransferSending pn) allocateLandTitleTransferSending

transitionPaymentSending :: LandTitleTransferRecording -> TransactionKindPaymentState -> TransactionKindPaymentAction -> Maybe TransactionKindPaymentState
transitionPaymentSending _ (InitialPayment ie) (RequestPayment fct)
    = Just (RequestedPayment ie fct)
transitionPaymentSending _ (RequestedPayment ie fct) DeclinePayment
    = Just (DeclinedPayment ie fct)
transitionPaymentSending _ (RequestedPayment ie fct) PromisePayment
    = Just (PromisedPayment ie fct)
transitionPaymentSending _ (DeclinedPayment ie fct) (RequestPayment fct1)
    = Just (RequestedPayment ie fct1)
transitionPaymentSending _ (DeclinedPayment ie fct) QuitPayment
    = Just (QuittedPayment ie fct)
transitionPaymentSending _ (PromisedPayment ie fct) StatePayment
    = Just (StatedPayment ie fct)
transitionPaymentSending _ (StatedPayment ie fct) RejectPayment
    = Just (RejectedPayment ie fct)
transitionPaymentSending _ (StatedPayment ie fct) AcceptPayment
    = Just (AcceptedPayment ie fct)
transitionPaymentSending _ (RejectedPayment ie fct) StatePayment
    = Just (StatedPayment ie fct)
transitionPaymentSending _ (RejectedPayment ie fct) StopPayment
    = Just (StoppedPayment ie fct)
transitionPaymentSending _ _ _
    = Nothing

checkPaymentSending :: LandTitleTransferRecording -> TransactionKindPaymentState -> TransactionKindPaymentAction -> PendingTx -> Bool
checkPaymentSending _ _ _ _ = True
checkPaymentSending _ (InitialPayment roles) (RequestPayment _) penTx = txSignedBy penTx (initiator roles)
checkPaymentSending _ (DeclinedPayment roles _) (RequestPayment _) penTx = txSignedBy penTx (initiator roles)
checkPaymentSending _ (DeclinedPayment roles _) QuitPayment penTx = txSignedBy penTx (initiator roles)
checkPaymentSending _ (StatedPayment roles _) AcceptPayment penTx = txSignedBy penTx (initiator roles)
checkPaymentSending _ (StatedPayment roles _) RejectPayment penTx = txSignedBy penTx (initiator roles)
checkPaymentSending _ (RequestedPayment roles _) PromisePayment penTx = txSignedBy penTx (executor roles)
checkPaymentSending _ (RequestedPayment roles _) DeclinePayment penTx = txSignedBy penTx (executor roles)
checkPaymentSending _ (PromisedPayment roles _) StatePayment penTx = txSignedBy penTx (executor roles)
checkPaymentSending _ (RejectedPayment roles _) StatePayment penTx = txSignedBy penTx (executor roles)
checkPaymentSending _ (RejectedPayment roles _) StopPayment penTx = txSignedBy penTx (executor roles)


isFinalPaymentSending :: TransactionKindPaymentState -> Bool
isFinalPaymentSending (QuittedPayment _ _)    =   True
isFinalPaymentSending (StoppedPayment _ _)    =   True
isFinalPaymentSending (AcceptedPayment _ _)   =   True
isFinalPaymentSending _                            =   False


machinePaymentSending :: LandTitleTransferRecording -> StateMachine TransactionKindPaymentState TransactionKindPaymentAction
machinePaymentSending pn = StateMachine
    { smTransition  =   transitionPaymentSending pn
    , smCheck       =   checkPaymentSending pn
    , smFinal       =   isFinalPaymentSending
    }

validatorPaymentSending :: LandTitleTransferRecording -> ValidatorType (StateMachine TransactionKindPaymentState TransactionKindPaymentAction)
validatorPaymentSending pn = mkValidator (machinePaymentSending pn)

scriptPaymentSending :: LandTitleTransferRecording -> ScriptInstance (StateMachine TransactionKindPaymentState TransactionKindPaymentAction)
scriptPaymentSending pn = 
    let val = $$(compile [|| validatorPaymentSending ||])
            `applyCode`
                liftCode pn
        wrap = wrapValidator @TransactionKindPaymentState @TransactionKindPaymentAction
    in validator @(StateMachine TransactionKindPaymentState TransactionKindPaymentAction)
        val $$(compile [|| wrap ||])

instancePaymentSending :: LandTitleTransferRecording -> StateMachineInstance TransactionKindPaymentState TransactionKindPaymentAction
instancePaymentSending pn = StateMachineInstance
    { stateMachine = machinePaymentSending pn
    , validatorInstance = scriptPaymentSending pn
    }

allocatePaymentSending :: TransactionKindPaymentState -> TransactionKindPaymentAction -> Value -> ValueAllocation
allocatePaymentSending _ _ currentValue =
    ValueAllocation
        { vaOwnAddress = currentValue
        , vaOtherPayments = mempty
        }

clientPaymentSending :: LandTitleTransferRecording -> StateMachineClient TransactionKindPaymentState TransactionKindPaymentAction
clientPaymentSending pn = mkStateMachineClient (instancePaymentSending pn) allocatePaymentSending

transitionPaymentCollection :: LandTitleTransferRecording -> TransactionKindPaymentState -> TransactionKindPaymentAction -> Maybe TransactionKindPaymentState
transitionPaymentCollection _ (InitialPayment ie) (RequestPayment fct)
    = Just (RequestedPayment ie fct)
transitionPaymentCollection _ (RequestedPayment ie fct) DeclinePayment
    = Just (DeclinedPayment ie fct)
transitionPaymentCollection _ (RequestedPayment ie fct) PromisePayment
    = Just (PromisedPayment ie fct)
transitionPaymentCollection _ (DeclinedPayment ie fct) (RequestPayment fct1)
    = Just (RequestedPayment ie fct1)
transitionPaymentCollection _ (DeclinedPayment ie fct) QuitPayment
    = Just (QuittedPayment ie fct)
transitionPaymentCollection _ (PromisedPayment ie fct) StatePayment
    = Just (StatedPayment ie fct)
transitionPaymentCollection _ (StatedPayment ie fct) RejectPayment
    = Just (RejectedPayment ie fct)
transitionPaymentCollection _ (StatedPayment ie fct) AcceptPayment
    = Just (AcceptedPayment ie fct)
transitionPaymentCollection _ (RejectedPayment ie fct) StatePayment
    = Just (StatedPayment ie fct)
transitionPaymentCollection _ (RejectedPayment ie fct) StopPayment
    = Just (StoppedPayment ie fct)
transitionPaymentCollection _ _ _
    = Nothing

checkPaymentCollection :: LandTitleTransferRecording -> TransactionKindPaymentState -> TransactionKindPaymentAction -> PendingTx -> Bool
checkPaymentCollection _ _ _ _ = True
checkPaymentCollection _ (InitialPayment roles) (RequestPayment _) penTx = txSignedBy penTx (initiator roles)
checkPaymentCollection _ (DeclinedPayment roles _) (RequestPayment _) penTx = txSignedBy penTx (initiator roles)
checkPaymentCollection _ (DeclinedPayment roles _) QuitPayment penTx = txSignedBy penTx (initiator roles)
checkPaymentCollection _ (StatedPayment roles _) AcceptPayment penTx = txSignedBy penTx (initiator roles)
checkPaymentCollection _ (StatedPayment roles _) RejectPayment penTx = txSignedBy penTx (initiator roles)
checkPaymentCollection _ (RequestedPayment roles _) PromisePayment penTx = txSignedBy penTx (executor roles)
checkPaymentCollection _ (RequestedPayment roles _) DeclinePayment penTx = txSignedBy penTx (executor roles)
checkPaymentCollection _ (PromisedPayment roles _) StatePayment penTx = txSignedBy penTx (executor roles)
checkPaymentCollection _ (RejectedPayment roles _) StatePayment penTx = txSignedBy penTx (executor roles)
checkPaymentCollection _ (RejectedPayment roles _) StopPayment penTx = txSignedBy penTx (executor roles)


isFinalPaymentCollection :: TransactionKindPaymentState -> Bool
isFinalPaymentCollection (QuittedPayment _ _)    =   True
isFinalPaymentCollection (StoppedPayment _ _)    =   True
isFinalPaymentCollection (AcceptedPayment _ _)   =   True
isFinalPaymentCollection _                            =   False


machinePaymentCollection :: LandTitleTransferRecording -> StateMachine TransactionKindPaymentState TransactionKindPaymentAction
machinePaymentCollection pn = StateMachine
    { smTransition  =   transitionPaymentCollection pn
    , smCheck       =   checkPaymentCollection pn
    , smFinal       =   isFinalPaymentCollection
    }

validatorPaymentCollection :: LandTitleTransferRecording -> ValidatorType (StateMachine TransactionKindPaymentState TransactionKindPaymentAction)
validatorPaymentCollection pn = mkValidator (machinePaymentCollection pn)

scriptPaymentCollection :: LandTitleTransferRecording -> ScriptInstance (StateMachine TransactionKindPaymentState TransactionKindPaymentAction)
scriptPaymentCollection pn = 
    let val = $$(compile [|| validatorPaymentCollection ||])
            `applyCode`
                liftCode pn
        wrap = wrapValidator @TransactionKindPaymentState @TransactionKindPaymentAction
    in validator @(StateMachine TransactionKindPaymentState TransactionKindPaymentAction)
        val $$(compile [|| wrap ||])

instancePaymentCollection :: LandTitleTransferRecording -> StateMachineInstance TransactionKindPaymentState TransactionKindPaymentAction
instancePaymentCollection pn = StateMachineInstance
    { stateMachine = machinePaymentCollection pn
    , validatorInstance = scriptPaymentCollection pn
    }

allocatePaymentCollection :: TransactionKindPaymentState -> TransactionKindPaymentAction -> Value -> ValueAllocation
allocatePaymentCollection _ _ currentValue =
    ValueAllocation
        { vaOwnAddress = currentValue
        , vaOtherPayments = mempty
        }

clientPaymentCollection :: LandTitleTransferRecording -> StateMachineClient TransactionKindPaymentState TransactionKindPaymentAction
clientPaymentCollection pn = mkStateMachineClient (instancePaymentCollection pn) allocatePaymentCollection

transitionLandTitleTransferCollection :: LandTitleTransferRecording -> TransactionKindLandTitleTransferState -> TransactionKindLandTitleTransferAction -> Maybe TransactionKindLandTitleTransferState
transitionLandTitleTransferCollection _ (InitialLandTitleTransfer ie) (RequestLandTitleTransfer fct)
    = Just (RequestedLandTitleTransfer ie fct)
transitionLandTitleTransferCollection _ (RequestedLandTitleTransfer ie fct) DeclineLandTitleTransfer
    = Just (DeclinedLandTitleTransfer ie fct)
transitionLandTitleTransferCollection _ (RequestedLandTitleTransfer ie fct) PromiseLandTitleTransfer
    = Just (PromisedLandTitleTransfer ie fct)
transitionLandTitleTransferCollection _ (DeclinedLandTitleTransfer ie fct) (RequestLandTitleTransfer fct1)
    = Just (RequestedLandTitleTransfer ie fct1)
transitionLandTitleTransferCollection _ (DeclinedLandTitleTransfer ie fct) QuitLandTitleTransfer
    = Just (QuittedLandTitleTransfer ie fct)
transitionLandTitleTransferCollection _ (PromisedLandTitleTransfer ie fct) StateLandTitleTransfer
    = Just (StatedLandTitleTransfer ie fct)
transitionLandTitleTransferCollection _ (StatedLandTitleTransfer ie fct) RejectLandTitleTransfer
    = Just (RejectedLandTitleTransfer ie fct)
transitionLandTitleTransferCollection _ (StatedLandTitleTransfer ie fct) AcceptLandTitleTransfer
    = Just (AcceptedLandTitleTransfer ie fct)
transitionLandTitleTransferCollection _ (RejectedLandTitleTransfer ie fct) StateLandTitleTransfer
    = Just (StatedLandTitleTransfer ie fct)
transitionLandTitleTransferCollection _ (RejectedLandTitleTransfer ie fct) StopLandTitleTransfer
    = Just (StoppedLandTitleTransfer ie fct)
transitionLandTitleTransferCollection _ _ _
    = Nothing

checkLandTitleTransferCollection :: LandTitleTransferRecording -> TransactionKindLandTitleTransferState -> TransactionKindLandTitleTransferAction -> PendingTx -> Bool
checkLandTitleTransferCollection _ _ _ _ = True
checkLandTitleTransferCollection _ (InitialLandTitleTransfer roles) (RequestLandTitleTransfer _) penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferCollection _ (DeclinedLandTitleTransfer roles _) (RequestLandTitleTransfer _) penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferCollection _ (DeclinedLandTitleTransfer roles _) QuitLandTitleTransfer penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferCollection _ (StatedLandTitleTransfer roles _) AcceptLandTitleTransfer penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferCollection _ (StatedLandTitleTransfer roles _) RejectLandTitleTransfer penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferCollection _ (RequestedLandTitleTransfer roles _) PromiseLandTitleTransfer penTx = txSignedBy penTx (executor roles)
checkLandTitleTransferCollection _ (RequestedLandTitleTransfer roles _) DeclineLandTitleTransfer penTx = txSignedBy penTx (executor roles)
checkLandTitleTransferCollection _ (PromisedLandTitleTransfer roles _) StateLandTitleTransfer penTx = txSignedBy penTx (executor roles)
checkLandTitleTransferCollection _ (RejectedLandTitleTransfer roles _) StateLandTitleTransfer penTx = txSignedBy penTx (executor roles)
checkLandTitleTransferCollection _ (RejectedLandTitleTransfer roles _) StopLandTitleTransfer penTx = txSignedBy penTx (executor roles)


isFinalLandTitleTransferCollection :: TransactionKindLandTitleTransferState -> Bool
isFinalLandTitleTransferCollection (QuittedLandTitleTransfer _ _)    =   True
isFinalLandTitleTransferCollection (StoppedLandTitleTransfer _ _)    =   True
isFinalLandTitleTransferCollection (AcceptedLandTitleTransfer _ _)   =   True
isFinalLandTitleTransferCollection _                            =   False


machineLandTitleTransferCollection :: LandTitleTransferRecording -> StateMachine TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction
machineLandTitleTransferCollection pn = StateMachine
    { smTransition  =   transitionLandTitleTransferCollection pn
    , smCheck       =   checkLandTitleTransferCollection pn
    , smFinal       =   isFinalLandTitleTransferCollection
    }

validatorLandTitleTransferCollection :: LandTitleTransferRecording -> ValidatorType (StateMachine TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction)
validatorLandTitleTransferCollection pn = mkValidator (machineLandTitleTransferCollection pn)

scriptLandTitleTransferCollection :: LandTitleTransferRecording -> ScriptInstance (StateMachine TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction)
scriptLandTitleTransferCollection pn = 
    let val = $$(compile [|| validatorLandTitleTransferCollection ||])
            `applyCode`
                liftCode pn
        wrap = wrapValidator @TransactionKindLandTitleTransferState @TransactionKindLandTitleTransferAction
    in validator @(StateMachine TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction)
        val $$(compile [|| wrap ||])

instanceLandTitleTransferCollection :: LandTitleTransferRecording -> StateMachineInstance TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction
instanceLandTitleTransferCollection pn = StateMachineInstance
    { stateMachine = machineLandTitleTransferCollection pn
    , validatorInstance = scriptLandTitleTransferCollection pn
    }

allocateLandTitleTransferCollection :: TransactionKindLandTitleTransferState -> TransactionKindLandTitleTransferAction -> Value -> ValueAllocation
allocateLandTitleTransferCollection _ _ currentValue =
    ValueAllocation
        { vaOwnAddress = currentValue
        , vaOtherPayments = mempty
        }

clientLandTitleTransferCollection :: LandTitleTransferRecording -> StateMachineClient TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction
clientLandTitleTransferCollection pn = mkStateMachineClient (instanceLandTitleTransferCollection pn) allocateLandTitleTransferCollection

transitionLandTitleTransferReturning :: LandTitleTransferRecording -> TransactionKindLandTitleTransferState -> TransactionKindLandTitleTransferAction -> Maybe TransactionKindLandTitleTransferState
transitionLandTitleTransferReturning _ (InitialLandTitleTransfer ie) (RequestLandTitleTransfer fct)
    = Just (RequestedLandTitleTransfer ie fct)
transitionLandTitleTransferReturning _ (RequestedLandTitleTransfer ie fct) DeclineLandTitleTransfer
    = Just (DeclinedLandTitleTransfer ie fct)
transitionLandTitleTransferReturning _ (RequestedLandTitleTransfer ie fct) PromiseLandTitleTransfer
    = Just (PromisedLandTitleTransfer ie fct)
transitionLandTitleTransferReturning _ (DeclinedLandTitleTransfer ie fct) (RequestLandTitleTransfer fct1)
    = Just (RequestedLandTitleTransfer ie fct1)
transitionLandTitleTransferReturning _ (DeclinedLandTitleTransfer ie fct) QuitLandTitleTransfer
    = Just (QuittedLandTitleTransfer ie fct)
transitionLandTitleTransferReturning _ (PromisedLandTitleTransfer ie fct) StateLandTitleTransfer
    = Just (StatedLandTitleTransfer ie fct)
transitionLandTitleTransferReturning _ (StatedLandTitleTransfer ie fct) RejectLandTitleTransfer
    = Just (RejectedLandTitleTransfer ie fct)
transitionLandTitleTransferReturning _ (StatedLandTitleTransfer ie fct) AcceptLandTitleTransfer
    = Just (AcceptedLandTitleTransfer ie fct)
transitionLandTitleTransferReturning _ (RejectedLandTitleTransfer ie fct) StateLandTitleTransfer
    = Just (StatedLandTitleTransfer ie fct)
transitionLandTitleTransferReturning _ (RejectedLandTitleTransfer ie fct) StopLandTitleTransfer
    = Just (StoppedLandTitleTransfer ie fct)
transitionLandTitleTransferReturning _ _ _
    = Nothing

checkLandTitleTransferReturning :: LandTitleTransferRecording -> TransactionKindLandTitleTransferState -> TransactionKindLandTitleTransferAction -> PendingTx -> Bool
checkLandTitleTransferReturning _ _ _ _ = True
checkLandTitleTransferReturning _ (InitialLandTitleTransfer roles) (RequestLandTitleTransfer _) penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferReturning _ (DeclinedLandTitleTransfer roles _) (RequestLandTitleTransfer _) penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferReturning _ (DeclinedLandTitleTransfer roles _) QuitLandTitleTransfer penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferReturning _ (StatedLandTitleTransfer roles _) AcceptLandTitleTransfer penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferReturning _ (StatedLandTitleTransfer roles _) RejectLandTitleTransfer penTx = txSignedBy penTx (initiator roles)
checkLandTitleTransferReturning _ (RequestedLandTitleTransfer roles _) PromiseLandTitleTransfer penTx = txSignedBy penTx (executor roles)
checkLandTitleTransferReturning _ (RequestedLandTitleTransfer roles _) DeclineLandTitleTransfer penTx = txSignedBy penTx (executor roles)
checkLandTitleTransferReturning _ (PromisedLandTitleTransfer roles _) StateLandTitleTransfer penTx = txSignedBy penTx (executor roles)
checkLandTitleTransferReturning _ (RejectedLandTitleTransfer roles _) StateLandTitleTransfer penTx = txSignedBy penTx (executor roles)
checkLandTitleTransferReturning _ (RejectedLandTitleTransfer roles _) StopLandTitleTransfer penTx = txSignedBy penTx (executor roles)


isFinalLandTitleTransferReturning :: TransactionKindLandTitleTransferState -> Bool
isFinalLandTitleTransferReturning (QuittedLandTitleTransfer _ _)    =   True
isFinalLandTitleTransferReturning (StoppedLandTitleTransfer _ _)    =   True
isFinalLandTitleTransferReturning (AcceptedLandTitleTransfer _ _)   =   True
isFinalLandTitleTransferReturning _                            =   False


machineLandTitleTransferReturning :: LandTitleTransferRecording -> StateMachine TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction
machineLandTitleTransferReturning pn = StateMachine
    { smTransition  =   transitionLandTitleTransferReturning pn
    , smCheck       =   checkLandTitleTransferReturning pn
    , smFinal       =   isFinalLandTitleTransferReturning
    }

validatorLandTitleTransferReturning :: LandTitleTransferRecording -> ValidatorType (StateMachine TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction)
validatorLandTitleTransferReturning pn = mkValidator (machineLandTitleTransferReturning pn)

scriptLandTitleTransferReturning :: LandTitleTransferRecording -> ScriptInstance (StateMachine TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction)
scriptLandTitleTransferReturning pn = 
    let val = $$(compile [|| validatorLandTitleTransferReturning ||])
            `applyCode`
                liftCode pn
        wrap = wrapValidator @TransactionKindLandTitleTransferState @TransactionKindLandTitleTransferAction
    in validator @(StateMachine TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction)
        val $$(compile [|| wrap ||])

instanceLandTitleTransferReturning :: LandTitleTransferRecording -> StateMachineInstance TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction
instanceLandTitleTransferReturning pn = StateMachineInstance
    { stateMachine = machineLandTitleTransferReturning pn
    , validatorInstance = scriptLandTitleTransferReturning pn
    }

allocateLandTitleTransferReturning :: TransactionKindLandTitleTransferState -> TransactionKindLandTitleTransferAction -> Value -> ValueAllocation
allocateLandTitleTransferReturning _ _ currentValue =
    ValueAllocation
        { vaOwnAddress = currentValue
        , vaOtherPayments = mempty
        }

clientLandTitleTransferReturning :: LandTitleTransferRecording -> StateMachineClient TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction
clientLandTitleTransferReturning pn = mkStateMachineClient (instanceLandTitleTransferReturning pn) allocateLandTitleTransferReturning

transitionPaymentReturning :: LandTitleTransferRecording -> TransactionKindPaymentState -> TransactionKindPaymentAction -> Maybe TransactionKindPaymentState
transitionPaymentReturning _ (InitialPayment ie) (RequestPayment fct)
    = Just (RequestedPayment ie fct)
transitionPaymentReturning _ (RequestedPayment ie fct) DeclinePayment
    = Just (DeclinedPayment ie fct)
transitionPaymentReturning _ (RequestedPayment ie fct) PromisePayment
    = Just (PromisedPayment ie fct)
transitionPaymentReturning _ (DeclinedPayment ie fct) (RequestPayment fct1)
    = Just (RequestedPayment ie fct1)
transitionPaymentReturning _ (DeclinedPayment ie fct) QuitPayment
    = Just (QuittedPayment ie fct)
transitionPaymentReturning _ (PromisedPayment ie fct) StatePayment
    = Just (StatedPayment ie fct)
transitionPaymentReturning _ (StatedPayment ie fct) RejectPayment
    = Just (RejectedPayment ie fct)
transitionPaymentReturning _ (StatedPayment ie fct) AcceptPayment
    = Just (AcceptedPayment ie fct)
transitionPaymentReturning _ (RejectedPayment ie fct) StatePayment
    = Just (StatedPayment ie fct)
transitionPaymentReturning _ (RejectedPayment ie fct) StopPayment
    = Just (StoppedPayment ie fct)
transitionPaymentReturning _ _ _
    = Nothing

checkPaymentReturning :: LandTitleTransferRecording -> TransactionKindPaymentState -> TransactionKindPaymentAction -> PendingTx -> Bool
checkPaymentReturning _ _ _ _ = True
checkPaymentReturning _ (InitialPayment roles) (RequestPayment _) penTx = txSignedBy penTx (initiator roles)
checkPaymentReturning _ (DeclinedPayment roles _) (RequestPayment _) penTx = txSignedBy penTx (initiator roles)
checkPaymentReturning _ (DeclinedPayment roles _) QuitPayment penTx = txSignedBy penTx (initiator roles)
checkPaymentReturning _ (StatedPayment roles _) AcceptPayment penTx = txSignedBy penTx (initiator roles)
checkPaymentReturning _ (StatedPayment roles _) RejectPayment penTx = txSignedBy penTx (initiator roles)
checkPaymentReturning _ (RequestedPayment roles _) PromisePayment penTx = txSignedBy penTx (executor roles)
checkPaymentReturning _ (RequestedPayment roles _) DeclinePayment penTx = txSignedBy penTx (executor roles)
checkPaymentReturning _ (PromisedPayment roles _) StatePayment penTx = txSignedBy penTx (executor roles)
checkPaymentReturning _ (RejectedPayment roles _) StatePayment penTx = txSignedBy penTx (executor roles)
checkPaymentReturning _ (RejectedPayment roles _) StopPayment penTx = txSignedBy penTx (executor roles)


isFinalPaymentReturning :: TransactionKindPaymentState -> Bool
isFinalPaymentReturning (QuittedPayment _ _)    =   True
isFinalPaymentReturning (StoppedPayment _ _)    =   True
isFinalPaymentReturning (AcceptedPayment _ _)   =   True
isFinalPaymentReturning _                            =   False


machinePaymentReturning :: LandTitleTransferRecording -> StateMachine TransactionKindPaymentState TransactionKindPaymentAction
machinePaymentReturning pn = StateMachine
    { smTransition  =   transitionPaymentReturning pn
    , smCheck       =   checkPaymentReturning pn
    , smFinal       =   isFinalPaymentReturning
    }

validatorPaymentReturning :: LandTitleTransferRecording -> ValidatorType (StateMachine TransactionKindPaymentState TransactionKindPaymentAction)
validatorPaymentReturning pn = mkValidator (machinePaymentReturning pn)

scriptPaymentReturning :: LandTitleTransferRecording -> ScriptInstance (StateMachine TransactionKindPaymentState TransactionKindPaymentAction)
scriptPaymentReturning pn = 
    let val = $$(compile [|| validatorPaymentReturning ||])
            `applyCode`
                liftCode pn
        wrap = wrapValidator @TransactionKindPaymentState @TransactionKindPaymentAction
    in validator @(StateMachine TransactionKindPaymentState TransactionKindPaymentAction)
        val $$(compile [|| wrap ||])

instancePaymentReturning :: LandTitleTransferRecording -> StateMachineInstance TransactionKindPaymentState TransactionKindPaymentAction
instancePaymentReturning pn = StateMachineInstance
    { stateMachine = machinePaymentReturning pn
    , validatorInstance = scriptPaymentReturning pn
    }

allocatePaymentReturning :: TransactionKindPaymentState -> TransactionKindPaymentAction -> Value -> ValueAllocation
allocatePaymentReturning _ _ currentValue =
    ValueAllocation
        { vaOwnAddress = currentValue
        , vaOtherPayments = mempty
        }

clientPaymentReturning :: LandTitleTransferRecording -> StateMachineClient TransactionKindPaymentState TransactionKindPaymentAction
clientPaymentReturning pn = mkStateMachineClient (instancePaymentReturning pn) allocatePaymentReturning

data ParametersLandTitleTransferCompletionInitial = ParametersLandTitleTransferCompletionInitial
    { paramLandTitleTransferCompletionInitialIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCompletionInitialInWallet :: PubKey
    , paramLandTitleTransferCompletionInitialExWallet :: PubKey
    , paramLandTitleTransferCompletionInitiallandtitle :: ByteString
    , paramLandTitleTransferCompletionInitialprice :: Integer
    , paramLandTitleTransferCompletionInitialcurrentowner :: PubKey
    , paramLandTitleTransferCompletionInitialnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferCompletionRequest = ParametersLandTitleTransferCompletionRequest
    { paramLandTitleTransferCompletionRequestIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCompletionRequestlandtitle :: ByteString
    , paramLandTitleTransferCompletionRequestprice :: Integer
    , paramLandTitleTransferCompletionRequestcurrentowner :: PubKey
    , paramLandTitleTransferCompletionRequestnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferCompletionPromise = ParametersLandTitleTransferCompletionPromise
    { paramLandTitleTransferCompletionPromiseIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCompletionPromiselandtitle :: ByteString
    , paramLandTitleTransferCompletionPromiseprice :: Integer
    , paramLandTitleTransferCompletionPromisecurrentowner :: PubKey
    , paramLandTitleTransferCompletionPromisenewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferCompletionDecline = ParametersLandTitleTransferCompletionDecline
    { paramLandTitleTransferCompletionDeclineIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCompletionDeclinelandtitle :: ByteString
    , paramLandTitleTransferCompletionDeclineprice :: Integer
    , paramLandTitleTransferCompletionDeclinecurrentowner :: PubKey
    , paramLandTitleTransferCompletionDeclinenewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferCompletionQuit = ParametersLandTitleTransferCompletionQuit
    { paramLandTitleTransferCompletionQuitIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCompletionQuitlandtitle :: ByteString
    , paramLandTitleTransferCompletionQuitprice :: Integer
    , paramLandTitleTransferCompletionQuitcurrentowner :: PubKey
    , paramLandTitleTransferCompletionQuitnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferCompletionState = ParametersLandTitleTransferCompletionState
    { paramLandTitleTransferCompletionStateIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCompletionStatelandtitle :: ByteString
    , paramLandTitleTransferCompletionStateprice :: Integer
    , paramLandTitleTransferCompletionStatecurrentowner :: PubKey
    , paramLandTitleTransferCompletionStatenewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferCompletionAccept = ParametersLandTitleTransferCompletionAccept
    { paramLandTitleTransferCompletionAcceptIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCompletionAcceptlandtitle :: ByteString
    , paramLandTitleTransferCompletionAcceptprice :: Integer
    , paramLandTitleTransferCompletionAcceptcurrentowner :: PubKey
    , paramLandTitleTransferCompletionAcceptnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferCompletionReject = ParametersLandTitleTransferCompletionReject
    { paramLandTitleTransferCompletionRejectIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCompletionRejectlandtitle :: ByteString
    , paramLandTitleTransferCompletionRejectprice :: Integer
    , paramLandTitleTransferCompletionRejectcurrentowner :: PubKey
    , paramLandTitleTransferCompletionRejectnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferCompletionStop = ParametersLandTitleTransferCompletionStop
    { paramLandTitleTransferCompletionStopIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCompletionStoplandtitle :: ByteString
    , paramLandTitleTransferCompletionStopprice :: Integer
    , paramLandTitleTransferCompletionStopcurrentowner :: PubKey
    , paramLandTitleTransferCompletionStopnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferSendingInitial = ParametersLandTitleTransferSendingInitial
    { paramLandTitleTransferSendingInitialIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferSendingInitialInWallet :: PubKey
    , paramLandTitleTransferSendingInitialExWallet :: PubKey
    , paramLandTitleTransferSendingInitiallandtitle :: ByteString
    , paramLandTitleTransferSendingInitialprice :: Integer
    , paramLandTitleTransferSendingInitialcurrentowner :: PubKey
    , paramLandTitleTransferSendingInitialnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferSendingRequest = ParametersLandTitleTransferSendingRequest
    { paramLandTitleTransferSendingRequestIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferSendingRequestlandtitle :: ByteString
    , paramLandTitleTransferSendingRequestprice :: Integer
    , paramLandTitleTransferSendingRequestcurrentowner :: PubKey
    , paramLandTitleTransferSendingRequestnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferSendingPromise = ParametersLandTitleTransferSendingPromise
    { paramLandTitleTransferSendingPromiseIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferSendingPromiselandtitle :: ByteString
    , paramLandTitleTransferSendingPromiseprice :: Integer
    , paramLandTitleTransferSendingPromisecurrentowner :: PubKey
    , paramLandTitleTransferSendingPromisenewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferSendingDecline = ParametersLandTitleTransferSendingDecline
    { paramLandTitleTransferSendingDeclineIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferSendingDeclinelandtitle :: ByteString
    , paramLandTitleTransferSendingDeclineprice :: Integer
    , paramLandTitleTransferSendingDeclinecurrentowner :: PubKey
    , paramLandTitleTransferSendingDeclinenewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferSendingQuit = ParametersLandTitleTransferSendingQuit
    { paramLandTitleTransferSendingQuitIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferSendingQuitlandtitle :: ByteString
    , paramLandTitleTransferSendingQuitprice :: Integer
    , paramLandTitleTransferSendingQuitcurrentowner :: PubKey
    , paramLandTitleTransferSendingQuitnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferSendingState = ParametersLandTitleTransferSendingState
    { paramLandTitleTransferSendingStateIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferSendingStatelandtitle :: ByteString
    , paramLandTitleTransferSendingStateprice :: Integer
    , paramLandTitleTransferSendingStatecurrentowner :: PubKey
    , paramLandTitleTransferSendingStatenewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferSendingAccept = ParametersLandTitleTransferSendingAccept
    { paramLandTitleTransferSendingAcceptIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferSendingAcceptlandtitle :: ByteString
    , paramLandTitleTransferSendingAcceptprice :: Integer
    , paramLandTitleTransferSendingAcceptcurrentowner :: PubKey
    , paramLandTitleTransferSendingAcceptnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferSendingReject = ParametersLandTitleTransferSendingReject
    { paramLandTitleTransferSendingRejectIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferSendingRejectlandtitle :: ByteString
    , paramLandTitleTransferSendingRejectprice :: Integer
    , paramLandTitleTransferSendingRejectcurrentowner :: PubKey
    , paramLandTitleTransferSendingRejectnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferSendingStop = ParametersLandTitleTransferSendingStop
    { paramLandTitleTransferSendingStopIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferSendingStoplandtitle :: ByteString
    , paramLandTitleTransferSendingStopprice :: Integer
    , paramLandTitleTransferSendingStopcurrentowner :: PubKey
    , paramLandTitleTransferSendingStopnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentSendingInitial = ParametersPaymentSendingInitial
    { paramPaymentSendingInitialIdLandTitleTransferRecording :: ByteString
    , paramPaymentSendingInitialInWallet :: PubKey
    , paramPaymentSendingInitialExWallet :: PubKey
    , paramPaymentSendingInitialamountpaid :: Integer
    , paramPaymentSendingInitialpayer :: PubKey
    , paramPaymentSendingInitialreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentSendingRequest = ParametersPaymentSendingRequest
    { paramPaymentSendingRequestIdLandTitleTransferRecording :: ByteString
    , paramPaymentSendingRequestamountpaid :: Integer
    , paramPaymentSendingRequestpayer :: PubKey
    , paramPaymentSendingRequestreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentSendingPromise = ParametersPaymentSendingPromise
    { paramPaymentSendingPromiseIdLandTitleTransferRecording :: ByteString
    , paramPaymentSendingPromiseamountpaid :: Integer
    , paramPaymentSendingPromisepayer :: PubKey
    , paramPaymentSendingPromisereceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentSendingDecline = ParametersPaymentSendingDecline
    { paramPaymentSendingDeclineIdLandTitleTransferRecording :: ByteString
    , paramPaymentSendingDeclineamountpaid :: Integer
    , paramPaymentSendingDeclinepayer :: PubKey
    , paramPaymentSendingDeclinereceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentSendingQuit = ParametersPaymentSendingQuit
    { paramPaymentSendingQuitIdLandTitleTransferRecording :: ByteString
    , paramPaymentSendingQuitamountpaid :: Integer
    , paramPaymentSendingQuitpayer :: PubKey
    , paramPaymentSendingQuitreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentSendingState = ParametersPaymentSendingState
    { paramPaymentSendingStateIdLandTitleTransferRecording :: ByteString
    , paramPaymentSendingStateamountpaid :: Integer
    , paramPaymentSendingStatepayer :: PubKey
    , paramPaymentSendingStatereceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentSendingAccept = ParametersPaymentSendingAccept
    { paramPaymentSendingAcceptIdLandTitleTransferRecording :: ByteString
    , paramPaymentSendingAcceptamountpaid :: Integer
    , paramPaymentSendingAcceptpayer :: PubKey
    , paramPaymentSendingAcceptreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentSendingReject = ParametersPaymentSendingReject
    { paramPaymentSendingRejectIdLandTitleTransferRecording :: ByteString
    , paramPaymentSendingRejectamountpaid :: Integer
    , paramPaymentSendingRejectpayer :: PubKey
    , paramPaymentSendingRejectreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentSendingStop = ParametersPaymentSendingStop
    { paramPaymentSendingStopIdLandTitleTransferRecording :: ByteString
    , paramPaymentSendingStopamountpaid :: Integer
    , paramPaymentSendingStoppayer :: PubKey
    , paramPaymentSendingStopreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentCollectionInitial = ParametersPaymentCollectionInitial
    { paramPaymentCollectionInitialIdLandTitleTransferRecording :: ByteString
    , paramPaymentCollectionInitialInWallet :: PubKey
    , paramPaymentCollectionInitialExWallet :: PubKey
    , paramPaymentCollectionInitialamountpaid :: Integer
    , paramPaymentCollectionInitialpayer :: PubKey
    , paramPaymentCollectionInitialreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentCollectionRequest = ParametersPaymentCollectionRequest
    { paramPaymentCollectionRequestIdLandTitleTransferRecording :: ByteString
    , paramPaymentCollectionRequestamountpaid :: Integer
    , paramPaymentCollectionRequestpayer :: PubKey
    , paramPaymentCollectionRequestreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentCollectionPromise = ParametersPaymentCollectionPromise
    { paramPaymentCollectionPromiseIdLandTitleTransferRecording :: ByteString
    , paramPaymentCollectionPromiseamountpaid :: Integer
    , paramPaymentCollectionPromisepayer :: PubKey
    , paramPaymentCollectionPromisereceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentCollectionDecline = ParametersPaymentCollectionDecline
    { paramPaymentCollectionDeclineIdLandTitleTransferRecording :: ByteString
    , paramPaymentCollectionDeclineamountpaid :: Integer
    , paramPaymentCollectionDeclinepayer :: PubKey
    , paramPaymentCollectionDeclinereceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentCollectionQuit = ParametersPaymentCollectionQuit
    { paramPaymentCollectionQuitIdLandTitleTransferRecording :: ByteString
    , paramPaymentCollectionQuitamountpaid :: Integer
    , paramPaymentCollectionQuitpayer :: PubKey
    , paramPaymentCollectionQuitreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentCollectionState = ParametersPaymentCollectionState
    { paramPaymentCollectionStateIdLandTitleTransferRecording :: ByteString
    , paramPaymentCollectionStateamountpaid :: Integer
    , paramPaymentCollectionStatepayer :: PubKey
    , paramPaymentCollectionStatereceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentCollectionAccept = ParametersPaymentCollectionAccept
    { paramPaymentCollectionAcceptIdLandTitleTransferRecording :: ByteString
    , paramPaymentCollectionAcceptamountpaid :: Integer
    , paramPaymentCollectionAcceptpayer :: PubKey
    , paramPaymentCollectionAcceptreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentCollectionReject = ParametersPaymentCollectionReject
    { paramPaymentCollectionRejectIdLandTitleTransferRecording :: ByteString
    , paramPaymentCollectionRejectamountpaid :: Integer
    , paramPaymentCollectionRejectpayer :: PubKey
    , paramPaymentCollectionRejectreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentCollectionStop = ParametersPaymentCollectionStop
    { paramPaymentCollectionStopIdLandTitleTransferRecording :: ByteString
    , paramPaymentCollectionStopamountpaid :: Integer
    , paramPaymentCollectionStoppayer :: PubKey
    , paramPaymentCollectionStopreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferCollectionInitial = ParametersLandTitleTransferCollectionInitial
    { paramLandTitleTransferCollectionInitialIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCollectionInitialInWallet :: PubKey
    , paramLandTitleTransferCollectionInitialExWallet :: PubKey
    , paramLandTitleTransferCollectionInitiallandtitle :: ByteString
    , paramLandTitleTransferCollectionInitialprice :: Integer
    , paramLandTitleTransferCollectionInitialcurrentowner :: PubKey
    , paramLandTitleTransferCollectionInitialnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferCollectionRequest = ParametersLandTitleTransferCollectionRequest
    { paramLandTitleTransferCollectionRequestIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCollectionRequestlandtitle :: ByteString
    , paramLandTitleTransferCollectionRequestprice :: Integer
    , paramLandTitleTransferCollectionRequestcurrentowner :: PubKey
    , paramLandTitleTransferCollectionRequestnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferCollectionPromise = ParametersLandTitleTransferCollectionPromise
    { paramLandTitleTransferCollectionPromiseIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCollectionPromiselandtitle :: ByteString
    , paramLandTitleTransferCollectionPromiseprice :: Integer
    , paramLandTitleTransferCollectionPromisecurrentowner :: PubKey
    , paramLandTitleTransferCollectionPromisenewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferCollectionDecline = ParametersLandTitleTransferCollectionDecline
    { paramLandTitleTransferCollectionDeclineIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCollectionDeclinelandtitle :: ByteString
    , paramLandTitleTransferCollectionDeclineprice :: Integer
    , paramLandTitleTransferCollectionDeclinecurrentowner :: PubKey
    , paramLandTitleTransferCollectionDeclinenewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferCollectionQuit = ParametersLandTitleTransferCollectionQuit
    { paramLandTitleTransferCollectionQuitIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCollectionQuitlandtitle :: ByteString
    , paramLandTitleTransferCollectionQuitprice :: Integer
    , paramLandTitleTransferCollectionQuitcurrentowner :: PubKey
    , paramLandTitleTransferCollectionQuitnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferCollectionState = ParametersLandTitleTransferCollectionState
    { paramLandTitleTransferCollectionStateIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCollectionStatelandtitle :: ByteString
    , paramLandTitleTransferCollectionStateprice :: Integer
    , paramLandTitleTransferCollectionStatecurrentowner :: PubKey
    , paramLandTitleTransferCollectionStatenewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferCollectionAccept = ParametersLandTitleTransferCollectionAccept
    { paramLandTitleTransferCollectionAcceptIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCollectionAcceptlandtitle :: ByteString
    , paramLandTitleTransferCollectionAcceptprice :: Integer
    , paramLandTitleTransferCollectionAcceptcurrentowner :: PubKey
    , paramLandTitleTransferCollectionAcceptnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferCollectionReject = ParametersLandTitleTransferCollectionReject
    { paramLandTitleTransferCollectionRejectIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCollectionRejectlandtitle :: ByteString
    , paramLandTitleTransferCollectionRejectprice :: Integer
    , paramLandTitleTransferCollectionRejectcurrentowner :: PubKey
    , paramLandTitleTransferCollectionRejectnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferCollectionStop = ParametersLandTitleTransferCollectionStop
    { paramLandTitleTransferCollectionStopIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferCollectionStoplandtitle :: ByteString
    , paramLandTitleTransferCollectionStopprice :: Integer
    , paramLandTitleTransferCollectionStopcurrentowner :: PubKey
    , paramLandTitleTransferCollectionStopnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferReturningInitial = ParametersLandTitleTransferReturningInitial
    { paramLandTitleTransferReturningInitialIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferReturningInitialInWallet :: PubKey
    , paramLandTitleTransferReturningInitialExWallet :: PubKey
    , paramLandTitleTransferReturningInitiallandtitle :: ByteString
    , paramLandTitleTransferReturningInitialprice :: Integer
    , paramLandTitleTransferReturningInitialcurrentowner :: PubKey
    , paramLandTitleTransferReturningInitialnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferReturningRequest = ParametersLandTitleTransferReturningRequest
    { paramLandTitleTransferReturningRequestIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferReturningRequestlandtitle :: ByteString
    , paramLandTitleTransferReturningRequestprice :: Integer
    , paramLandTitleTransferReturningRequestcurrentowner :: PubKey
    , paramLandTitleTransferReturningRequestnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferReturningPromise = ParametersLandTitleTransferReturningPromise
    { paramLandTitleTransferReturningPromiseIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferReturningPromiselandtitle :: ByteString
    , paramLandTitleTransferReturningPromiseprice :: Integer
    , paramLandTitleTransferReturningPromisecurrentowner :: PubKey
    , paramLandTitleTransferReturningPromisenewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferReturningDecline = ParametersLandTitleTransferReturningDecline
    { paramLandTitleTransferReturningDeclineIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferReturningDeclinelandtitle :: ByteString
    , paramLandTitleTransferReturningDeclineprice :: Integer
    , paramLandTitleTransferReturningDeclinecurrentowner :: PubKey
    , paramLandTitleTransferReturningDeclinenewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferReturningQuit = ParametersLandTitleTransferReturningQuit
    { paramLandTitleTransferReturningQuitIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferReturningQuitlandtitle :: ByteString
    , paramLandTitleTransferReturningQuitprice :: Integer
    , paramLandTitleTransferReturningQuitcurrentowner :: PubKey
    , paramLandTitleTransferReturningQuitnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferReturningState = ParametersLandTitleTransferReturningState
    { paramLandTitleTransferReturningStateIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferReturningStatelandtitle :: ByteString
    , paramLandTitleTransferReturningStateprice :: Integer
    , paramLandTitleTransferReturningStatecurrentowner :: PubKey
    , paramLandTitleTransferReturningStatenewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferReturningAccept = ParametersLandTitleTransferReturningAccept
    { paramLandTitleTransferReturningAcceptIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferReturningAcceptlandtitle :: ByteString
    , paramLandTitleTransferReturningAcceptprice :: Integer
    , paramLandTitleTransferReturningAcceptcurrentowner :: PubKey
    , paramLandTitleTransferReturningAcceptnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferReturningReject = ParametersLandTitleTransferReturningReject
    { paramLandTitleTransferReturningRejectIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferReturningRejectlandtitle :: ByteString
    , paramLandTitleTransferReturningRejectprice :: Integer
    , paramLandTitleTransferReturningRejectcurrentowner :: PubKey
    , paramLandTitleTransferReturningRejectnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersLandTitleTransferReturningStop = ParametersLandTitleTransferReturningStop
    { paramLandTitleTransferReturningStopIdLandTitleTransferRecording :: ByteString
    , paramLandTitleTransferReturningStoplandtitle :: ByteString
    , paramLandTitleTransferReturningStopprice :: Integer
    , paramLandTitleTransferReturningStopcurrentowner :: PubKey
    , paramLandTitleTransferReturningStopnewowner :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentReturningInitial = ParametersPaymentReturningInitial
    { paramPaymentReturningInitialIdLandTitleTransferRecording :: ByteString
    , paramPaymentReturningInitialInWallet :: PubKey
    , paramPaymentReturningInitialExWallet :: PubKey
    , paramPaymentReturningInitialamountpaid :: Integer
    , paramPaymentReturningInitialpayer :: PubKey
    , paramPaymentReturningInitialreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentReturningRequest = ParametersPaymentReturningRequest
    { paramPaymentReturningRequestIdLandTitleTransferRecording :: ByteString
    , paramPaymentReturningRequestamountpaid :: Integer
    , paramPaymentReturningRequestpayer :: PubKey
    , paramPaymentReturningRequestreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentReturningPromise = ParametersPaymentReturningPromise
    { paramPaymentReturningPromiseIdLandTitleTransferRecording :: ByteString
    , paramPaymentReturningPromiseamountpaid :: Integer
    , paramPaymentReturningPromisepayer :: PubKey
    , paramPaymentReturningPromisereceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentReturningDecline = ParametersPaymentReturningDecline
    { paramPaymentReturningDeclineIdLandTitleTransferRecording :: ByteString
    , paramPaymentReturningDeclineamountpaid :: Integer
    , paramPaymentReturningDeclinepayer :: PubKey
    , paramPaymentReturningDeclinereceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentReturningQuit = ParametersPaymentReturningQuit
    { paramPaymentReturningQuitIdLandTitleTransferRecording :: ByteString
    , paramPaymentReturningQuitamountpaid :: Integer
    , paramPaymentReturningQuitpayer :: PubKey
    , paramPaymentReturningQuitreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentReturningState = ParametersPaymentReturningState
    { paramPaymentReturningStateIdLandTitleTransferRecording :: ByteString
    , paramPaymentReturningStateamountpaid :: Integer
    , paramPaymentReturningStatepayer :: PubKey
    , paramPaymentReturningStatereceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentReturningAccept = ParametersPaymentReturningAccept
    { paramPaymentReturningAcceptIdLandTitleTransferRecording :: ByteString
    , paramPaymentReturningAcceptamountpaid :: Integer
    , paramPaymentReturningAcceptpayer :: PubKey
    , paramPaymentReturningAcceptreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentReturningReject = ParametersPaymentReturningReject
    { paramPaymentReturningRejectIdLandTitleTransferRecording :: ByteString
    , paramPaymentReturningRejectamountpaid :: Integer
    , paramPaymentReturningRejectpayer :: PubKey
    , paramPaymentReturningRejectreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ParametersPaymentReturningStop = ParametersPaymentReturningStop
    { paramPaymentReturningStopIdLandTitleTransferRecording :: ByteString
    , paramPaymentReturningStopamountpaid :: Integer
    , paramPaymentReturningStoppayer :: PubKey
    , paramPaymentReturningStopreceiver :: PubKey
    }
    deriving (Prelude.Eq, Prelude.Show, Generic, FromJSON, ToJSON, IotsType, ToSchema)



data ErrorTransactionKindLandTitleTransfer = 
    ContractErrorTransactionKindLandTitleTransfer ContractError 
    | SMErrorTransactionKindLandTitleTransfer (SMContractError TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction)
    deriving (Show)

makeClassyPrisms ''ErrorTransactionKindLandTitleTransfer

instance AsContractError ErrorTransactionKindLandTitleTransfer where
    _ContractError = _ContractErrorTransactionKindLandTitleTransfer

instance AsSMContractError ErrorTransactionKindLandTitleTransfer TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction where
    _SMContractError = _SMErrorTransactionKindLandTitleTransfer

data ErrorTransactionKindPayment = 
    ContractErrorTransactionKindPayment ContractError 
    | SMErrorTransactionKindPayment (SMContractError TransactionKindPaymentState TransactionKindPaymentAction)
    deriving (Show)

makeClassyPrisms ''ErrorTransactionKindPayment

instance AsContractError ErrorTransactionKindPayment where
    _ContractError = _ContractErrorTransactionKindPayment

instance AsSMContractError ErrorTransactionKindPayment TransactionKindPaymentState TransactionKindPaymentAction where
    _SMContractError = _SMErrorTransactionKindPayment


type Schema = BlockchainActions
    .\/ Endpoint "doLandTitleTransferCompletionInitial" ParametersLandTitleTransferCompletionInitial
    .\/ Endpoint "doLandTitleTransferCompletionRequest" ParametersLandTitleTransferCompletionRequest
    .\/ Endpoint "doLandTitleTransferCompletionPromise" ParametersLandTitleTransferCompletionPromise
    .\/ Endpoint "doLandTitleTransferCompletionDecline" ParametersLandTitleTransferCompletionDecline
    .\/ Endpoint "doLandTitleTransferCompletionQuit" ParametersLandTitleTransferCompletionQuit
    .\/ Endpoint "doLandTitleTransferCompletionState" ParametersLandTitleTransferCompletionState
    .\/ Endpoint "doLandTitleTransferCompletionAccept" ParametersLandTitleTransferCompletionAccept
    .\/ Endpoint "doLandTitleTransferCompletionReject" ParametersLandTitleTransferCompletionReject
    .\/ Endpoint "doLandTitleTransferCompletionStop" ParametersLandTitleTransferCompletionStop
    .\/ Endpoint "doLandTitleTransferSendingInitial" ParametersLandTitleTransferSendingInitial
    .\/ Endpoint "doLandTitleTransferSendingRequest" ParametersLandTitleTransferSendingRequest
    .\/ Endpoint "doLandTitleTransferSendingPromise" ParametersLandTitleTransferSendingPromise
    .\/ Endpoint "doLandTitleTransferSendingDecline" ParametersLandTitleTransferSendingDecline
    .\/ Endpoint "doLandTitleTransferSendingQuit" ParametersLandTitleTransferSendingQuit
    .\/ Endpoint "doLandTitleTransferSendingState" ParametersLandTitleTransferSendingState
    .\/ Endpoint "doLandTitleTransferSendingAccept" ParametersLandTitleTransferSendingAccept
    .\/ Endpoint "doLandTitleTransferSendingReject" ParametersLandTitleTransferSendingReject
    .\/ Endpoint "doLandTitleTransferSendingStop" ParametersLandTitleTransferSendingStop
    .\/ Endpoint "doPaymentSendingInitial" ParametersPaymentSendingInitial
    .\/ Endpoint "doPaymentSendingRequest" ParametersPaymentSendingRequest
    .\/ Endpoint "doPaymentSendingPromise" ParametersPaymentSendingPromise
    .\/ Endpoint "doPaymentSendingDecline" ParametersPaymentSendingDecline
    .\/ Endpoint "doPaymentSendingQuit" ParametersPaymentSendingQuit
    .\/ Endpoint "doPaymentSendingState" ParametersPaymentSendingState
    .\/ Endpoint "doPaymentSendingAccept" ParametersPaymentSendingAccept
    .\/ Endpoint "doPaymentSendingReject" ParametersPaymentSendingReject
    .\/ Endpoint "doPaymentSendingStop" ParametersPaymentSendingStop
    .\/ Endpoint "doPaymentCollectionInitial" ParametersPaymentCollectionInitial
    .\/ Endpoint "doPaymentCollectionRequest" ParametersPaymentCollectionRequest
    .\/ Endpoint "doPaymentCollectionPromise" ParametersPaymentCollectionPromise
    .\/ Endpoint "doPaymentCollectionDecline" ParametersPaymentCollectionDecline
    .\/ Endpoint "doPaymentCollectionQuit" ParametersPaymentCollectionQuit
    .\/ Endpoint "doPaymentCollectionState" ParametersPaymentCollectionState
    .\/ Endpoint "doPaymentCollectionAccept" ParametersPaymentCollectionAccept
    .\/ Endpoint "doPaymentCollectionReject" ParametersPaymentCollectionReject
    .\/ Endpoint "doPaymentCollectionStop" ParametersPaymentCollectionStop
    .\/ Endpoint "doLandTitleTransferCollectionInitial" ParametersLandTitleTransferCollectionInitial
    .\/ Endpoint "doLandTitleTransferCollectionRequest" ParametersLandTitleTransferCollectionRequest
    .\/ Endpoint "doLandTitleTransferCollectionPromise" ParametersLandTitleTransferCollectionPromise
    .\/ Endpoint "doLandTitleTransferCollectionDecline" ParametersLandTitleTransferCollectionDecline
    .\/ Endpoint "doLandTitleTransferCollectionQuit" ParametersLandTitleTransferCollectionQuit
    .\/ Endpoint "doLandTitleTransferCollectionState" ParametersLandTitleTransferCollectionState
    .\/ Endpoint "doLandTitleTransferCollectionAccept" ParametersLandTitleTransferCollectionAccept
    .\/ Endpoint "doLandTitleTransferCollectionReject" ParametersLandTitleTransferCollectionReject
    .\/ Endpoint "doLandTitleTransferCollectionStop" ParametersLandTitleTransferCollectionStop
    .\/ Endpoint "doLandTitleTransferReturningInitial" ParametersLandTitleTransferReturningInitial
    .\/ Endpoint "doLandTitleTransferReturningRequest" ParametersLandTitleTransferReturningRequest
    .\/ Endpoint "doLandTitleTransferReturningPromise" ParametersLandTitleTransferReturningPromise
    .\/ Endpoint "doLandTitleTransferReturningDecline" ParametersLandTitleTransferReturningDecline
    .\/ Endpoint "doLandTitleTransferReturningQuit" ParametersLandTitleTransferReturningQuit
    .\/ Endpoint "doLandTitleTransferReturningState" ParametersLandTitleTransferReturningState
    .\/ Endpoint "doLandTitleTransferReturningAccept" ParametersLandTitleTransferReturningAccept
    .\/ Endpoint "doLandTitleTransferReturningReject" ParametersLandTitleTransferReturningReject
    .\/ Endpoint "doLandTitleTransferReturningStop" ParametersLandTitleTransferReturningStop
    .\/ Endpoint "doPaymentReturningInitial" ParametersPaymentReturningInitial
    .\/ Endpoint "doPaymentReturningRequest" ParametersPaymentReturningRequest
    .\/ Endpoint "doPaymentReturningPromise" ParametersPaymentReturningPromise
    .\/ Endpoint "doPaymentReturningDecline" ParametersPaymentReturningDecline
    .\/ Endpoint "doPaymentReturningQuit" ParametersPaymentReturningQuit
    .\/ Endpoint "doPaymentReturningState" ParametersPaymentReturningState
    .\/ Endpoint "doPaymentReturningAccept" ParametersPaymentReturningAccept
    .\/ Endpoint "doPaymentReturningReject" ParametersPaymentReturningReject
    .\/ Endpoint "doPaymentReturningStop" ParametersPaymentReturningStop

mkSchemaDefinitions ''Schema

doLandTitleTransferCompletionInitial :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCompletionInitial = do
    params <- endpoint @"doLandTitleTransferCompletionInitial" @ParametersLandTitleTransferCompletionInitial
    let pn = LandTitleTransferRecording (paramLandTitleTransferCompletionInitialIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferCompletionRequest :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCompletionRequest = do
    params <- endpoint @"doLandTitleTransferCompletionRequest" @ParametersLandTitleTransferCompletionRequest
    let pn = LandTitleTransferRecording (paramLandTitleTransferCompletionRequestIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferCompletionPromise :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCompletionPromise = do
    params <- endpoint @"doLandTitleTransferCompletionPromise" @ParametersLandTitleTransferCompletionPromise
    let pn = LandTitleTransferRecording (paramLandTitleTransferCompletionPromiseIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferCompletionDecline :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCompletionDecline = do
    params <- endpoint @"doLandTitleTransferCompletionDecline" @ParametersLandTitleTransferCompletionDecline
    let pn = LandTitleTransferRecording (paramLandTitleTransferCompletionDeclineIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferCompletionQuit :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCompletionQuit = do
    params <- endpoint @"doLandTitleTransferCompletionQuit" @ParametersLandTitleTransferCompletionQuit
    let pn = LandTitleTransferRecording (paramLandTitleTransferCompletionQuitIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferCompletionState :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCompletionState = do
    params <- endpoint @"doLandTitleTransferCompletionState" @ParametersLandTitleTransferCompletionState
    let pn = LandTitleTransferRecording (paramLandTitleTransferCompletionStateIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferCompletionAccept :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCompletionAccept = do
    params <- endpoint @"doLandTitleTransferCompletionAccept" @ParametersLandTitleTransferCompletionAccept
    let pn = LandTitleTransferRecording (paramLandTitleTransferCompletionAcceptIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferCompletionReject :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCompletionReject = do
    params <- endpoint @"doLandTitleTransferCompletionReject" @ParametersLandTitleTransferCompletionReject
    let pn = LandTitleTransferRecording (paramLandTitleTransferCompletionRejectIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferCompletionStop :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCompletionStop = do
    params <- endpoint @"doLandTitleTransferCompletionStop" @ParametersLandTitleTransferCompletionStop
    let pn = LandTitleTransferRecording (paramLandTitleTransferCompletionStopIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferSendingInitial :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferSendingInitial = do
    params <- endpoint @"doLandTitleTransferSendingInitial" @ParametersLandTitleTransferSendingInitial
    let pn = LandTitleTransferRecording (paramLandTitleTransferSendingInitialIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferSendingRequest :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferSendingRequest = do
    params <- endpoint @"doLandTitleTransferSendingRequest" @ParametersLandTitleTransferSendingRequest
    let pn = LandTitleTransferRecording (paramLandTitleTransferSendingRequestIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferSendingPromise :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferSendingPromise = do
    params <- endpoint @"doLandTitleTransferSendingPromise" @ParametersLandTitleTransferSendingPromise
    let pn = LandTitleTransferRecording (paramLandTitleTransferSendingPromiseIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferSendingDecline :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferSendingDecline = do
    params <- endpoint @"doLandTitleTransferSendingDecline" @ParametersLandTitleTransferSendingDecline
    let pn = LandTitleTransferRecording (paramLandTitleTransferSendingDeclineIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferSendingQuit :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferSendingQuit = do
    params <- endpoint @"doLandTitleTransferSendingQuit" @ParametersLandTitleTransferSendingQuit
    let pn = LandTitleTransferRecording (paramLandTitleTransferSendingQuitIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferSendingState :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferSendingState = do
    params <- endpoint @"doLandTitleTransferSendingState" @ParametersLandTitleTransferSendingState
    let pn = LandTitleTransferRecording (paramLandTitleTransferSendingStateIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferSendingAccept :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferSendingAccept = do
    params <- endpoint @"doLandTitleTransferSendingAccept" @ParametersLandTitleTransferSendingAccept
    let pn = LandTitleTransferRecording (paramLandTitleTransferSendingAcceptIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferSendingReject :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferSendingReject = do
    params <- endpoint @"doLandTitleTransferSendingReject" @ParametersLandTitleTransferSendingReject
    let pn = LandTitleTransferRecording (paramLandTitleTransferSendingRejectIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferSendingStop :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferSendingStop = do
    params <- endpoint @"doLandTitleTransferSendingStop" @ParametersLandTitleTransferSendingStop
    let pn = LandTitleTransferRecording (paramLandTitleTransferSendingStopIdLandTitleTransferRecording params)
    pure ()

doPaymentSendingInitial :: (AsContractError e
    ) => Contract Schema e ()
doPaymentSendingInitial = do
    params <- endpoint @"doPaymentSendingInitial" @ParametersPaymentSendingInitial
    let pn = LandTitleTransferRecording (paramPaymentSendingInitialIdLandTitleTransferRecording params)
    pure ()

doPaymentSendingRequest :: (AsContractError e
    ) => Contract Schema e ()
doPaymentSendingRequest = do
    params <- endpoint @"doPaymentSendingRequest" @ParametersPaymentSendingRequest
    let pn = LandTitleTransferRecording (paramPaymentSendingRequestIdLandTitleTransferRecording params)
    pure ()

doPaymentSendingPromise :: (AsContractError e
    ) => Contract Schema e ()
doPaymentSendingPromise = do
    params <- endpoint @"doPaymentSendingPromise" @ParametersPaymentSendingPromise
    let pn = LandTitleTransferRecording (paramPaymentSendingPromiseIdLandTitleTransferRecording params)
    pure ()

doPaymentSendingDecline :: (AsContractError e
    ) => Contract Schema e ()
doPaymentSendingDecline = do
    params <- endpoint @"doPaymentSendingDecline" @ParametersPaymentSendingDecline
    let pn = LandTitleTransferRecording (paramPaymentSendingDeclineIdLandTitleTransferRecording params)
    pure ()

doPaymentSendingQuit :: (AsContractError e
    ) => Contract Schema e ()
doPaymentSendingQuit = do
    params <- endpoint @"doPaymentSendingQuit" @ParametersPaymentSendingQuit
    let pn = LandTitleTransferRecording (paramPaymentSendingQuitIdLandTitleTransferRecording params)
    pure ()

doPaymentSendingState :: (AsContractError e
    ) => Contract Schema e ()
doPaymentSendingState = do
    params <- endpoint @"doPaymentSendingState" @ParametersPaymentSendingState
    let pn = LandTitleTransferRecording (paramPaymentSendingStateIdLandTitleTransferRecording params)
    pure ()

doPaymentSendingAccept :: (AsContractError e
    ) => Contract Schema e ()
doPaymentSendingAccept = do
    params <- endpoint @"doPaymentSendingAccept" @ParametersPaymentSendingAccept
    let pn = LandTitleTransferRecording (paramPaymentSendingAcceptIdLandTitleTransferRecording params)
    pure ()

doPaymentSendingReject :: (AsContractError e
    ) => Contract Schema e ()
doPaymentSendingReject = do
    params <- endpoint @"doPaymentSendingReject" @ParametersPaymentSendingReject
    let pn = LandTitleTransferRecording (paramPaymentSendingRejectIdLandTitleTransferRecording params)
    pure ()

doPaymentSendingStop :: (AsContractError e
    ) => Contract Schema e ()
doPaymentSendingStop = do
    params <- endpoint @"doPaymentSendingStop" @ParametersPaymentSendingStop
    let pn = LandTitleTransferRecording (paramPaymentSendingStopIdLandTitleTransferRecording params)
    pure ()

doPaymentCollectionInitial :: (AsContractError e
    ) => Contract Schema e ()
doPaymentCollectionInitial = do
    params <- endpoint @"doPaymentCollectionInitial" @ParametersPaymentCollectionInitial
    let pn = LandTitleTransferRecording (paramPaymentCollectionInitialIdLandTitleTransferRecording params)
    pure ()

doPaymentCollectionRequest :: (AsContractError e
    ) => Contract Schema e ()
doPaymentCollectionRequest = do
    params <- endpoint @"doPaymentCollectionRequest" @ParametersPaymentCollectionRequest
    let pn = LandTitleTransferRecording (paramPaymentCollectionRequestIdLandTitleTransferRecording params)
    pure ()

doPaymentCollectionPromise :: (AsContractError e
    ) => Contract Schema e ()
doPaymentCollectionPromise = do
    params <- endpoint @"doPaymentCollectionPromise" @ParametersPaymentCollectionPromise
    let pn = LandTitleTransferRecording (paramPaymentCollectionPromiseIdLandTitleTransferRecording params)
    pure ()

doPaymentCollectionDecline :: (AsContractError e
    ) => Contract Schema e ()
doPaymentCollectionDecline = do
    params <- endpoint @"doPaymentCollectionDecline" @ParametersPaymentCollectionDecline
    let pn = LandTitleTransferRecording (paramPaymentCollectionDeclineIdLandTitleTransferRecording params)
    pure ()

doPaymentCollectionQuit :: (AsContractError e
    ) => Contract Schema e ()
doPaymentCollectionQuit = do
    params <- endpoint @"doPaymentCollectionQuit" @ParametersPaymentCollectionQuit
    let pn = LandTitleTransferRecording (paramPaymentCollectionQuitIdLandTitleTransferRecording params)
    pure ()

doPaymentCollectionState :: (AsContractError e
    ) => Contract Schema e ()
doPaymentCollectionState = do
    params <- endpoint @"doPaymentCollectionState" @ParametersPaymentCollectionState
    let pn = LandTitleTransferRecording (paramPaymentCollectionStateIdLandTitleTransferRecording params)
    pure ()

doPaymentCollectionAccept :: (AsContractError e
    ) => Contract Schema e ()
doPaymentCollectionAccept = do
    params <- endpoint @"doPaymentCollectionAccept" @ParametersPaymentCollectionAccept
    let pn = LandTitleTransferRecording (paramPaymentCollectionAcceptIdLandTitleTransferRecording params)
    pure ()

doPaymentCollectionReject :: (AsContractError e
    ) => Contract Schema e ()
doPaymentCollectionReject = do
    params <- endpoint @"doPaymentCollectionReject" @ParametersPaymentCollectionReject
    let pn = LandTitleTransferRecording (paramPaymentCollectionRejectIdLandTitleTransferRecording params)
    pure ()

doPaymentCollectionStop :: (AsContractError e
    ) => Contract Schema e ()
doPaymentCollectionStop = do
    params <- endpoint @"doPaymentCollectionStop" @ParametersPaymentCollectionStop
    let pn = LandTitleTransferRecording (paramPaymentCollectionStopIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferCollectionInitial :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCollectionInitial = do
    params <- endpoint @"doLandTitleTransferCollectionInitial" @ParametersLandTitleTransferCollectionInitial
    let pn = LandTitleTransferRecording (paramLandTitleTransferCollectionInitialIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferCollectionRequest :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCollectionRequest = do
    params <- endpoint @"doLandTitleTransferCollectionRequest" @ParametersLandTitleTransferCollectionRequest
    let pn = LandTitleTransferRecording (paramLandTitleTransferCollectionRequestIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferCollectionPromise :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCollectionPromise = do
    params <- endpoint @"doLandTitleTransferCollectionPromise" @ParametersLandTitleTransferCollectionPromise
    let pn = LandTitleTransferRecording (paramLandTitleTransferCollectionPromiseIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferCollectionDecline :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCollectionDecline = do
    params <- endpoint @"doLandTitleTransferCollectionDecline" @ParametersLandTitleTransferCollectionDecline
    let pn = LandTitleTransferRecording (paramLandTitleTransferCollectionDeclineIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferCollectionQuit :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCollectionQuit = do
    params <- endpoint @"doLandTitleTransferCollectionQuit" @ParametersLandTitleTransferCollectionQuit
    let pn = LandTitleTransferRecording (paramLandTitleTransferCollectionQuitIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferCollectionState :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCollectionState = do
    params <- endpoint @"doLandTitleTransferCollectionState" @ParametersLandTitleTransferCollectionState
    let pn = LandTitleTransferRecording (paramLandTitleTransferCollectionStateIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferCollectionAccept :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCollectionAccept = do
    params <- endpoint @"doLandTitleTransferCollectionAccept" @ParametersLandTitleTransferCollectionAccept
    let pn = LandTitleTransferRecording (paramLandTitleTransferCollectionAcceptIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferCollectionReject :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCollectionReject = do
    params <- endpoint @"doLandTitleTransferCollectionReject" @ParametersLandTitleTransferCollectionReject
    let pn = LandTitleTransferRecording (paramLandTitleTransferCollectionRejectIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferCollectionStop :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferCollectionStop = do
    params <- endpoint @"doLandTitleTransferCollectionStop" @ParametersLandTitleTransferCollectionStop
    let pn = LandTitleTransferRecording (paramLandTitleTransferCollectionStopIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferReturningInitial :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferReturningInitial = do
    params <- endpoint @"doLandTitleTransferReturningInitial" @ParametersLandTitleTransferReturningInitial
    let pn = LandTitleTransferRecording (paramLandTitleTransferReturningInitialIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferReturningRequest :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferReturningRequest = do
    params <- endpoint @"doLandTitleTransferReturningRequest" @ParametersLandTitleTransferReturningRequest
    let pn = LandTitleTransferRecording (paramLandTitleTransferReturningRequestIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferReturningPromise :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferReturningPromise = do
    params <- endpoint @"doLandTitleTransferReturningPromise" @ParametersLandTitleTransferReturningPromise
    let pn = LandTitleTransferRecording (paramLandTitleTransferReturningPromiseIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferReturningDecline :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferReturningDecline = do
    params <- endpoint @"doLandTitleTransferReturningDecline" @ParametersLandTitleTransferReturningDecline
    let pn = LandTitleTransferRecording (paramLandTitleTransferReturningDeclineIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferReturningQuit :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferReturningQuit = do
    params <- endpoint @"doLandTitleTransferReturningQuit" @ParametersLandTitleTransferReturningQuit
    let pn = LandTitleTransferRecording (paramLandTitleTransferReturningQuitIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferReturningState :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferReturningState = do
    params <- endpoint @"doLandTitleTransferReturningState" @ParametersLandTitleTransferReturningState
    let pn = LandTitleTransferRecording (paramLandTitleTransferReturningStateIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferReturningAccept :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferReturningAccept = do
    params <- endpoint @"doLandTitleTransferReturningAccept" @ParametersLandTitleTransferReturningAccept
    let pn = LandTitleTransferRecording (paramLandTitleTransferReturningAcceptIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferReturningReject :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferReturningReject = do
    params <- endpoint @"doLandTitleTransferReturningReject" @ParametersLandTitleTransferReturningReject
    let pn = LandTitleTransferRecording (paramLandTitleTransferReturningRejectIdLandTitleTransferRecording params)
    pure ()

doLandTitleTransferReturningStop :: (AsContractError e
    ) => Contract Schema e ()
doLandTitleTransferReturningStop = do
    params <- endpoint @"doLandTitleTransferReturningStop" @ParametersLandTitleTransferReturningStop
    let pn = LandTitleTransferRecording (paramLandTitleTransferReturningStopIdLandTitleTransferRecording params)
    pure ()

doPaymentReturningInitial :: (AsContractError e
    ) => Contract Schema e ()
doPaymentReturningInitial = do
    params <- endpoint @"doPaymentReturningInitial" @ParametersPaymentReturningInitial
    let pn = LandTitleTransferRecording (paramPaymentReturningInitialIdLandTitleTransferRecording params)
    pure ()

doPaymentReturningRequest :: (AsContractError e
    ) => Contract Schema e ()
doPaymentReturningRequest = do
    params <- endpoint @"doPaymentReturningRequest" @ParametersPaymentReturningRequest
    let pn = LandTitleTransferRecording (paramPaymentReturningRequestIdLandTitleTransferRecording params)
    pure ()

doPaymentReturningPromise :: (AsContractError e
    ) => Contract Schema e ()
doPaymentReturningPromise = do
    params <- endpoint @"doPaymentReturningPromise" @ParametersPaymentReturningPromise
    let pn = LandTitleTransferRecording (paramPaymentReturningPromiseIdLandTitleTransferRecording params)
    pure ()

doPaymentReturningDecline :: (AsContractError e
    ) => Contract Schema e ()
doPaymentReturningDecline = do
    params <- endpoint @"doPaymentReturningDecline" @ParametersPaymentReturningDecline
    let pn = LandTitleTransferRecording (paramPaymentReturningDeclineIdLandTitleTransferRecording params)
    pure ()

doPaymentReturningQuit :: (AsContractError e
    ) => Contract Schema e ()
doPaymentReturningQuit = do
    params <- endpoint @"doPaymentReturningQuit" @ParametersPaymentReturningQuit
    let pn = LandTitleTransferRecording (paramPaymentReturningQuitIdLandTitleTransferRecording params)
    pure ()

doPaymentReturningState :: (AsContractError e
    ) => Contract Schema e ()
doPaymentReturningState = do
    params <- endpoint @"doPaymentReturningState" @ParametersPaymentReturningState
    let pn = LandTitleTransferRecording (paramPaymentReturningStateIdLandTitleTransferRecording params)
    pure ()

doPaymentReturningAccept :: (AsContractError e
    ) => Contract Schema e ()
doPaymentReturningAccept = do
    params <- endpoint @"doPaymentReturningAccept" @ParametersPaymentReturningAccept
    let pn = LandTitleTransferRecording (paramPaymentReturningAcceptIdLandTitleTransferRecording params)
    pure ()

doPaymentReturningReject :: (AsContractError e
    ) => Contract Schema e ()
doPaymentReturningReject = do
    params <- endpoint @"doPaymentReturningReject" @ParametersPaymentReturningReject
    let pn = LandTitleTransferRecording (paramPaymentReturningRejectIdLandTitleTransferRecording params)
    pure ()

doPaymentReturningStop :: (AsContractError e
    ) => Contract Schema e ()
doPaymentReturningStop = do
    params <- endpoint @"doPaymentReturningStop" @ParametersPaymentReturningStop
    let pn = LandTitleTransferRecording (paramPaymentReturningStopIdLandTitleTransferRecording params)
    pure ()


endpoints :: (AsContractError e
    , AsSMContractError e TransactionKindLandTitleTransferState TransactionKindLandTitleTransferAction
    , AsSMContractError e TransactionKindPaymentState TransactionKindPaymentAction
    ) => Contract Schema e ()
endpoints = doLandTitleTransferCompletionInitial <|> doLandTitleTransferCompletionRequest <|> doLandTitleTransferCompletionPromise <|> doLandTitleTransferCompletionDecline <|> doLandTitleTransferCompletionQuit <|> doLandTitleTransferCompletionState <|> doLandTitleTransferCompletionAccept <|> doLandTitleTransferCompletionReject <|> doLandTitleTransferCompletionStop <|> doLandTitleTransferSendingInitial <|> doLandTitleTransferSendingRequest <|> doLandTitleTransferSendingPromise <|> doLandTitleTransferSendingDecline <|> doLandTitleTransferSendingQuit <|> doLandTitleTransferSendingState <|> doLandTitleTransferSendingAccept <|> doLandTitleTransferSendingReject <|> doLandTitleTransferSendingStop <|> doPaymentSendingInitial <|> doPaymentSendingRequest <|> doPaymentSendingPromise <|> doPaymentSendingDecline <|> doPaymentSendingQuit <|> doPaymentSendingState <|> doPaymentSendingAccept <|> doPaymentSendingReject <|> doPaymentSendingStop <|> doPaymentCollectionInitial <|> doPaymentCollectionRequest <|> doPaymentCollectionPromise <|> doPaymentCollectionDecline <|> doPaymentCollectionQuit <|> doPaymentCollectionState <|> doPaymentCollectionAccept <|> doPaymentCollectionReject <|> doPaymentCollectionStop <|> doLandTitleTransferCollectionInitial <|> doLandTitleTransferCollectionRequest <|> doLandTitleTransferCollectionPromise <|> doLandTitleTransferCollectionDecline <|> doLandTitleTransferCollectionQuit <|> doLandTitleTransferCollectionState <|> doLandTitleTransferCollectionAccept <|> doLandTitleTransferCollectionReject <|> doLandTitleTransferCollectionStop <|> doLandTitleTransferReturningInitial <|> doLandTitleTransferReturningRequest <|> doLandTitleTransferReturningPromise <|> doLandTitleTransferReturningDecline <|> doLandTitleTransferReturningQuit <|> doLandTitleTransferReturningState <|> doLandTitleTransferReturningAccept <|> doLandTitleTransferReturningReject <|> doLandTitleTransferReturningStop <|> doPaymentReturningInitial <|> doPaymentReturningRequest <|> doPaymentReturningPromise <|> doPaymentReturningDecline <|> doPaymentReturningQuit <|> doPaymentReturningState <|> doPaymentReturningAccept <|> doPaymentReturningReject <|> doPaymentReturningStop
