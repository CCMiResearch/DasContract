using DasContract.DasContract.Abstraction.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.UserInterface
{
    public class UserForm: IIdentifiable
    {
        public string Id { get; set; }

        /// <summary>
        /// Field of this user form
        /// </summary>
        public List<FormField> Fields { get; set; } = new List<FormField>();
    }
}
