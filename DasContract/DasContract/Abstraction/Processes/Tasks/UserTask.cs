using DasContract.Abstraction.Interface.Processes.Tasks;
using DasContract.Abstraction.UserInterface;
using System;
using System.Collections.Generic;

namespace DasContract.Abstraction.Processes.Tasks
{
    public class UserTask : Task, ICustomDataCopyable<UserTask>
    {
        public UserForm Form { get; set; }

        /// <summary>
        /// A due date expression such as $(someDate) or an ISO date. 
        /// </summary>
        public string DueDateExpression { get; set; }

        /*/// <summary>
        /// A user who is assigned to perform a task. May be null. 
        /// </summary>
        public ProcessUser Assignee { get; set; }

        /// <summary>
        /// Process users which are selected to perform this task. 
        /// </summary>
        public List<ProcessUser> CandidateUsers { get; set; }
        /// <summary>
        /// Process roles allowed to execute this task. 
        /// </summary>
        public List<ProcessRole> CandidateRoles { get; set; }*/

        public void CopyCustomDataFrom(UserTask task)
        {
            Form = task.Form;
            DueDateExpression = task.DueDateExpression;
        }
    }
}
