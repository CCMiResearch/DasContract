using DasContract.Abstraction.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Tasks
{
    public class UserTask : PayableTask
    {
        public UserForm Form { get; set; }

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
        public IEnumerable<ProcessUser> CandidateUsers { get; set; }
        /// <summary>
        /// Process roles allowed to execute this task. 
        /// </summary>
        public IEnumerable<ProcessRole> CandidateRoles { get; set; }

        public string ValidationScript { get; set; }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "UserTask";
            xElement.Add(
                Form?.ToXElement(),
                new XElement("DueDateExpression", DueDateExpression),
                Assignee?.ToXElement(),
                new XElement("ValidationScript", ValidationScript),
                new XElement("CandidateUsers", CandidateUsers.Select(u => u.ToXElement()).ToList()),
                new XElement("CandidateRoles", CandidateRoles.Select(r => r.ToXElement()).ToList()));
            return xElement;
        }
    }
}