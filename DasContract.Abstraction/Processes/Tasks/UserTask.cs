using DasContract.Abstraction.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Tasks
{
    public class UserTask : PayableTask
    {
        public UserForm Form { get; set; } = new UserForm();

        public string FormScript { get; set; }

        /// <summary>
        /// A due date expression such as $(someDate) or an ISO date. 
        /// </summary>
        public string DueDateExpression { get; set; }

        /// <summary>
        /// An user who is assigned to perform a task. May be null. 
        /// </summary>
        public ProcessUser Assignee { get; set; }

        /// <summary>
        /// Process users which are selected to perform this task. 
        /// </summary>
        public IList<ProcessUser> CandidateUsers { get; set; } = new List<ProcessUser>();
        /// <summary>
        /// Process roles allowed to execute this task. 
        /// </summary>
        public IList<ProcessRole> CandidateRoles { get; set; } = new List<ProcessRole>();

        public string ValidationScript { get; set; }

        public UserTask() { }
        public UserTask(XElement xElement, IDictionary<string, ProcessRole> roles, IDictionary<string, ProcessUser> users) : base(xElement)
        {
            DueDateExpression = xElement.Element("DueDateExpression")?.Value;
            ValidationScript = xElement.Element("ValidationScript")?.Value;
            FormScript = xElement.Element("FormScript")?.Value;

            var xUserForm = xElement.Element("UserForm");
            if (xUserForm != null)
                Form = new UserForm(xUserForm);


            var xAssignee = xElement.Element("Assignee");
            if (xAssignee != null)
                Assignee = users[xAssignee.Value];

            CandidateUsers = xElement.Element("CandidateUsers")?.Elements("ProcessUser")?.Select(e => users[e.Value]).ToList()
                ?? CandidateUsers;
            CandidateRoles = xElement.Element("CandidateRoles")?.Elements("ProcessRole")?.Select(e => roles[e.Value]).ToList()
                ?? CandidateRoles;
        }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "UserTask";

            if(Assignee != null)
            {
                xElement.Add(new XElement("Assignee", Assignee.Id));
            }

            xElement.Add(
                Form?.ToXElement(),
                new XElement("DueDateExpression", DueDateExpression),
                new XElement("FormScript", FormScript),
                new XElement("ValidationScript", ValidationScript),
                new XElement("CandidateUsers", CandidateUsers?.Select(u => new XElement("ProcessUser", u.Id)).ToList()),
                new XElement("CandidateRoles", CandidateRoles?.Select(r => new XElement("ProcessRole", r.Id)).ToList())); 
            return xElement;
        }
    }
}