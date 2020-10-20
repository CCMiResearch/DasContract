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
    }
}
