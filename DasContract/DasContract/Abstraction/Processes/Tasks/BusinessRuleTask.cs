using DasContract.Abstraction.Interface.Processes.Tasks;

namespace DasContract.Abstraction.Processes.Tasks
{
    public class BusinessRuleTask : Task, ICustomDataCopyable<BusinessRuleTask>
    {
        /// <summary>
        /// A definition of a business rule in xml format. 
        /// </summary>
        public string BusinessRuleDefinitionXml { get; set; }

        public void CopyCustomDataFrom(BusinessRuleTask task)
        {
            BusinessRuleDefinitionXml = task.BusinessRuleDefinitionXml;
        }
    }
}
