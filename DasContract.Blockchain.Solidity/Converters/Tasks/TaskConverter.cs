using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.Converters.Tasks
{
    public abstract class TaskConverter: ElementConverter
    {
        protected List<SolidityStatement> boundaryEventCalls = new List<SolidityStatement>();

        public void AddBoundaryEventCall(SolidityStatement eventCall)
        {
            boundaryEventCalls.Add(eventCall);
            //Update the logic
            ConvertElementLogic();
        }

        SolidityStatement CreateLoopVariableDefinitionStatement(Task element) 
        {
            var elementCallName = GetElementCallName(element);
            if (element.LoopCardinality != 0)
            {
                var variableName = ConversionTemplates.MultiInstanceCountVariable(GetElementCallName(element));
                return new SolidityStatement($"int {variableName}");
            }
            else if(element.LoopCollection != null)
            {
                var collectionEntity = processConverter.ContractConverter.GetDataTypeOfType<Entity>(element.LoopCollection);
                var entityName = Helpers.ToUpperCamelCase(collectionEntity.Name);
                var variableName = ConversionTemplates.MultiInstanceCollectionVariable(GetElementCallName(element));

                return new SolidityStatement($"{entityName}[] {variableName}");
            }
            else
            {
                //TODO: Exception
                return null;
            }
        }

        SolidityStatement CreateLoopVariableAssignmentStatement(Task element)
        {
            if (element.LoopCardinality != 0)
            {
                var variableName = ConversionTemplates.MultiInstanceCountVariable(GetElementCallName(element));
                return new SolidityStatement($"{variableName} = {element.LoopCardinality}");
            }
            else if (element.LoopCollection != null)
            {
                var collectionEntity = processConverter.ContractConverter.GetDataTypeOfType<Entity>(element.LoopCollection);
                var variableName = ConversionTemplates.MultiInstanceCollectionVariable(GetElementCallName(element));
                return new SolidityStatement($"{Helpers.ToUpperCamelCase(collectionEntity.Name)}[] {variableName} = ");
            }
            else
            {
                //TODO: Exception
                return null;
            }
        }
    }
}
