using DasContract.Abstraction.Processes;
using System.Collections.Generic;
using DasContract.Blockchain.Solidity.SolidityComponents;

namespace DasContract.Blockchain.Solidity.Converters
{

    public abstract class ElementConverter
    {
        protected ProcessConverter processConverter;

        public abstract void ConvertElementLogic();
        /// <summary>
        /// Returns generated logic for the given element
        /// </summary>
        /// <param name="nextElements"></param>
        /// <param name="outgoingSeqFlows"></param>
        /// <returns></returns>
        public abstract IList<SolidityComponent> GetGeneratedSolidityComponents();
        /// <summary>
        /// Returns a statement that allows the previous element to call the main logic function
        /// </summary>
        /// <returns></returns>
        public abstract SolidityStatement GetStatementForPrevious(ProcessElement previous);

        public abstract string GetElementId();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A string specifying how the function/other logic will be named</returns>
        public abstract string GetElementCallName();

        protected SolidityStatement GetChangeActiveStateStatement(bool active)
        {
            string statement = $"{ConversionTemplates.ActiveStatesMappingName(processConverter.Id)}";
            foreach(var identifier in processConverter.InstanceIdentifiers)
            {
                statement += $"[{identifier.IdentifierName}]";
            }
            statement += $"[\"{GetElementCallName()}\"] = {Helpers.ToLowerCamelCase(active.ToString())}";
            return new SolidityStatement(statement);
        }

        protected string GetElementCallName<T>(T element) where T: ProcessElement
        {
            return processConverter.GetElementCallName(element);
        }

        protected SolidityStatement GetFunctionCallStatement()
        {
            return new SolidityStatement($"{GetElementCallName()}({processConverter.GetIdentifierNames()})");
        }
    }
}
