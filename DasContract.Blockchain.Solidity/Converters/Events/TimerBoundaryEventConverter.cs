using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Blockchain.Solidity.Converters.Tasks;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.Converters.Events
{
    public class TimerBoundaryEventConverter : ElementConverter
    {
        TimerBoundaryEvent eventElement;

        SolidityFunction touchFunction;
        SolidityStatement timerConstantStatement;

        public TimerBoundaryEventConverter(TimerBoundaryEvent eventElement, ProcessConverter processConverter)
        {
            this.eventElement = eventElement;
            this.processConverter = processConverter;
        }

        public override void ConvertElementLogic()
        {
            var attachedToConverter = processConverter.GetConverterOfElementOfType<TaskConverter>(eventElement.AttachedTo);
            touchFunction = CreateTouchFunction(attachedToConverter);
            AddTouchCheckToAttachedElement(attachedToConverter);
        }

        void AddTouchCheckToAttachedElement(TaskConverter attachedToConverter)
        {
            attachedToConverter.AddBoundaryEventCall(new SolidityStatement($"require(touch{GetElementCallName()}({processConverter.GetIdentifierNames()}))"));
        }

        string GetTimerCondition()
        {
            if (eventElement.TimerDefinitionType == TimerDefinitionType.Date)
            {
                if (eventElement.TimerDefinition.StartsWith("$"))
                {
                    var trimmed = eventElement.TimerDefinition.Trim();
                    return trimmed.Substring(2, trimmed.Length - 3);
                }
                else
                {
                    var datetime = eventElement.TimerDefinition.ParseISO8601String();
                    long unixTime = ((DateTimeOffset)datetime).ToUnixTimeSeconds();
                    return unixTime.ToString();
                }
            }
            else if (eventElement.TimerDefinitionType == TimerDefinitionType.Duration) //TODO
                return null;
            return null; //TODO throw exception
        }

        SolidityFunction CreateTouchFunction(TaskConverter attachedToConverter)
        {
            var function = new SolidityFunction($"touch{GetElementCallName()}", SolidityVisibility.Public, "bool");
            function.AddParameters(processConverter.GetIdentifiersAsParameters());
            function.AddModifier($"{ConversionTemplates.StateGuardModifierName(attachedToConverter.GetElementCallName())}({processConverter.GetIdentifierNames()})");

            var solidityCondition = new SolidityIfElse();
            solidityCondition.AddConditionBlock($"now > {GetTimerCondition()}", CreateTouchFunctionLogic(attachedToConverter));
            function.AddToBody(solidityCondition);
            function.AddToBody(new SolidityStatement("return true"));
            return function;
        }

        SolidityStatement CreateTouchFunctionLogic(TaskConverter attachedToConverter)
        {
            var touchLogic = new SolidityStatement();
            //Disable the current state
            touchLogic.Add(GetChangeActiveStateStatement(false));
            //place the call statement of the next element
            touchLogic.Add(processConverter.GetStatementOfNextElement(eventElement));
            touchLogic.Add(new SolidityStatement("return false"));
            return touchLogic;
        }
        

        public override string GetElementCallName()
        {
            return GetElementCallName(eventElement);
        }

        public override string GetElementId()
        {
            return eventElement.Id;
        }

        public override IList<SolidityComponent> GetGeneratedSolidityComponents()
        {
            return new List<SolidityComponent>()
            {
                touchFunction
            };
        }

        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            throw new NotImplementedException();
        }
    }
}
