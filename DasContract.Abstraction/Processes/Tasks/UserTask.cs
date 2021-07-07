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
        public IEnumerable<ProcessUser> CandidateUsers { get; set; } = new List<ProcessUser>();
        /// <summary>
        /// Process roles allowed to execute this task. 
        /// </summary>
        public IEnumerable<ProcessRole> CandidateRoles { get; set; } = new List<ProcessRole>();

        public string ValidationScript { get; set; }

        public UserTask() { }
        public UserTask(XElement xElement) : base(xElement)
        {
            DueDateExpression = xElement.Element("DueDateExpression")?.Value;
            ValidationScript = xElement.Element("ValidationScript")?.Value;

            var xUserForm = xElement.Element("UserForm");
            if (xUserForm != null)
                Form = new UserForm(xUserForm);

            var xAssignee = xElement.Element("Assignee");
            if (xAssignee != null)
                Assignee = new ProcessUser(xAssignee);

            CandidateUsers = xElement.Element("CandidateUsers")?.Elements("ProcessUser")?.Select(e => new ProcessUser(e)).ToList()
                ?? CandidateUsers;
            CandidateRoles = xElement.Element("CandidateRoles")?.Elements("ProcessRole")?.Select(e => new ProcessRole(e)).ToList()
                ?? CandidateRoles;
        }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "UserTask";

            var xAssignee = Assignee?.ToXElement();
            if (xAssignee != null)
                xAssignee.Name = "Assignee";

            xElement.Add(
                Form?.ToXElement(),
                xAssignee,
                new XElement("DueDateExpression", DueDateExpression),
                new XElement("ValidationScript", ValidationScript),
                new XElement("CandidateUsers", CandidateUsers?.Select(u => u.ToXElement()).ToList()),
                new XElement("CandidateRoles", CandidateRoles?.Select(r => r.ToXElement()).ToList()));
            return xElement;
        }
    }
}