using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.UserInterface
{
    public class UserForm
    {
        public string Id { get; set; }
        public IList<FormField> Fields { get; set; }

    }
}
