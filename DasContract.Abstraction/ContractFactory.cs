using DasContract.Abstraction.Processes;

namespace DasContract.Abstraction
{
    public class ContractFactory
    {
        public static Contract FromBpmn(string bpmnXml)
        {
            var contract = new Contract
            {
                Process = new Process(bpmnXml)
            };

            return contract; 
        }
    }
}
