using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using Newtonsoft.Json;
using System;
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

        public string Name { get; set; }

        public string DataModelDefinition { get; set; }

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
        public IEnumerable<Data.Enum> Enums { get { return DataTypes.Values.OfType<Data.Enum>(); } }
        public IEnumerable<Entity> Entities { get { return DataTypes.Values.OfType<Entity>().Except(Tokens); } }

        public IList<ProcessRole> Roles { get; set; } = new List<ProcessRole>();
        public IList<ProcessUser> Users { get; set; } = new List<ProcessUser>();

        public Contract() { }
        public Contract(XElement xElement)
        {
            Id = xElement.Attribute("Id").Value;
            Name = xElement.Element("Name")?.Value;
            Roles = xElement.Element("Roles")?.Elements("ProcessRole")?.Select(r => new ProcessRole(r)).ToList();
            var rolesDict = Roles.ToDictionary(r => r.Id);
            Users = xElement.Element("Users")?.Elements("ProcessUser")?.Select(u => new ProcessUser(u, rolesDict)).ToList();
            var usersDict = Users.ToDictionary(u => u.Id);

            ProcessDiagram = xElement.Element("ProcessDiagram")?.Value;
            Processes = xElement.Element("Processes")?.Elements("Process")?.Select(e => new Process(e, rolesDict, usersDict)).ToList();
            DataModelDefinition = xElement.Element("DataModelDefinition")?.Value;

            XElement xDataModel;
            if (string.IsNullOrEmpty(DataModelDefinition))
            {
                xDataModel = xElement.Element("DataTypes");
                SetDataModelFromXml(xDataModel);
            }
            else
            {
                try
                {
                    xDataModel = XElement.Parse(DataModelDefinition);
                    SetDataModelFromXml(xDataModel);
                }
                catch (Exception) { }
            }
        }

        public void SetDataModelFromXml(XElement xDataModel)
        {
            DataTypes = xDataModel.Elements()?
                .Select(e => CreateDataType(e)).ToDictionary(d => d.Id);
        }

        public XElement ToXElement()
        {
            var xElement =  new XElement("Contract",
                new XAttribute("Id", Id),
                new XElement("Name", Name),
                new XElement("ProcessDiagram", ProcessDiagram),
                new XElement("Processes", Processes.Select(p => p.ToXElement()).ToList()),
                new XElement("Roles", Roles.Select(r => r.ToXElement())),
                new XElement("Users", Users.Select(u => u.ToXElement()))
            );

            if (string.IsNullOrEmpty(DataModelDefinition))
            {
                xElement.Add(new XElement("DataTypes", DataTypes.Select(d => d.Value.ToXElement()).ToList()));
            }
            else
            {
                xElement.Add(new XElement("DataModelDefinition", DataModelDefinition));
            }
            return xElement;
        }

        private DataType CreateDataType(XElement xElement)
        {
            switch (xElement.Name.LocalName)
            {
                case ElementNames.ENTITY: return new Entity(xElement);
                case ElementNames.TOKEN: return new Token(xElement);
                case ElementNames.ENUM: return new Data.Enum(xElement);
                default: throw new Exception($"Invalid data type element name: {xElement.Name.LocalName}");
            }
        }
    }
}
