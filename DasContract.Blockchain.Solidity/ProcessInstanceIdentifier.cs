using DasContract.Blockchain.Solidity.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity
{
    public class ProcessInstanceIdentifier
    {
        private string callActivityCallName;

        public string IdentifierName { get { return ConversionTemplates.IdentifierVariableName(callActivityCallName); } }

        public ProcessInstanceIdentifier(string callActivityCallName)
        {
            this.callActivityCallName = callActivityCallName;
        }
    }
}
