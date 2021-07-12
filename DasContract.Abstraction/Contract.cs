using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

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
        public Process Process
        {
            get { return Processes.ElementAtOrDefault(0); }
            set { Processes[0] = value; }
        }



        public bool TryGetProcess(string processId, out Process process)
        {
            var search = Processes.Where(p => p.Id == processId);
            if (search.Count() == 0)
            {
                process = null;
                return false;
            }
            process = search.First();
            return true;
        }

        public bool TryGetProperty(string propertyId, out Property property, out Entity outEntity)
        {
            foreach (var entity in Entities)
            {
                var search = entity.Properties.Where(p => p.Id == propertyId);
                if (search.Count() > 0)
                {
                    property = search.First();
                    outEntity = entity;
                    return true;
                }
            }
            property = null;
            outEntity = null;
            return false;
        }

        public bool TryGetProperty(string propertyId, out Property property)
        {
            return TryGetProperty(propertyId, out property, out _);
        }

        /// <summary>
        /// List of processes present in the contract.
        /// </summary>
        public IList<Process> Processes { get; set; } = new List<Process>();

        /// <summary>
        /// Data Model
        /// </summary>
        public IDictionary<string, DataType> DataTypes { get; set; } = new Dictionary<string, DataType>();

        public IEnumerable<Token> Tokens { get { return DataTypes.Values.OfType<Token>(); } }
        public IEnumerable<Enum> Enums { get { return DataTypes.Values.OfType<Enum>(); } }
        public IEnumerable<Entity> Entities { get { return DataTypes.Values.OfType<Entity>().Except(Tokens); } }

        public IList<ProcessRole> Roles { get; set; } = new List<ProcessRole>();
        public IList<ProcessUser> Users { get; set; } = new List<ProcessUser>();

        public Contract() { }
        public Contract(XElement xElement)
        {
            Id = xElement.Attribute("Id").Value;
            ProcessDiagram = xElement.Element("ProcessDiagram")?.Value;
            Processes = xElement.Element("Processes")?.Elements("Process")?.Select(e => new Process(e)).ToList();
            DataTypes = CreateDataTypes(xElement.Element("DataTypes"));
            Users = xElement.Element("Users")?.Elements("ProcessUser")?.Select(u => new ProcessUser(u)).ToList();
            Roles = xElement.Element("Roles")?.Elements("ProcessRole")?.Select(r => new ProcessRole(r)).ToList();
        }

        private IDictionary<string, DataType> CreateDataTypes(XElement xElement)
        {
            IEnumerable<DataType> dataTypes = new List<DataType>();
            if (xElement != null)
            {
                var tokens = xElement.Elements("Token")?.Select(e => new Token(e));
                var enums = xElement.Elements("Enum")?.Select(e => new Enum(e));
                var entities = xElement.Elements("Entity")?.Select(e => new Entity(e));
                if (tokens != null)
                    dataTypes = dataTypes.Concat(tokens);
                if (enums != null)
                    dataTypes = dataTypes.Concat(enums);
                if (entities != null)
                    dataTypes = dataTypes.Concat(entities);
            }
            return dataTypes.ToDictionary(d => d.Id);
        }

        public XElement ToXElement()
        {
            return new XElement("Contract",
                new XAttribute("Id", Id),
                new XElement("ProcessDiagram", ProcessDiagram),
                new XElement("Processes", Processes.Select(p => p.ToXElement()).ToList()),
                new XElement("DataTypes", DataTypes.Select(d => d.Value.ToXElement()).ToList()),
                new XElement("Roles", Roles.Select(r => r.ToXElement())),
                new XElement("Users", Users.Select(u => u.ToXElement()))
            );
        }
    }
}
