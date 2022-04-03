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

        protected string GetCountTarget(Task taskElement)
        {
            if (taskElement.LoopCollection != null)
            {
                if (processConverter.Contract.TryGetProperty(taskElement.LoopCollection, out var property, out var entity))
                {
                    if (!entity.IsRootEntity)
                        return null; //TODO Exception
                    if (property.PropertyType == PropertyType.Collection)
                        return $"{property.Name.ToLowerCamelCase()}.length";
                    if (property.PropertyType == PropertyType.Dictionary)
                        return $"{ConversionTemplates.MappingKeysArrayName(property.Name.ToLowerCamelCase())}.length";

                }
            }
            else if (taskElement.LoopCardinality != "0")
            {
                return taskElement.LoopCardinality.ToString();
            }
            return null; //TODO exception
        }


    }
}
