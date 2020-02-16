using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Exceptions
{
    public class ContractException: Exception
    {
        public ContractException()
            :base()
        {

        }

        public ContractException(string message)
            :base(message)
        {

        }
    }
}
