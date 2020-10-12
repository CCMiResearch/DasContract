using DasContract.Abstraction.Data;
using DasContract.Abstraction.Data.Tokens;
using DasContract.Abstraction.Processes;
using System.Collections.Generic;
using System.Linq;

namespace DasContract.Abstraction
{
    public class Contract
    {
        public string Id { get; set; }
        /// <summary>
        /// BPMN 2.0 XML with process description and a visual process information. 
        /// </summary>
        public string ProcessDiagram { get; set; }

        /// <summary>
        /// Currently used for backwards compatibility, when only one 
        /// process was allowed.
        /// </summary>
        public Process Process { 
            get { return Processes.ElementAtOrDefault(0); } 
            set { Processes[0] = value; } 
        }

        /// <summary>
        /// List of processes present in the contract.
        /// </summary>
        public IList<Process> Processes { get; set; } = new List<Process>();

        /// <summary>
        /// Data Model
        /// </summary>
        public IEnumerable<DataType> DataTypes { get; set; } = new List<DataType>();

        public IEnumerable<Token> Tokens { get { return DataTypes.OfType<Token>(); } }
        public IEnumerable<Enum> Enums { get { return DataTypes.OfType<Enum>(); } }
        public IEnumerable<Entity> Entities { get { return DataTypes.OfType<Entity>(); } }

        public IList<ProcessRole> Roles { get; set; } = new List<ProcessRole>();
    }
}
