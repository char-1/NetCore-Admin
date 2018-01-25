using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Admin.Utility.Exceptions
{
    public class NotEqualException : Exception
    {
        public NotEqualException(string message)
            : base(message)
        {
        }
    }
}
